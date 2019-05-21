using UnityEngine;

public class SRoleAttack:RoleState {
    private bool m_isAttacking = false;

    public SRoleAttack(ModelRole role, Animation animation):base(role, animation) { }

    public override void Enter() {
        PlayAnimation();
        m_isAttacking = true;
        m_role.Attack(ModelAttackLevel.Normal);
    }

    public override void Update() {
        if (!m_isAttacking)
            return;
        if (IsPlayingAnimation)
            return;
        m_isAttacking = false;
        m_role.State = Type.SRoleStand;
    }

    public override void Exit() {
        
    }

    public override Type GetState() { return Type.SRoleAttack; }
}