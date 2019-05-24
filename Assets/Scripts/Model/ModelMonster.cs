using UnityEngine;

public class ModelMonster:ModelAttackRole {
    public ModelMonster(GameObject node, int healthPoint, ModelMonsterVision vision) :base(node, healthPoint) {
        vision.SetCallBack(OnTriggerEnter, OnTriggerStay, OnTriggerExit);
    }

    protected override void InitAnimation() {
        AddAnimation(SRoleState.Type.SRoleStand.ToString(), "DrawBlade");
        AddAnimation(SRoleState.Type.SRoleRun.ToString(), "Run00");
        AddAnimation(SRoleState.Type.SRoleReadyFight.ToString(), "DrawBlade");
    }

    private Transform m_target;
    private float m_disToTarget;
    private Vector3 m_speed;

    public override void Update() {
        base.Update();
        if (m_target == null)
            return;
        switch (State) {
            case SRoleState.Type.SRoleStand:
                if (m_disToTarget < m_attackDis)
                    State = SRoleState.Type.SRoleAttack;
                break;
            case SRoleState.Type.SRoleRun:
                if (m_disToTarget < m_attackDis)
                    State = SRoleState.Type.SRoleAttack;
                else {
                    m_transform.LookAt(m_target, Vector3.up);
                    m_rotationY = m_transform.rotation.eulerAngles.y;
                    m_disToTarget = (m_target.position - m_transform.position).magnitude;
                    m_speed = (m_target.position - m_transform.position).normalized * GameConfig.CameraMoveThirdModeTime * 3;
                    m_velocity = m_speed * Time.deltaTime;
                }
                break;
            case SRoleState.Type.SRoleAttack:
                m_disToTarget = (m_target.position - m_transform.position).magnitude;
                if (m_disToTarget > m_attackDis)
                    State = SRoleState.Type.SRoleRun;
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
            ModelAttackData attackData = new ModelAttackData(RoleType.Monster, 
                                                            (int)MonsterType.Rubbish, level, gameObject.name, colliders[i].name);
            EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.Damage, attackData);
            return;
        }
    }

    public override float Damage(ModelAttackData data) {
        m_healthPoint -= data.Damage;
        DebugTool.Log("ModelMonster::Damage " + m_healthPoint.ToString());
        SRoleState.Type state;
        if (IsHPZero)
            state = SRoleState.Type.SRoleDeath;
        else
            state = SRoleState.Type.SRoleDamage;
        State = state;
        return m_healthPoint.Percent;
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag != GameTagInfo.Player.ToString())
            return;
        m_target = collider.transform;
        m_transform.LookAt(m_target, Vector3.up);
        m_disToTarget = (m_target.position - m_transform.position).magnitude;
        State = SRoleState.Type.SRoleRun;
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