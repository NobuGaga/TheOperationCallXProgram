using UnityEngine;
using System;

public class CSelect:Controller {
    private SelectView m_selectView;

    public CSelect(GameMoudle moudle, Type modelType):base(moudle, modelType) { }

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
        ViewManager.Open(GameViewInfo.GetViewName(GameMoudle.Select, GameView.MainView), 
            delegate (GameObject gameObject) {
                m_selectView = new SelectView(gameObject.GetComponent<UIPrefab>());
            }
        );
    }
}