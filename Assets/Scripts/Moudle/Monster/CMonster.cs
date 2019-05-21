using System;

public class CMonster:Controller {
    private  ModelMonster m_player;

    public CMonster(GameMoudle moudle, Type modelType):base(moudle, modelType) { }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.Damage);
    }

    public override Action<object> GetEvent(GameEvent.Type eventType) {
        switch (eventType) {
            case GameEvent.Type.Damage:
                return Damage;
            default:
                return null;
        }
    }
    
    public void Damage(object arg) {
        DebugTool.Log((string)arg);
    }
}