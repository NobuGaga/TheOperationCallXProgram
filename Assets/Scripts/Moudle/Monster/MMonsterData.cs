using UnityEngine;
using System.Collections.Generic;

public class MMonsterData:Model {
    private Dictionary<MonsterType, ModelMonsterData> m_dicMonsterData;

    public override void Init() {
        m_dicMonsterData = new Dictionary<MonsterType, ModelMonsterData>();
        ModelMonsterData data = new ModelMonsterData(MonsterType.Rubbish, "Monster_Warrior_Prefab", 100, 30, 2, 5, 0.5f, 30);
        data.AddScenePosition(GameScene.DesertScene, new Vector3(-0.2f, -0.48f, -0.3f), new Vector3(0.2f, -0.48f, 0.3f));
        m_dicMonsterData.Add(data.Type, data);
    }

    public ModelMonsterData GetMonsterData(MonsterType type) {
        if (!m_dicMonsterData.ContainsKey(type))
            DebugTool.LogError(string.Format("MMonsterData::GetAttackRoleData not exit type \"{0}\" data", type.ToString()));
        return m_dicMonsterData[type];
    }

    public ModelAttackRoleData GetAttackRoleData(MonsterType type) {
        return GetMonsterData(type).AttackRoleData;
    }

    public override void Dispose() {

    }
}