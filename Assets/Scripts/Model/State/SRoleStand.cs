using UnityEngine;

public class SRoleStand:RoleState {
    public SRoleStand(ModelRole role, Animation animation):base(role, animation) { }

    public override void Enter() {
        PlayAnimation();
    }

    public override void Update() { }

    public override void Exit() { }

    public override Type GetState() { return Type.SRoleStand; }
}