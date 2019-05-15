using UnityEngine;
using System;

public class CLoading:Controller {
    private LoadingView m_loadingView;
    private float m_startTime;
    private bool isLoadScene = false;

    public CLoading(GameMoudle moudle, Type modelType):base(moudle, modelType) { }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.OpenMainView);
        m_eventList.Add(GameEvent.Type.CloseMainView);
    }

    public override Action<object> GetEvent(GameEvent.Type eventType) {
        switch (eventType) {
            case GameEvent.Type.OpenMainView:
                return OpenMainView;
            case GameEvent.Type.CloseMainView:
                return CloseMainView;
            default:
                return null;
        }
    }

    public void OpenMainView(object arg) {
        if (m_loadingView == null) {
            GameView viewType = GameView.MainView;
            ViewManager.Open(GameViewInfo.GetViewName(Moudle, viewType), 
                (GameObject gameObject) => {
                    m_loadingView = new LoadingView(Moudle, viewType, gameObject.GetComponent<UIPrefab>());
                    EventManager.Register(GameEvent.Type.FrameUpdate, OnStartGameFrameUpdate);
                    m_startTime = Time.time;
                }
            );
        } else {
            m_loadingView.Show();
            EventManager.Register(GameEvent.Type.FrameUpdate, OnFrameUpdate);
        }
    }

    public void OnStartGameFrameUpdate(object arg) {
        float process;
        if (isLoadScene) {
            float sceneProcess = GameSceneManager.LoadSceneProcess;
            if (sceneProcess < 0) {
                EventManager.Unregister(GameEvent.Type.FrameUpdate, OnStartGameFrameUpdate);
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
            EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.CloseMainView);
        }
        else
            m_loadingView.UpdateLoadingProcess(process);
    }

    public void OnFrameUpdate(object arg) {
        float process = GameSceneManager.LoadSceneProcess;
        if (process < 0) {
            EventManager.Unregister(GameEvent.Type.FrameUpdate, OnFrameUpdate);
            return;
        }
        if (process >= 1) {
            m_loadingView.UpdateLoadingProcess(1);
            EventManager.Unregister(GameEvent.Type.FrameUpdate, OnFrameUpdate);
            EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.CloseMainView);
        }
        else
            m_loadingView.UpdateLoadingProcess(process);
    }

    public void CloseMainView(object arg) {
        m_loadingView.Hide();
    }

    ~CLoading() {
        m_loadingView = null;
    }
}