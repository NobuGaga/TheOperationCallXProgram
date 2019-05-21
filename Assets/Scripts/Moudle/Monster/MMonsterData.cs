using System.Collections.Generic;

public class MMonsterData:Model {
    private Dictionary<string, int> m_dicMonsterHP;

    public override void Init() {
        m_dicMonsterHP = new Dictionary<string, int>();
        m_dicMonsterHP.Add("Monster_Warrior_Prefab", 100);
    }

    public int GetHealthPoint(string name) {
        if (m_dicMonsterHP.ContainsKey(name))
            return m_dicMonsterHP[name];
        return 0;
    }

    public override void Dispose() {

    }
}