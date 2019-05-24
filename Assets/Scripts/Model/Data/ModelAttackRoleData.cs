public struct ModelAttackRoleData {
    private int maxHP;
    public int MaxHP => maxHP;
    public float attackDis;
    public int attackAngle;

    /// <summary>
    /// </summary>
    /// <param name="maxHP">最大血量</param>
    /// <param name="attackDis">攻击距离</param>
    /// <param name="attackAngle">攻击角度</param>
    public ModelAttackRoleData(int maxHP, float attackDis, int attackAngle) {
        this.maxHP = maxHP;
        this.attackDis = attackDis;
        this.attackAngle = attackAngle;
    }
}