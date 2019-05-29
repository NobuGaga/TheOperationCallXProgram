public struct ModelAttackData {
    private RoleType roleType;
    private int roleLevel;
    private ModelAttackLevel attackLevel;
    public string sender;
    public string receiver;

    /// <summary>
    /// </summary>
    /// <param name="roleType">攻击发出者角色类型</param>
    /// <param name="roleLevel">攻击发出者角色类型等级, 对应类型枚举</param>
    /// <param name="attackLevel">攻击发出者攻击等级</param>
    /// <param name="sender">攻击发出者</param>
    /// <param name="receiver">攻击接收者</param>
    public ModelAttackData(RoleType roleType, int roleLevel, ModelAttackLevel attackLevel, string sender, string receiver) {
        this.roleType = roleType;
        this.roleLevel = roleLevel;
        this.attackLevel = attackLevel;
        this.sender = sender;
        this.receiver = receiver;
    }

    public override string ToString() {
        return string.Format("\n{{\n\troleType:{0},\n\troleLevel:{1},\n\tattackLevel:{2},\n\tsender:{3},\n\treceiver:{4}\n}}",
                            roleType.ToString(), roleLevel, attackLevel.ToString(), sender, receiver);
    }

    public int Damage {
        get {
            return roleLevel * 10 + (int)attackLevel * 10;
        }
    }
}

public enum ModelAttackLevel {
    Normal = 1,
    SkillOne,
    SkillTwo,
    SkillThree,
}