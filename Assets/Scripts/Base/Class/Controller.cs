﻿using System;
using System.Collections.Generic;

public abstract class Controller :IDisposable {
    protected List<GameEvent.Type> m_eventList = new List<GameEvent.Type>();
    public List<GameEvent.Type> EventList => m_eventList;

    protected GameMoudle m_moudle;
    protected Model m_data;
    private Type m_modelType;
    public Controller(GameMoudle moudle, Type modelType) {
        m_moudle = moudle;
        m_modelType = modelType;
        if (modelType.IsSubclassOf(typeof(Model)))
            m_data = Activator.CreateInstance(modelType) as Model;
        else
            DebugTool.LogError("Controller Model Type Error");
        InitEvent();
    }
    protected abstract void InitEvent();
    public abstract Action GetEvent(GameEvent.Type eventType);

    protected T GetModel<T>() where T:Model {
        return m_data as T;
    }

    public void Dispose() {
        m_data.Dispose();
    }
}
