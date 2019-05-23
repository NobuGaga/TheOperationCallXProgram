using UnityEngine;

public class ModelPlayer:ModelRole {
    private float m_curFoward;
    public float BeforeMoveFoward => m_curFoward;

    private ModelHPData m_healthPoint;
    public override bool IsHPZero {
        get {
            return m_healthPoint.IsZero;
        }
    }

    public ModelPlayer(GameObject node, int healthPoint):base(node) {
        m_curFoward = m_transform.rotation.eulerAngles.y;
        m_healthPoint = new ModelHPData(healthPoint);
    }

    protected override void InitAnimation() {
        AddAnimation(RoleState.Type.SRoleStand.ToString(), "DrawBlade");
        AddAnimation(RoleState.Type.SRoleRun.ToString(), "Run00");
        AddAnimation(RoleState.Type.SRoleReadyFight.ToString(), "DrawBlade");
    }

    public void SetVelocityAndRotation(Vector2 direction2D, float cameraRotationY) {
        direction2D = direction2D.normalized;
        m_velocity.x = direction2D.x;
        m_velocity.z = direction2D.y;
        switch(GameConfig.CameraType) {
            case GameCameraType.Fix:
                m_velocity = Quaternion.Euler(0, cameraRotationY, 0) * m_velocity * GameConfig.PlayerMoveFix;
                m_rotationY = Quaternion.LookRotation(m_velocity).eulerAngles.y;
                break;
            case GameCameraType.ThirdPerson:
                Quaternion playerRotation = Quaternion.LookRotation(m_velocity, Vector3.up);
                m_rotationY = playerRotation.eulerAngles.y;
                m_rotationY += m_curFoward;
                if (m_rotationY > GameConst.RoundAngle || m_rotationY < -GameConst.RoundAngle)
                    m_rotationY %= GameConst.RoundAngle;
                m_velocity = Quaternion.Euler(0, m_curFoward, 0) * m_velocity * GameConfig.PlayerMoveFix;
                break;
        }
        State = RoleState.Type.SRoleRun;
    }

    public override void EndRun() {
        base.EndRun();
        m_curFoward = m_transform.rotation.eulerAngles.y;
    }

    private float m_attackDis = 0.5f;
    private int m_attackAngle = 30;

    public override void Attack(ModelAttackLevel level) {
        Collider[] colliders = Physics.OverlapSphere(m_transform.position, m_attackDis, LayerMask.GetMask(GameLayerInfo.Enemy.ToString()));
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].isTrigger)
                continue;
            if (colliders[i].tag != GameTagInfo.Enemy.ToString())
                continue;
            Vector3 selfToTarget = colliders[i].transform.position - m_transform.position;
            float selfToTargetAngle = Vector3.Angle(selfToTarget, m_transform.forward);
            if (selfToTargetAngle > m_attackAngle)
                continue;
            ModelAttackData attackData = new ModelAttackData(RoleType.PlayerType,
                                                            (int)PlayerType.Master, level, m_gameObject.name, colliders[i].name);
            EventManager.Dispatch(GameMoudle.Monster, GameEvent.Type.Damage, attackData);
        }
    }

    public float Damage(ModelAttackData data) {
        m_healthPoint -= data.Damage;
        DebugTool.Log("ModelPlayer::Damage " + m_healthPoint.ToString());
        RoleState.Type state;
        if (IsHPZero)
            state = RoleState.Type.SRoleDeath;
        else
            state = RoleState.Type.SRoleDamage;
        State = state;
        return m_healthPoint.Percent;
    }
}