using UnityEngine;
using System;

public class CSelect:Controller {
    private SelectView m_selectView;

    public CSelect(GameMoudle Moudle, Type modelType):base(Moudle, modelType) { }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.OpenMainView);
        m_eventList.Add(GameEvent.Type.Click);
        m_eventList.Add(GameEvent.Type.CloseMainView);
    }

    public override Action<object> GetEvent(GameEvent.Type eventType) {
        switch (eventType) {
            case GameEvent.Type.OpenMainView:
                return OpenMainView;
            case GameEvent.Type.Click:
                return ClickEvent;
            case GameEvent.Type.CloseMainView:
                return CloseMainView;
            default:
                return null;
        }
    }

    public void OpenMainView(object arg) {
        GameView viewType = GameView.MainView;
        ViewManager.Open(GameViewInfo.GetViewName(Moudle, GameView.MainView), 
            (GameObject gameObject) => {
                m_selectView = new SelectView(Moudle, viewType, gameObject.GetComponent<UIPrefab>());
                m_selectView.ShowSelectScene(GetModel<MSelectData>().Scenes);
            }
       );
    }

    public void ClickEvent(object arg) {
        if (arg is int) {
            EventManager.Dispatch(GameEvent.Type.ChangeScene, GetModel<MSelectData>().GetScene((int)arg));
            EventManager.Dispatch(GameMoudle.Select, GameEvent.Type.CloseMainView);
        }
    }

    public void CloseMainView(object arg) {
        m_selectView = null;
    }

    ~CSelect() {
        m_selectView = null;
    }
}