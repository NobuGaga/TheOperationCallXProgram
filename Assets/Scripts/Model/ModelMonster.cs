using UnityEngine;

public class ModelMonster:ModelRole {
    public Transform m_target;

    private Vector3 speed = Vector3.forward;

    protected override void InitAnimation() {
        AddAnimation(RoleState.Type.SRoleStand.ToString(), "DrawBlade");
        AddAnimation(RoleState.Type.SRoleRun.ToString(), "Run00");
        AddAnimation(RoleState.Type.SRoleReadyFight.ToString(), "DrawBlade");
    }

    protected override void Start() {
        base.Start();
        if (m_target != null) {
            transform.LookAt(m_target, Vector3.up);
            speed = m_target.position - transform.position;
            m_rotationY = transform.rotation.eulerAngles.y;
            State = RoleState.Type.SRoleRun;
        }
    }

    private void Update() {
        m_velocity = speed * Time.deltaTime * GameConfig.CameraMoveFixTime;
        UpdateState();
    }

    // 开始接触
    void OnTriggerEnter(Collider collider) {
        //Debug.Log("开始接触");
    }

    // 接触结束
    void OnTriggerExit(Collider collider) {
        //Debug.Log("接触结束");
    }

    // 接触持续中
    void OnTriggerStay(Collider collider) {
        //Debug.Log("接触持续中");
    }
}