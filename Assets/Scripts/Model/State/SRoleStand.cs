using UnityEngine;

public class SRoleStand:SRoleState {
    public SRoleStand(ModelRole role, Animation animation):base(role, animation) { }

    public override void Enter() {
        PlayAnimation();
    }

    public override void Update() { }

    public override bool Exit() {
        return true;
    }

    public override Type GetState() { return Type.SRoleStand; }
}