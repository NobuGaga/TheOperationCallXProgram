using UnityEngine;
using System;
using System.Collections.Generic;

public class CMonster:Controller {
    private Transform m_parent;
    private Dictionary<string, ModelMonster> m_dicNameMonster = new Dictionary<string, ModelMonster>();
    private List<ModelMonster> m_listMonster = new List<ModelMonster>();

    public CMonster(GameMoudle moudle, Type modelType):base(moudle, modelType) { }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.InitModel);
        m_eventList.Add(GameEvent.Type.Damage);
    }

    public override Action<object> GetEvent(GameEvent.Type eventType) {
        switch (eventType) {
            case GameEvent.Type.InitModel:
                return InitModel;
            case GameEvent.Type.Damage:
                return Damage;
            default:
                return null;
        }
    }
    
    public void InitModel(object arg) {
        Dictionary<string, GameObject> dicNodeName = arg as Dictionary<string, GameObject>;
        m_parent = dicNodeName["MonsterGroup"].transform;
        for (int i = 0; i < 2; i++)
            CreateMonster(i);
        EventManager.Register(GameEvent.Type.FrameUpdate, FrameUpdate);
        EventManager.Register(GameEvent.Type.LastUpdate, LastUpdate);
    }

    private void CreateMonster(int index, MonsterType type = MonsterType.Rubbish) {
        ModelMonsterData data = GetModel<MMonsterData>().GetMonsterData(type);
        GameObject monster = GameObject.Instantiate(data.Prefab);
        string nodeName = string.Concat(data.PrefabName, index);
        monster.name = nodeName;
        monster.transform.SetParent(m_parent);
        monster.transform.position = data.GetScenePostion();
        ModelMonster script = new ModelMonster(monster, data.AttackRoleData, data.Speed, data.headHeight, data.trackDis, type);
        if (m_dicNameMonster.ContainsKey(nodeName))
            m_dicNameMonster[nodeName] = script;
        else
            m_dicNameMonster.Add(nodeName, script);
        m_listMonster.Add(script);
        Vector3 screenPos = GameSceneManager.ToScreenPoint(monster.transform.position);
        script.HPPosY = ModelRoleManager.GetModelRoleHpPosY(data.PrefabName);
        screenPos.y += script.HPPosY;
        KeyValuePair<string, Vector3> hpProcessData = new KeyValuePair<string, Vector3>(monster.name, screenPos);
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.MonsterCreate, hpProcessData);
    }

    public void Damage(object arg) {
        ModelAttackData data = (ModelAttackData)arg;
        ModelMonster monster = GetMonster(data.receiver);
        if (monster == null || monster.gameObject == null)
            return;
        float hpPercent = monster.Damage(data);
        KeyValuePair<string, float> hpData = new KeyValuePair<string, float>(monster.gameObject.name, hpPercent);
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.MonsterDamage, hpData);
    }

    private ModelMonster GetMonster(string nodeName) {
        if (m_dicNameMonster.ContainsKey(nodeName))
            return m_dicNameMonster[nodeName];
        DebugTool.LogError(string.Format("CMonster node name {0} not exit", nodeName));
        return null;
    }

    private void FrameUpdate(object arg) {
        List<int> deleteIndexList = null;
        List<string> deleteNameList = null;
        for (int i = 0; i < m_listMonster.Count; i++) {
            if (m_listMonster[i].State != SRoleState.Type.SRoleDeath) {
                m_listMonster[i].Update();
                continue;
            }
            if (deleteIndexList == null)
                deleteIndexList = new List<int>();
            if (deleteNameList == null)
                deleteNameList = new List<string>();
            deleteIndexList.Add(i);
            deleteNameList.Add(m_listMonster[i].gameObject.name);
        }
        if (deleteIndexList == null)
            return;
        for (int i = 0; i < deleteIndexList.Count; i++) {
            int index = deleteIndexList[i];
            string name = deleteNameList[i];
            m_listMonster.RemoveAt(index - i);
            if (m_dicNameMonster.ContainsKey(name))
                m_dicNameMonster.Remove(name);
            CreateMonster(index);
        }
    }

    private void LastUpdate(object arg) {
        for (int i = 0; i < m_listMonster.Count; i++) {
            string nodeName = m_listMonster[i].gameObject.name;
            Vector3 position = m_listMonster[i].transform.position;
            position = GameSceneManager.ToScreenPoint(position);
            position.y += m_listMonster[i].HPPosY;
            KeyValuePair<string, Vector3> data = new KeyValuePair<string, Vector3>(nodeName, position);
            EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.MonsterMove, data);
        }
    }
}