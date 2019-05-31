using UnityEngine;
using System;
using System.Collections.Generic;

public class CPlayer:Controller {
    private PlayerView m_playerView;
    private ModelPlayer m_player;
    private Transform m_cameraTrans;
    private float m_cameraRotationY;
    private float m_cameraHeight;
    private float m_cameraToPlayerDis;
    private Vector3 m_cameraToPlayerDisVec3;

    public CPlayer(GameMoudle moudle, Type modelType):base(moudle, modelType) { }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.OpenMainView);
        m_eventList.Add(GameEvent.Type.Attack);
        m_eventList.Add(GameEvent.Type.Damage);
        m_eventList.Add(GameEvent.Type.ShowDamageText);
        m_eventList.Add(GameEvent.Type.MonsterCreate);
        m_eventList.Add(GameEvent.Type.MonsterMove);
        m_eventList.Add(GameEvent.Type.MonsterDamage);
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
            case GameEvent.Type.ShowDamageText:
                return ShowDamageText;
            case GameEvent.Type.MonsterCreate:
                return MonsterCreate;
            case GameEvent.Type.MonsterMove:
                return MonsterMove;
            case GameEvent.Type.MonsterDamage:
                return MonsterDamage;
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
        Dictionary<string, GameObject> dicNodeName = arg as Dictionary<string, GameObject>;
        string playerPrefabName = "Blade_Warrior_Prefab";
        GameObject playerObj = dicNodeName[playerPrefabName];
        ModelAttackRoleData data = new ModelAttackRoleData(GetModel<MPlayerData>().PlayerHP, 0.5f, 30);
        m_player = new ModelPlayer(playerObj, data);

        m_cameraTrans = dicNodeName[GameConst.PlayerCamera].transform;
        m_cameraRotationY = m_cameraTrans.rotation.eulerAngles.y;
        m_cameraHeight = m_cameraTrans.position.y - GameConfig.CameraHeightFix;
        m_cameraToPlayerDis = m_player.transform.position.z - m_cameraTrans.position.z;
        m_cameraToPlayerDisVec3 = m_player.transform.position - m_cameraTrans.position;

        GameView viewType = GameView.MainView;
        ViewManager.Open(GameViewInfo.GetViewName(Moudle, GameView.MainView),
            (GameObject gameObject) => {
                m_playerView = new PlayerView(Moudle, viewType, gameObject.GetComponent<UIPrefab>());
                Vector3 position = GameSceneManager.ToScreenPoint(playerObj.transform.position);
                position.y += ModelRoleManager.GetModelRoleHpPosY(playerPrefabName);
                m_playerView.CreateHPProcess(playerPrefabName, position, true);
            }
        );

        EventManager.Register(GameEvent.Type.FrameUpdate, FrameUpdate);
    }

    private void Attack(object arg) {
        ModelAttackLevel level = (ModelAttackLevel)arg;
        m_player.AttackLevel = level;
        m_player.State = SRoleState.Type.SRoleAttack;
    }

    private void Damage(object arg) {
        ModelAttackData data = (ModelAttackData)arg;
        float percent = m_player.Damage(data);
        m_playerView.UpdateHP(percent);
    }

    private void ShowDamageText(object arg) {
        if (!(arg is KeyValuePair<Vector3, int>))
            return;
        KeyValuePair<Vector3, int> data = (KeyValuePair<Vector3, int>)arg;
        m_playerView.ShowDamageText(data.Key, data.Value);
    }

    private void MonsterCreate(object arg) {
        KeyValuePair<string, Vector3> data = (KeyValuePair<string, Vector3>)arg;
        m_playerView.CreateHPProcess(data.Key, data.Value);
    }

    private void MonsterMove(object arg) {
        KeyValuePair<string, Vector3> data = (KeyValuePair<string, Vector3>)arg;
        m_playerView.MoveHPProcess(data.Key, data.Value);
    }

    private void MonsterDamage(object arg) {
        KeyValuePair<string, float> data = (KeyValuePair<string, float>)arg;
        m_playerView.UpdateHPProcess(data.Key, data.Value);
    }

    private void SteeringWheelDragBegin(object arg) {
        SteeringWheelDraging(arg);
        switch (GameConfig.CameraType) {
            case GameCameraType.Fix:
                EventManager.Register(GameEvent.Type.LastUpdate, LastUpdateFixMode);
                break;
            case GameCameraType.ThirdPerson:
                EventManager.Register(GameEvent.Type.LastUpdate, LastUpdateThirdPersonMode);
                break;
        }
    }

    private void SteeringWheelDraging(object arg) {
        m_player.SetVelocityAndRotation((Vector2)arg, m_cameraRotationY);
    }

    private void SteeringWheelDragEnd(object arg) {
        m_player.State = SRoleState.Type.SRoleStand;
    }

    private void FrameUpdate(object arg = null) {
        m_playerView?.Update();
        m_player.Update();
    }

    private void LastUpdateFixMode(object arg) {
        PlayerHPUpdatePosition();
        ResetCameraFixMode();
    }

    private void LastUpdateThirdPersonMode(object arg) {
        PlayerHPUpdatePosition();
        ResetCameraThirdPersonMode();
    }

    private void PlayerHPUpdatePosition() {
        Vector3 position = GameSceneManager.ToScreenPoint(m_player.transform.position);
        position.y += ModelRoleManager.GetModelRoleHpPosY(m_player.gameObject.name);
        m_playerView?.MoveHPProcess(string.Empty, position, true);
    }

    private void ResetCameraFixMode() {
        Vector3 cameraPos = m_player.transform.position - m_cameraToPlayerDisVec3;
        Vector3 cameraLastPos = m_cameraTrans.position;
        if ((cameraPos - cameraLastPos).magnitude < 0.01f && m_player.State == SRoleState.Type.SRoleStand) {
            EventManager.Unregister(GameEvent.Type.LastUpdate, LastUpdateFixMode);
            return;
        }
        m_cameraTrans.position = Vector3.Lerp(cameraLastPos, cameraPos, Time.deltaTime * GameConfig.CameraMoveFixModeTime);
    }

    private void ResetCameraThirdPersonMode() {
        Quaternion cameraRotation = m_cameraTrans.rotation;
        Quaternion playerRotation = m_player.transform.rotation;
        if (Math.Abs(cameraRotation.eulerAngles.y - playerRotation.eulerAngles.y) < 1 && m_player.State == SRoleState.Type.SRoleStand) {
            EventManager.Unregister(GameEvent.Type.LastUpdate, LastUpdateThirdPersonMode);
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