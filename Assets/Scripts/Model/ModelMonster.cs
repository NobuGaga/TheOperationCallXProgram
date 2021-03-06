﻿using UnityEngine;

public class ModelMonster:ModelAttackRole {
    private float m_speed;
    private ModelMonsterVision m_vision;
    private GameObject m_selectObj;
    private MonsterType m_type;
    private int m_hpPosY;
    public int HPPosY {
        set {
            m_hpPosY = value;
        }
        get {
            return m_hpPosY;
        }
    }
    private static GameObject m_monsterVision;
    private static GameObject selectObj;

    static ModelMonster() {
        m_monsterVision = Resources.Load<GameObject>("MonsterVision");
        selectObj = Resources.Load<GameObject>("MonsterVision");
    }

    public ModelMonster(GameObject node, ModelMonsterData data, MonsterType type):base(node, data.AttackRoleData) {
        m_speed = data.Speed;
        m_type = type;
        GameObject monsterVision = GameObject.Instantiate(m_monsterVision);
        monsterVision.transform.SetParent(transform, false);
        ModelMonsterVision vision = monsterVision.GetComponent<ModelMonsterVision>();
        vision.SetVision(data.headHeight, data.trackDis);
        vision.SetCallBack(OnTriggerEnter, OnTriggerStay, OnTriggerExit);
        m_vision = vision;
        m_visionArea = data.trackDis;
        m_selectObj = GameObject.Instantiate(selectObj);
        m_selectObj.transform.SetParent(transform, false);
        Vector3 position = m_selectObj.transform.localPosition;
        m_selectObj.transform.localPosition = new Vector3(position.x, data.selectPoint, position.z);
        m_selectObj.SetActive(false);
    }

    protected override void InitAnimation() {
        AddAnimation(SRoleState.Type.SRoleStand.ToString(), "DrawBlade");
        AddAnimation(SRoleState.Type.SRoleRun.ToString(), "Run00");
        AddAnimation(SRoleState.Type.SRoleReadyFight.ToString(), "DrawBlade");
    }

    protected override void InitAttackSkillAnimation() { }

    private Transform m_target;
    private float m_disToTarget;
    private float m_visionArea;

    public override void Update() {
        base.Update();
        if (m_target == null)
            return;
        if (State == SRoleState.Type.SRoleDeath)
            return;
        switch (State) {
            case SRoleState.Type.SRoleStand:
                if (m_disToTarget < m_attackDis)
                    State = SRoleState.Type.SRoleAttack;
                else if (m_disToTarget < m_visionArea)
                    State = SRoleState.Type.SRoleRun;
                break;
            case SRoleState.Type.SRoleRun:
                if (m_disToTarget < m_attackDis)
                    State = SRoleState.Type.SRoleAttack;
                else {
                    m_transform.LookAt(m_target, Vector3.up);
                    m_rotationY = m_transform.rotation.eulerAngles.y;
                    m_disToTarget = (m_target.position - m_transform.position).magnitude;
                    Vector3 speed = (m_target.position - m_transform.position).normalized * m_speed;
                    m_velocity = speed * Time.deltaTime;
                }
                break;
            case SRoleState.Type.SRoleAttack:
                m_disToTarget = (m_target.position - m_transform.position).magnitude;
                if (m_disToTarget > m_attackDis)
                    State = SRoleState.Type.SRoleRun;
                break;
        }
    }

    public override void Attack() {
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
                                                            (int)m_type, m_attackLevel, gameObject.name, colliders[i].name);
            EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.Damage, attackData);
            return;
        }
    }

    public override float Damage(ModelAttackData data) {
        float percent = base.Damage(data);
        DebugTool.Log("ModelMonster::Damage " + m_healthPoint.ToString());
        if (m_target == null)
            m_target = GameSceneManager.GetNode<Transform>(data.sender);
        m_selectObj.SetActive(true);
        TimerManager.Register(1, () => m_selectObj.SetActive(false));
        return percent;
    }

    public override void Death() {
        m_selectObj.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
        m_vision.gameObject.SetActive(false);
        float time = 1;
        TimerManager.Register(time, () => GameObject.Destroy(gameObject));
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag != GameTagInfo.Player.ToString() || State == SRoleState.Type.SRoleDeath)
            return;
        m_target = collider.transform;
        m_transform.LookAt(m_target, Vector3.up);
        m_disToTarget = (m_target.position - m_transform.position).magnitude;
        State = SRoleState.Type.SRoleRun;
    }

    private void OnTriggerStay(Collider collider) {
        if (collider.tag != GameTagInfo.Player.ToString() || m_target == null)
            return;
        m_transform.LookAt(m_target, Vector3.up);
        m_disToTarget = (m_target.position - m_transform.position).magnitude;
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.tag != GameTagInfo.Player.ToString())
            return;
        m_target = null;
        State = SRoleState.Type.SRoleStand;
    }
}