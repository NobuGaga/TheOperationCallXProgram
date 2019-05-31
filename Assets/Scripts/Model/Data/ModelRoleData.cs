public struct ModelRoleData {
    public RoleType roleType;
    public int roleLevel;
    public string prefabName;
    public int hpProcessPosY;

    public ModelRoleData(RoleType roleType, int roleLevel, string prefabName, int hpProcessPosY) {
        this.roleType = roleType;
        this.roleLevel = roleLevel;
        this.prefabName = prefabName;
        this.hpProcessPosY = hpProcessPosY;
    }

    public bool IsMonster {
        get {
            return roleType.ToString() == RoleType.Monster.ToString();
        }
    }

    public MonsterType GetMonsterType() {
        if (IsMonster)
            return (MonsterType)roleLevel;
        return MonsterType.Rubbish;
    }

    public PlayerType GetPlayerType() {
        if (!IsMonster)
            return (PlayerType)roleLevel;
        return PlayerType.Rubbish;
    }
}