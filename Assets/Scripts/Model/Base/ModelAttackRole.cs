using UnityEngine;

public abstract class ModelAttackRole:ModelRunRole {
    protected ModelHPData m_healthPoint;
    public bool IsHPZero {
        get {
            return m_healthPoint.IsZero;
        }
    }

    protected float m_attackDis;
    protected int m_attackAngle;
    private GameObject m_damageText;
    protected ModelAttackLevel m_attackLevel = ModelAttackLevel.Normal;
    public ModelAttackLevel AttackLevel {
        set {
            m_attackLevel = value;
        }
    }

    public ModelAttackRole(GameObject node, ModelAttackRoleData data):base(node) {
        m_healthPoint = new ModelHPData(data.MaxHP);
        m_attackDis = data.attackDis;
        m_attackAngle = data.attackAngle;
        m_damageText = Resources.Load<GameObject>("ModelDamageText");
    }

    public abstract void Attack();
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
    public abstract void Death();
}