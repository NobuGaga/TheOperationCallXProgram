using System;
using System.Collections.Generic;

public static class EventManager {
    private static Dictionary<GameMoudle, GameEvent> m_dicMoudleEvent = new Dictionary<GameMoudle, GameEvent>();
    private static Dictionary<GameEvent.Type, List<Action<object>>> m_dicSpecialEvent = new Dictionary<GameEvent.Type, List<Action<object>>>();

    public static void Dispatch(GameEvent.Type eventType, object arg) {
        if (!m_dicSpecialEvent.ContainsKey(eventType) || m_dicSpecialEvent[eventType].Count == 0)
            return;
        Action<object>[] listAction = m_dicSpecialEvent[eventType].ToArray();
        foreach (Action<object> action in listAction)
            action(arg);
    }

    public static void Register(GameEvent.Type eventType, Action<object> action) {
        if (!m_dicSpecialEvent.ContainsKey(eventType))
            m_dicSpecialEvent.Add(eventType, new List<Action<object>>());
        if (m_dicSpecialEvent[eventType].Contains(action))
            return;
        m_dicSpecialEvent[eventType].Add(action);
    }

    public static void Unregister(GameEvent.Type eventType, Action<object> action) {
        if (!m_dicSpecialEvent.ContainsKey(eventType) || !(m_dicSpecialEvent[eventType].Contains(action)))
            return;
        m_dicSpecialEvent[eventType].Remove(action);
    }

    public static void Dispatch(GameMoudle moudle, GameEvent.Type eventType) {
        if (!m_dicMoudleEvent.ContainsKey(moudle))
            Register(moudle);
        m_dicMoudleEvent[moudle].Trigger(eventType);
    }

    private static void Register(GameMoudle moudle) {
        Controller controller = ControllerManager.GetController(moudle);
        GameEvent gameEvent = new GameEvent();
        foreach (GameEvent.Type type in controller.EventList)
            gameEvent.Add(type, controller.GetEvent(type));
        m_dicMoudleEvent.Add(moudle, gameEvent);
    }
}
