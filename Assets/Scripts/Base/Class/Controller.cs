using System;
using System.Collections.Generic;

public abstract class Controller:IDisposable {
    protected List<GameEvent.Type> m_eventList = new List<GameEvent.Type>();
    public List<GameEvent.Type> EventList => m_eventList;

    private GameMoudle m_moudle;
    protected GameMoudle Moudle => m_moudle;
    protected Model m_data;
    public Controller(GameMoudle moudle, Type modelType = null) {
        m_moudle = moudle;
        InitEvent();
        if (modelType == null)
            return;
        else if (modelType.IsSubclassOf(typeof(Model)))
            m_data = Activator.CreateInstance(modelType) as Model;
        else
            DebugTool.LogError("Controller Model Type Error");
    }
    protected abstract void InitEvent();
    public abstract Action<object> GetEvent(GameEvent.Type eventType);

    protected T GetModel<T>() where T:Model {
        if (m_data == null)
            return null;
        return m_data as T;
    }

    public void Dispose() {
        if (m_data == null)
            return;
        m_data.Dispose();
    }
}