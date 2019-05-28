using UnityEngine;

public class SRoleDeath:SRoleState {
    public SRoleDeath(ModelRole role, Animation animation):base(role, animation) { }

    public override void Enter() {
        PlayAnimation();
        attackRole?.Death();
    }

    public override void Update() { }

    public override bool Exit() {
        return false;
    }

    public override Type GetState() { return Type.SRoleDeath; }

    private ModelAttackRole attackRole {
        get {
            if (m_role is ModelAttackRole)
                return m_role as ModelAttackRole;
            return null;
        }
    }
}