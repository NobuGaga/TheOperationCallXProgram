using UnityEngine;

public abstract class ModelAttackRole:ModelRunRole {
    protected ModelHPData m_healthPoint;
    public bool IsHPZero {
        get {
            return m_healthPoint.IsZero;
        }
    }

    private GameObject m_damageText;

    public ModelAttackRole(GameObject node, int healthPoint):base(node) {
        m_healthPoint = new ModelHPData(healthPoint);
        m_damageText = Resources.Load<GameObject>("ModelDamageText");
    }

    protected float m_attackDis = 0.5f;
    protected int m_attackAngle = 30;

    public abstract void Attack(ModelAttackLevel level);
    public virtual float Damage(ModelAttackData data) {
        int damage = data.Damage;
        GameObject damageObj = GameObject.Instantiate<GameObject>(m_damageText);
        damageObj.transform.SetParent(transform, false);
        ModelDamageText text = damageObj.GetComponent<ModelDamageText>();
        text.SetText(damage.ToString());
        m_healthPoint -= damage;
        SRoleState.Type state;
        if (IsHPZero)
            state = SRoleState.Type.SRoleDeath;
        else
            state = SRoleState.Type.SRoleDamage;
        State = state;
        return m_healthPoint.Percent;
    }
}