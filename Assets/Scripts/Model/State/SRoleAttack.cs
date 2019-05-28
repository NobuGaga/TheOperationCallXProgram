using UnityEngine;

public class SRoleAttack:SRoleState {
    private bool m_isAttacking = false;

    public SRoleAttack(ModelRole role, Animation animation):base(role, animation) { }

    public override void Enter() {
        PlayAnimation();
        attackRole?.PlaySkillAnimator();
        m_isAttacking = true;
        float time = attackRole.AttackDelayTime;
        if (time <= 0)
            attackRole?.Attack();
        else
            TimerManager.Register(time, attackRole.Attack);
    }

    public override void Update() {
        if (!m_isAttacking)
            return;
        if (IsPlayingAnimation)
            return;
        m_isAttacking = false;
        m_role.State = Type.SRoleStand;
    }

    public override bool Exit() {
        return !m_isAttacking;
    }

    public override Type GetState() { return Type.SRoleAttack; }

    private ModelAttackRole attackRole {
        get {
            if (m_role is ModelAttackRole)
                return m_role as ModelAttackRole;
            return null;
        }
    }
}