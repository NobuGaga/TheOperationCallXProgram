﻿using UnityEngine;
using System.Collections.Generic;

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
    private Dictionary<ModelAttackLevel, string> m_AttackAnimation;
    protected void AddAttackAnimation(ModelAttackLevel level, string animationName) {
        if (m_AttackAnimation == null)
            m_AttackAnimation = new Dictionary<ModelAttackLevel, string>();
        if (m_AttackAnimation.ContainsKey(level))
            m_AttackAnimation[level] = animationName;
        else
            m_AttackAnimation.Add(level, animationName);
    }

    private string GetAttackAnimationName() {
        if (m_AttackAnimation == null)
            return string.Empty;
        if (m_AttackAnimation.ContainsKey(m_attackLevel))
            return m_AttackAnimation[m_attackLevel];
        return string.Empty;
    }

    public override string GetAnimationName(string state) {
        string animationName = string.Empty;
        if (State == SRoleState.Type.SRoleAttack)
            animationName = GetAttackAnimationName();
        if (animationName == string.Empty)
            animationName = base.GetAnimationName(state);
        return animationName;
    }

    public ModelAttackRole(GameObject node, ModelAttackRoleData data):base(node) {
        InitAttackAnimation();
        m_healthPoint = new ModelHPData(data.MaxHP);
        m_attackDis = data.attackDis;
        m_attackAngle = data.attackAngle;
    }

    protected abstract void InitAttackAnimation();

    public abstract void Attack();
    public virtual float Damage(ModelAttackData data) {
        int damage = data.Damage;
        KeyValuePair<Transform, string> message = new KeyValuePair<Transform, string>(transform, damage.ToString());
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.ShowDamageText, message);
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