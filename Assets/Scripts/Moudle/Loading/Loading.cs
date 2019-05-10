using UnityEngine;
using System;

public class Loading:Controller {
    private LoadingView m_loadingView;
    private float m_startTime;

    protected override void InitModel() {
        m_data = new LoadingData();
    }

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
        AssetBundleLoader.Load(GameManager.m_curLogicScript, "prefabs/moudle/loading/ui", "LoadingMainView",
            delegate (GameObject gameObj) {
                GameObject view = UnityEngine.Object.Instantiate(gameObj) as GameObject;
                view.transform.SetParent(GameManager.m_curLogicScript.gameObject.transform, false);
                m_loadingView = new LoadingView(view.GetComponent<UIView>());
                EventManager.Register(GameEvent.Type.FrameUpdate, OnFrameUpdate);
                m_startTime = Time.time;
            }
        );
    }

    public void OnFrameUpdate(object arg) {
        float passTime = (float)arg - m_startTime;
        if (passTime >= 1) {
            m_loadingView.UpdateLoadingProcess(1);
            EventManager.Unregister(GameEvent.Type.FrameUpdate, OnFrameUpdate);
            m_startTime = Time.time;
            EventManager.Register(GameEvent.Type.FrameUpdate, OnFrameUpdate);
        }
        else
            m_loadingView.UpdateLoadingProcess(passTime);
    }
}
