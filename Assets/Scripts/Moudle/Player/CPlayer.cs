using UnityEngine;
using System;
using System.Collections.Generic;

public class CPlayer:Controller {
    private PlayerView m_playerView;
    private ModelPlayer m_player;
    private Vector3 m_playerLastPosition;
    private Transform m_cameraTrans;
    private float m_cameraRotationY;
    private float m_cameraHeight;
    private float m_cameraToPlayerDis;

    public CPlayer(GameMoudle moudle, Type modelType):base(moudle, modelType) { }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.OpenMainView);
        m_eventList.Add(GameEvent.Type.Attack);
        m_eventList.Add(GameEvent.Type.Damage);
        m_eventList.Add(GameEvent.Type.SteeringWheelDragBegin);
        m_eventList.Add(GameEvent.Type.SteeringWheelDraging);
        m_eventList.Add(GameEvent.Type.SteeringWheelDragEnd);
    }

    public override Action<object> GetEvent(GameEvent.Type eventType) {
        switch (eventType) {
            case GameEvent.Type.OpenMainView:
                return OpenMainView;
            case GameEvent.Type.Attack:
                return Attack;
            case GameEvent.Type.Damage:
                return Damage;
            case GameEvent.Type.SteeringWheelDragBegin:
                return SteeringWheelDragBegin;
            case GameEvent.Type.SteeringWheelDraging:
                return SteeringWheelDraging;
            case GameEvent.Type.SteeringWheelDragEnd:
                return SteeringWheelDragEnd;
            default:
                return null;
        }
    }

    private void OpenMainView(object arg) {
        GameView viewType = GameView.MainView;
        ViewManager.Open(GameViewInfo.GetViewName(Moudle, GameView.MainView),
            (GameObject gameObject) =>
                m_playerView = new PlayerView(Moudle, viewType, gameObject.GetComponent<UIPrefab>())
        );

        Dictionary<string, GameObject> dicNodeName = arg as Dictionary<string, GameObject>;
        GameObject playerObj = dicNodeName["Blade_Warrior_Prefab"];
        m_player = playerObj.AddComponent<ModelPlayer>();
        m_player.Init(GetModel<MPlayerData>().PlayerHP);

        m_cameraTrans = dicNodeName[GameConst.PlayerCamera].transform;
        m_cameraRotationY = m_cameraTrans.rotation.y;
        m_cameraHeight = m_cameraTrans.position.y - GameConfig.CameraHeightFix;
        m_cameraToPlayerDis = m_player.transform.position.z - m_cameraTrans.position.z;

        EventManager.Register(GameEvent.Type.FrameUpdate, PlayerUpdate);
    }

    private void Attack(object arg) {
        ModelAttackLevel level = (ModelAttackLevel)arg;
        if (level == ModelAttackLevel.Normal)
            m_player.State = RoleState.Type.SRoleAttack;
    }

    private void Damage(object arg) {
        ModelAttackData data = (ModelAttackData)arg;
        float percent = m_player.Damage(data);
        m_playerView.UpdateHP(percent);
    }

    private void SteeringWheelDragBegin(object arg) {
        SteeringWheelDraging(arg);
        switch (GameConfig.CameraType) {
            case GameCameraType.Fix:
                m_playerLastPosition = m_player.transform.position;
                EventManager.Register(GameEvent.Type.LastUpdate, ResetCameraFixMode);
                break;
            case GameCameraType.ThirdPerson:
                EventManager.Register(GameEvent.Type.LastUpdate, ResetCameraThirdPersonMode);
                break;
        }
    }

    private void SteeringWheelDraging(object arg) {
        m_player.SetVelocityAndRotation((Vector2)arg, m_cameraRotationY);
    }

    private void SteeringWheelDragEnd(object arg) {
        //DebugTool.Log(m_player.BeforeMoveFoward);
        //DebugTool.Log(m_player.transform.rotation.eulerAngles.y);
        m_player.State = RoleState.Type.SRoleStand;
    }

    private void PlayerUpdate(object arg = null) {
        m_player.UpdateState();
    }

    private void ResetCameraFixMode(object arg = null) {
        Vector3 playerCurPos = m_player.transform.position;
        Vector3 playerMoveDis = playerCurPos - m_playerLastPosition;
        if (playerMoveDis.magnitude < 0.01f && m_player.State == RoleState.Type.SRoleStand) {
            EventManager.Unregister(GameEvent.Type.LastUpdate, ResetCameraThirdPersonMode);
            return;
        }
        m_playerLastPosition = playerCurPos;
        Vector3 cameraLastPos = m_cameraTrans.position;
        Vector3 cameraPos = playerMoveDis + cameraLastPos;
        m_cameraTrans.position = Vector3.Lerp(cameraLastPos, cameraPos, Time.deltaTime * GameConfig.CameraMoveFixModeTime);
    }

    private void ResetCameraThirdPersonMode(object arg = null) {
        Quaternion cameraRotation = m_cameraTrans.rotation;
        Quaternion playerRotation = m_player.transform.rotation;
        if (Math.Abs(cameraRotation.eulerAngles.y - playerRotation.eulerAngles.y) < 1 && m_player.State == RoleState.Type.SRoleStand) {
            EventManager.Unregister(GameEvent.Type.LastUpdate, ResetCameraThirdPersonMode);
            return;
        }
        cameraRotation = Quaternion.Lerp(cameraRotation, playerRotation, Time.deltaTime * GameConfig.CameraMoveThirdModeTime);
        m_cameraTrans.rotation = cameraRotation;
        Vector3 disCamera = cameraRotation * Vector3.forward * m_cameraToPlayerDis;
        Vector3 cameraPos = m_player.transform.position - disCamera;
        cameraPos.y = m_cameraHeight;
        m_cameraTrans.position = Vector3.Lerp(m_cameraTrans.position, cameraPos, Time.deltaTime * GameConfig.CameraMoveThirdModeTime);
    }
}