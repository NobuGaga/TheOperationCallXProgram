using UnityEngine;

public abstract class ModelAttackRole:ModelRunRole {
    protected ModelHPData m_healthPoint;
    public bool IsHPZero {
        get {
            return m_healthPoint.IsZero;
        }
    }

    public ModelAttackRole(GameObject node, int healthPoint):base(node) {
        m_healthPoint = new ModelHPData(healthPoint);
    }

    protected float m_attackDis = 0.5f;
    protected int m_attackAngle = 30;

    public abstract void Attack(ModelAttackLevel level);
    public abstract float Damage(ModelAttackData data);
}