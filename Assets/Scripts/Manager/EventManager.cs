using System;
using System.Collections.Generic;

public static class EventManager {
    private static Dictionary<GameMoudle, GameEvent> dicMoudleEvent = new Dictionary<GameMoudle, GameEvent>();
    private static Dictionary<GameEvent.Type, List<Action<object>>> dicSpecialEvent = new Dictionary<GameEvent.Type, List<Action<object>>>();

    public static void Dispatch(GameMoudle moudle, GameEvent.Type eventType, object arg = null) {
        if (!dicMoudleEvent.ContainsKey(moudle))
            Register(moudle);
        dicMoudleEvent[moudle].Trigger(eventType, arg);
        if (eventType == GameEvent.Type.CloseMainView)
            AssetBundleManager.Clean();
    }

    public static void Dispatch(GameEvent.Type eventType, object arg = null) {
        switch (eventType) {
            case GameEvent.Type.ChangeScene:
                GameSceneManager.ChangeScene((GameScene)arg);
                return;
        }
        if (!dicSpecialEvent.ContainsKey(eventType) || dicSpecialEvent[eventType].Count == 0)
            return;
        for (int i = 0;  i < dicSpecialEvent[eventType].Count; i++)
            dicSpecialEvent[eventType][i](arg);
    }

    private static void Register(GameMoudle moudle) {
        Controller controller = ControllerManager.GetController(moudle);
        GameEvent gameEvent = new GameEvent();
        foreach (GameEvent.Type type in controller.EventList)
            gameEvent.Add(type, controller.GetEvent(type));
        dicMoudleEvent.Add(moudle, gameEvent);
    }

    public static void Register(GameEvent.Type eventType, Action<object> action) {
        if (!dicSpecialEvent.ContainsKey(eventType))
            dicSpecialEvent.Add(eventType, new List<Action<object>>());
        if (dicSpecialEvent[eventType].Contains(action))
            return;
        dicSpecialEvent[eventType].Add(action);
    }

    public static void Unregister(GameEvent.Type eventType, Action<object> action) {
        if (!dicSpecialEvent.ContainsKey(eventType) || !(dicSpecialEvent[eventType].Contains(action)))
            return;
        dicSpecialEvent[eventType].Remove(action);
    }
}