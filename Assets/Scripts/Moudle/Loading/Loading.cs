using UnityEngine;
using System;

public class Loading:Controller {
    private LoadingView m_loadingView;
    private float m_startTime;
    private bool isLoadScene = false;

    public Loading(GameMoudle moudle, Type modelType):base(moudle, modelType) { }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.OpenMainView);
    }

    public override Action GetEvent(GameEvent.Type eventType) {
        switch (eventType) {
            case GameEvent.Type.OpenMainView:
                return OpenMainView;
            default:
                return null;
        }
    }

    public void OpenMainView() {
        if (m_loadingView == null)
            ViewManager.Open(GameViewInfo.GetViewName(GameMoudle.Loading, GameView.MainView), 
                delegate (GameObject gameObject) {
                    m_loadingView = new LoadingView(gameObject.GetComponent<UIView>());
                    EventManager.Register(GameEvent.Type.FrameUpdate, OnStartGameFrameUpdate);
                    m_startTime = Time.time;
                }
            );
        else {
            m_loadingView.Show();
            EventManager.Register(GameEvent.Type.FrameUpdate, OnFrameUpdate);
        }
    }

    public void OnStartGameFrameUpdate(object arg) {
        float process;
        if (isLoadScene) {
            float sceneProcess = GameManager.LoadSceneProcess;
            if (sceneProcess < 0) {
                EventManager.Unregister(GameEvent.Type.FrameUpdate, OnStartGameFrameUpdate);
                DebugTool.LogError("Load Scene Error Current Scene : " + GameManager.CurScene.ToString());
                return;
            }
            process = (GameConfig.StartGameLoadTime + sceneProcess - 1) / GameConfig.StartGameLoadTime;
        }
        else {
            float passTime = Time.time - m_startTime;
            bool isStartLoadScene = passTime > GameConfig.StartGameLoadTime - 1;
            if (isStartLoadScene) {
                EventManager.Dispatch(GameEvent.Type.ChangeScene, GameScene.SelectScene);
                isLoadScene = true;
            }
            process = passTime / GameConfig.StartGameLoadTime;
        }
        if (process >= 1) {
            m_loadingView.UpdateLoadingProcess(1);
            EventManager.Unregister(GameEvent.Type.FrameUpdate, OnStartGameFrameUpdate);
            CloseMainView();
        }
        else
            m_loadingView.UpdateLoadingProcess(process);
    }

    public void OnFrameUpdate(object arg) {
        float process = GameManager.LoadSceneProcess;
        if (process < 0) {
            EventManager.Unregister(GameEvent.Type.FrameUpdate, OnFrameUpdate);
            DebugTool.LogError("Load Scene Error Current Scene : " + GameManager.CurScene.ToString());
            return;
        }
        if (process >= 1) {
            m_loadingView.UpdateLoadingProcess(1);
            EventManager.Unregister(GameEvent.Type.FrameUpdate, OnFrameUpdate);
            CloseMainView();
        }
        else
            m_loadingView.UpdateLoadingProcess(process);
    }

    public void CloseMainView() {
        m_loadingView.Hide();
    }
}