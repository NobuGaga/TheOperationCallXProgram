using UnityEngine;

public class SRoleRun:RoleState {
    public SRoleRun(ModelRole role, Animation animation):base(role, animation) { }

    public override void Enter() {
        PlayAnimation();
    }

    public override void Update() {
        PlayAnimation();
        m_role.Run();
    }

    public override void Exit() {
        m_role.EndRun();
    }

    public override Type GetState() { return Type.SRoleRun; }
}