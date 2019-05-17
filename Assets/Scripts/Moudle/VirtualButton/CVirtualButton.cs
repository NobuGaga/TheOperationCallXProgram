using UnityEngine;
using System;

public class CVirtualButton:Controller {
    private VirtualButtonView m_virtualButtonView;

    public CVirtualButton(GameMoudle moudle):base(moudle) { }

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
        if (m_virtualButtonView == null) {
            GameView viewType = GameView.MainView;
            ViewManager.Open(GameViewInfo.GetViewName(Moudle, viewType), 
                (GameObject gameObject) =>
                    m_virtualButtonView = new VirtualButtonView(Moudle, viewType, gameObject.GetComponent<UIPrefab>())
            );
        } else
            m_virtualButtonView.Show();
    }

    public void CloseMainView(object arg) {
        m_virtualButtonView.Hide();
    }

    ~CVirtualButton() {
        m_virtualButtonView = null;
    }
}