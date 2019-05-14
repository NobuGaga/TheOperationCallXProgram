using UnityEngine;
using System;

public class CSelect:Controller {
    private SelectView m_selectView;

    public CSelect(GameMoudle Moudle, Type modelType):base(Moudle, modelType) { }

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
        GameView viewType = GameView.MainView;
        ViewManager.Open(GameViewInfo.GetViewName(Moudle, GameView.MainView), 
            (GameObject gameObject) =>
                m_selectView = new SelectView(GetModel<MSelectData>().SceneCount,
                                                Moudle, viewType, gameObject.GetComponent<UIPrefab>())
        );
    }

    ~CSelect() {
        m_selectView = null;
    }
}