using System.Collections.Generic;

public static class EventManager {
    private static Dictionary<GameMoudle, GameEvent> m_dicMoudleEvent = new Dictionary<GameMoudle, GameEvent>();

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
