using System;
using System.Collections.Generic;

public abstract class Controller :IDisposable {
    protected List<GameEvent.Type> m_eventList = new List<GameEvent.Type>();
    public List<GameEvent.Type> EventList { 
        get {
            return m_eventList;
        }
    }

    protected Model m_data;
    public Controller() {
        InitModel();
        InitEvent();
    }
    protected abstract void InitModel();
    protected abstract void InitEvent();
    public abstract Action GetEvent(GameEvent.Type eventType);

    public void Dispose() {
        m_data.Dispose();
    }
}
