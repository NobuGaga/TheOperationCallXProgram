using UnityEngine;

public class ModelMonster:ModelRole {
    private Transform m_target;
    private float m_disToTarget;
    private Vector3 m_speed;
    private float m_attackDis = 0.5f;
    private int m_attackAngle = 30;

    protected override void InitAnimation() {
        AddAnimation(RoleState.Type.SRoleStand.ToString(), "DrawBlade");
        AddAnimation(RoleState.Type.SRoleRun.ToString(), "Run00");
        AddAnimation(RoleState.Type.SRoleReadyFight.ToString(), "DrawBlade");
    }

    private void Update() {
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
                    transform.LookAt(m_target, Vector3.up);
                    m_rotationY = transform.rotation.eulerAngles.y;
                    m_disToTarget = (m_target.position - transform.position).magnitude;
                    m_speed = (m_target.position - transform.position).normalized * GameConfig.CameraMoveFixTime * 3;
                    m_velocity = m_speed * Time.deltaTime;
                }
                break;
            case RoleState.Type.SRoleAttack:
                m_disToTarget = (m_target.position - transform.position).magnitude;
                if (m_disToTarget > m_attackDis)
                    State = RoleState.Type.SRoleRun;
                break;
        }
        UpdateState();
    }

    public override void Attack() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_attackDis, LayerMask.GetMask(GameLayerInfo.Player.ToString()));
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].tag != GameTagInfo.Player.ToString())
                continue;
            Vector3 selfToTarget = colliders[i].transform.position - transform.position;
            if (selfToTarget.magnitude > m_attackDis)
                continue;
            float selfToTargetAngle = Vector3.Angle(selfToTarget, transform.forward);
            if (selfToTargetAngle > m_attackAngle)
                continue;
            EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.Damage);
            return;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag != GameTagInfo.Player.ToString())
            return;
        m_target = collider.transform;
        transform.LookAt(m_target, Vector3.up);
        m_disToTarget = (m_target.position - transform.position).magnitude;
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