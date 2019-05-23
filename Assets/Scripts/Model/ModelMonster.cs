using UnityEngine;

public class ModelMonster:ModelRole {
    private ModelHPData m_healthPoint;
    public override bool IsHPZero {
        get {
            return m_healthPoint.IsZero;
        }
    }

    public ModelMonster(GameObject node, int healthPoint, ModelMonsterVision vision) :base(node) {
        m_healthPoint = new ModelHPData(healthPoint);
        vision.SetCallBack(OnTriggerEnter, OnTriggerStay, OnTriggerExit);
    }

    protected override void InitAnimation() {
        AddAnimation(RoleState.Type.SRoleStand.ToString(), "DrawBlade");
        AddAnimation(RoleState.Type.SRoleRun.ToString(), "Run00");
        AddAnimation(RoleState.Type.SRoleReadyFight.ToString(), "DrawBlade");
    }

    private Transform m_target;
    private float m_disToTarget;
    private Vector3 m_speed;
    private float m_attackDis = 0.5f;
    private int m_attackAngle = 30;

    public override void Update() {
        base.Update();
        if (m_target == null)
            return;
        switch (State) {
            case RoleState.Type.SRoleStand:
                if (m_disToTarget < m_attackDis)
                    State = RoleState.Type.SRoleAttack;
                break;
            case RoleState.Type.SRoleRun:
                if (m_disToTarget < m_attackDis)
                    State = RoleState.Type.SRoleAttack;
                else {
                    m_transform.LookAt(m_target, Vector3.up);
                    m_rotationY = m_transform.rotation.eulerAngles.y;
                    m_disToTarget = (m_target.position - m_transform.position).magnitude;
                    m_speed = (m_target.position - m_transform.position).normalized * GameConfig.CameraMoveThirdModeTime * 3;
                    m_velocity = m_speed * Time.deltaTime;
                }
                break;
            case RoleState.Type.SRoleAttack:
                m_disToTarget = (m_target.position - m_transform.position).magnitude;
                if (m_disToTarget > m_attackDis)
                    State = RoleState.Type.SRoleRun;
                break;
        }
    }

    public override void Attack(ModelAttackLevel level) {
        Collider[] colliders = Physics.OverlapSphere(m_transform.position, m_attackDis, LayerMask.GetMask(GameLayerInfo.Player.ToString()));
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].tag != GameTagInfo.Player.ToString())
                continue;
            Vector3 selfToTarget = colliders[i].transform.position - m_transform.position;
            if (selfToTarget.magnitude > m_attackDis)
                continue;
            float selfToTargetAngle = Vector3.Angle(selfToTarget, m_transform.forward);
            if (selfToTargetAngle > m_attackAngle)
                continue;
            ModelAttackData attackData = new ModelAttackData(RoleType.MonsterType, 
                                                            (int)MonsterType.Rubbish, level, gameObject.name, colliders[i].name);
            EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.Damage, attackData);
            return;
        }
    }

    public float Damage(ModelAttackData data) {
        m_healthPoint -= data.Damage;
        DebugTool.Log("ModelMonster::Damage " + m_healthPoint.ToString());
        RoleState.Type state;
        if (IsHPZero)
            state = RoleState.Type.SRoleDeath;
        else
            state = RoleState.Type.SRoleDamage;
        State = state;
        return m_healthPoint.Percent;
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag != GameTagInfo.Player.ToString())
            return;
        m_target = collider.transform;
        m_transform.LookAt(m_target, Vector3.up);
        m_disToTarget = (m_target.position - m_transform.position).magnitude;
        State = RoleState.Type.SRoleRun;
    }

    private void OnTriggerStay(Collider collider) {
        if (collider.tag != GameTagInfo.Player.ToString())
            return;
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.tag != GameTagInfo.Player.ToString())
            return;
    }
}