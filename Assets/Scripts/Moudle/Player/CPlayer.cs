using UnityEngine;
using System;
using System.Collections.Generic;

public class CPlayer:Controller {
    private ModelPlayer m_player;
    private Vector3 m_dirPlayerMove = Vector3.zero;
    private Transform m_cameraTrans;
    private float m_cameraHeight;
    private float m_cameraToPlayerDis;

    public CPlayer(GameMoudle moudle, Type modelType):base(moudle, modelType) { }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.InitPlayer);
        m_eventList.Add(GameEvent.Type.SteeringWheelDragBegin);
        m_eventList.Add(GameEvent.Type.SteeringWheelDraging);
        m_eventList.Add(GameEvent.Type.SteeringWheelDragEnd);
    }

    public override Action<object> GetEvent(GameEvent.Type eventType) {
        switch (eventType) {
            case GameEvent.Type.InitPlayer:
                return InitPlayer;
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

    private void InitPlayer(object arg) {
        Dictionary<string, GameObject> dicNodeName = arg as Dictionary<string, GameObject>;
        GameObject playerObj = dicNodeName["Blade_Warrior_Prefab"];
        m_player = playerObj.AddComponent<ModelPlayer>();

        m_cameraTrans = dicNodeName[GameConst.PlayerCamera].transform;
        m_cameraHeight = m_cameraTrans.position.y - GameConfig.CameraHeightFix;
        m_cameraToPlayerDis = m_player.transform.position.z - m_cameraTrans.position.z;
    }

    private void SteeringWheelDragBegin(object arg) {
        SteeringWheelDraging(arg);
        switch (GameConfig.CameraType) {
            case GameCameraType.ThirdPerson:
                EventManager.Register(GameEvent.Type.LastUpdate, ResetCameraThirdPersonMode);
                break;
        }
    }

    private void SteeringWheelDraging(object arg) {
        Vector2 direction2D = (Vector2)arg;
        switch(GameConfig.CameraType) {
            case GameCameraType.Fix:
                MovePlayerFixMode(direction2D);
                break;
            case GameCameraType.ThirdPerson:
                MovePlayerThirdPersonMode(direction2D);
                break;
        }
    }

    private void SteeringWheelDragEnd(object arg) {
        m_player.Stop();
        switch(GameConfig.CameraType) {
            case GameCameraType.ThirdPerson:
                m_player.EndMove();
                break;
        }
    }

    private void MovePlayerFixMode(Vector2 direction2D) {
        direction2D = direction2D.normalized;
        m_dirPlayerMove.x = direction2D.x;
        m_dirPlayerMove.z = direction2D.y;
        m_player.Move(m_dirPlayerMove, Quaternion.LookRotation(m_dirPlayerMove).eulerAngles.y);
    }

    private void MovePlayerThirdPersonMode(Vector2 direction2D) {
        direction2D = direction2D.normalized;
        m_dirPlayerMove.x = direction2D.x;
        m_dirPlayerMove.z = direction2D.y;
        Quaternion playerRotation = Quaternion.LookRotation(m_dirPlayerMove, Vector3.up);
        float rotationY = playerRotation.eulerAngles.y;
        m_player.Move(m_dirPlayerMove, rotationY);
    }

    private void ResetCameraThirdPersonMode(object arg = null) {
        Quaternion cameraRotation = m_cameraTrans.rotation;
        Quaternion playerRotation = m_player.transform.rotation;
        if (Math.Abs(cameraRotation.eulerAngles.y - playerRotation.eulerAngles.y) < 1 && m_player.RoleState == ModelRole.State.Stand) {
            EventManager.Unregister(GameEvent.Type.LastUpdate, ResetCameraThirdPersonMode);
            return;
        }
        cameraRotation = Quaternion.Lerp(cameraRotation, playerRotation, Time.deltaTime * GameConfig.CameraMoveFixTime);
        m_cameraTrans.rotation = cameraRotation;
        Vector3 disCamera = cameraRotation * Vector3.forward * m_cameraToPlayerDis;
        Vector3 cameraPos = m_player.transform.position - disCamera;
        cameraPos.y = m_cameraHeight;
        m_cameraTrans.position = Vector3.Lerp(m_cameraTrans.position, cameraPos, Time.deltaTime * GameConfig.CameraMoveFixTime);
    }
}