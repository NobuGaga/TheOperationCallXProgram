using UnityEngine;

public class SRoleRun : SRoleState {
    public SRoleRun(ModelRole role, Animation animation) : base(role, animation) { }

    public override void Enter() {
        PlayAnimation();
    }

    public override void Update() {
        PlayAnimation();
        runRole?.Run();
    }

    public override bool Exit() {
        runRole?.EndRun();
        return true;
    }

    public override Type GetState() { return Type.SRoleRun; }

    private ModelRunRole runRole {
        get {
            if (m_role is ModelRunRole)
                return m_role as ModelRunRole;
            return null;
        }
    }
}