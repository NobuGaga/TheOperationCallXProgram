using UnityEngine;
using System;
using System.Collections.Generic;

public class CPlayer:Controller {
    private ModelPlayer m_player;
    private Transform m_playerTrans;
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
        m_player = playerObj.GetComponent<ModelPlayer>();
        m_playerTrans = playerObj.transform;

        if (GameConfig.CameraType == GameCameraType.Fix)
            return;
        m_cameraTrans = dicNodeName[GameConst.PlayerCamera].transform;
        m_cameraHeight = m_cameraTrans.position.y;
        m_cameraToPlayerDis = (playerObj.transform.position - m_cameraTrans.position).magnitude;
    }

    private void SteeringWheelDragBegin(object arg) {
        SteeringWheelDraging(arg);
    }

    private void SteeringWheelDraging(object arg) {
        Vector2 direction2D = ((Vector2)arg).normalized;
        m_dirPlayerMove.x = direction2D.x;
        m_dirPlayerMove.z = direction2D.y;
        Quaternion playerRotation = Quaternion.LookRotation(m_dirPlayerMove);
        float rotationY = playerRotation.eulerAngles.y;
        m_player.Move(m_dirPlayerMove, rotationY);

        if (GameConfig.CameraType == GameCameraType.Fix)
            return;
        m_cameraTrans.rotation = Quaternion.Euler(Vector3.up * rotationY);
        Vector3 disCamera = playerRotation * Vector3.forward * m_cameraToPlayerDis;
        Vector3 cameraPos = m_playerTrans.position - disCamera;
        cameraPos.y = m_cameraHeight;
        m_cameraTrans.position = cameraPos;
    }

    private void SteeringWheelDragEnd(object arg) {
        m_player.Stop();
    }
}