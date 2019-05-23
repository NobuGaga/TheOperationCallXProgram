using UnityEngine;

public class SRoleDeath:SRoleState {
    public SRoleDeath(ModelRole role, Animation animation):base(role, animation) { }

    public override void Enter() {
        PlayAnimation();
    }

    public override void Update() { }

    public override void Exit() { }

    public override Type GetState() { return Type.SRoleDeath; }
}