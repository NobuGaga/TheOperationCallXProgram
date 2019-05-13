using System.Collections.Generic;

public class GameEvent {
    public enum Type {
        // GameManager Event
        ChangeScene,

        // Moudle Whole Event(Every Moudle Only One Event)
        FrameUpdate,

        // Moudle Sub Event
        OpenMainView,
    }

    private Dictionary<Type, System.Action> m_dicEventTrigger = new Dictionary<Type, System.Action>();

    public void Add(Type eventType, System.Action callback) {
        if (m_dicEventTrigger.ContainsKey(eventType))
            return;
        m_dicEventTrigger.Add(eventType, callback);
    }
    public void Remove(Type eventType) {
        if (!m_dicEventTrigger.ContainsKey(eventType))
            return;
        m_dicEventTrigger.Remove(eventType);
    }
    public void Trigger(Type eventType) {
        if (!m_dicEventTrigger.ContainsKey(eventType)) {
            DebugTool.LogError(string.Format("Event Type {0} is not exit", eventType.ToString()));
            return;
        }
        m_dicEventTrigger[eventType]();
    }
}