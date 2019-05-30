using System.Collections.Generic;

public class GameEvent {
    public enum Type {
        // GameManager Event
        ChangeScene,

        // Moudle Whole Event(Every Moudle Only One Event)
        FrameUpdate,
        PhysicsUpdate,
        LastUpdate,

        // Moudle Sub Event
        OpenMainView,
        CloseMainView,
        Click,

        // Virtual Button Event
        SteeringWheelDragBegin,
        SteeringWheelDraging,
        SteeringWheelDragEnd,

        // Model Event
        InitModel,
        Attack,
        Damage,
        ShowDamageText,
        MonsterCreate,
        MonsterMove,
        MonsterDamage,
    }

    private Dictionary<Type, System.Action<object>> m_dicEventTrigger = new Dictionary<Type, System.Action<object>>();

    public void Add(Type eventType, System.Action<object> callback) {
        if (m_dicEventTrigger.ContainsKey(eventType))
            return;
        m_dicEventTrigger.Add(eventType, callback);
    }
    public void Remove(Type eventType) {
        if (!m_dicEventTrigger.ContainsKey(eventType))
            return;
        m_dicEventTrigger.Remove(eventType);
    }
    public void Trigger(Type eventType, object arg) {
        if (!m_dicEventTrigger.ContainsKey(eventType)) {
            DebugTool.LogError(string.Format("Event Type {0} is not exit", eventType.ToString()));
            return;
        }
        m_dicEventTrigger[eventType](arg);
    }

    ~GameEvent() {
        m_dicEventTrigger.Clear();
    }
}