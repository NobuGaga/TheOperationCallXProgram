using UnityEngine;

public class SRoleDamage:RoleState {
    private bool m_isUnattackable = false;

    public SRoleDamage(ModelRole role, Animation animation):base(role, animation) { }

    public override void Enter() {
        base.Enter();
        m_isUnattackable = true;
    }

    public override void Update() {
        if (!m_isUnattackable)
            return;
        if (IsPlayingAnimation)
            return;
        m_isUnattackable = false;
        m_role.State = Type.SRoleStand;
    }

    public override void Exit() { }

    public override Type GetState() { return Type.SRoleDamage; }
}