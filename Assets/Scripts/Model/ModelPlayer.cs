using UnityEngine;

public class ModelPlayer:ModelRole {
    private float m_curFoward;
    private float m_attackDis = 0.5f;
    private int m_attackAngle = 30;

    protected override void InitAnimation() {
        AddAnimation(RoleState.Type.SRoleStand.ToString(), "DrawBlade");
        AddAnimation(RoleState.Type.SRoleRun.ToString(), "Run00");
        AddAnimation(RoleState.Type.SRoleReadyFight.ToString(), "DrawBlade");
    }

    protected override void Start() {
        base.Start();
        m_curFoward = transform.rotation.eulerAngles.y;
    }

    public void SetVelocityAndRotation(Vector2 direction2D) {
        direction2D = direction2D.normalized;
        m_velocity.x = direction2D.x;
        m_velocity.z = direction2D.y;
        switch(GameConfig.CameraType) {
            case GameCameraType.Fix:
                m_rotationY = Quaternion.LookRotation(m_velocity).eulerAngles.y;
                break;
            case GameCameraType.ThirdPerson:
                Quaternion playerRotation = Quaternion.LookRotation(m_velocity, Vector3.up);
                m_rotationY = playerRotation.eulerAngles.y;
                m_rotationY += m_curFoward;
                if (m_rotationY > GameConst.RoundAngle || m_rotationY < -GameConst.RoundAngle)
                    m_rotationY %= GameConst.RoundAngle;
                m_velocity = Quaternion.Euler(0, m_curFoward, 0) * m_velocity;
                break;
        }
        State = RoleState.Type.SRoleRun;
    }

    public override void EndRun() {
        base.EndRun();
        m_curFoward = transform.rotation.eulerAngles.y;
    }

    public override void Attack() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_attackDis, LayerMask.GetMask(GameLayerInfo.Player.ToString()));
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].isTrigger)
                continue;
            if (colliders[i].tag != GameTagInfo.Enemy.ToString())
                continue;
            Vector3 selfToTarget = colliders[i].transform.position - transform.position;
            float selfToTargetAngle = Vector3.Angle(selfToTarget, transform.forward);
            if (selfToTargetAngle > m_attackAngle)
                continue;
            EventManager.Dispatch(GameMoudle.Monster, GameEvent.Type.Damage, colliders[i].name);
        }
    }
}