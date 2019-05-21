﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class CMonster:Controller {
    private Transform m_parent;
    private GameObject m_monsterPrefab;
    private Dictionary<string, ModelMonster> m_dicNameMonster = new Dictionary<string, ModelMonster>();

    public CMonster(GameMoudle moudle, Type modelType):base(moudle, modelType) {
        m_monsterPrefab = Resources.Load<GameObject>("Monster_Warrior_Prefab");
    }

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
        for (int i = 0; i < 2; i++) {
            GameObject monster = GameObject.Instantiate(m_monsterPrefab);
            string nodeName = string.Concat(m_monsterPrefab.name, i);
            monster.name = nodeName;
            monster.transform.SetParent(m_parent);
            float positionX = UnityEngine.Random.Range(-0.2f, 0.2f);
            float positionZ = UnityEngine.Random.Range(-0.3f, 0.3f);
            float positionY = monster.transform.position.y;
            monster.transform.position = new Vector3(positionX, positionY, positionZ);
            ModelMonster script = monster.AddComponent<ModelMonster>();
            if (m_dicNameMonster.ContainsKey(nodeName))
                m_dicNameMonster[nodeName] = script;
            else
                m_dicNameMonster.Add(nodeName, script);
            script.Init(GetModel<MMonsterData>().GetHealthPoint(m_monsterPrefab.name));
        }
    }

    public void Damage(object arg) {
        ModelAttackData data = (ModelAttackData)arg;
        ModelMonster monster = GetMonster(data.receiver);
        monster.Damage(data);
    }

    private ModelMonster GetMonster(string nodeName) {
        if (m_dicNameMonster.ContainsKey(nodeName))
            return m_dicNameMonster[nodeName];
        DebugTool.LogError(string.Format("CMonster node name {0} not exit" + nodeName));
        return null;
    }
}