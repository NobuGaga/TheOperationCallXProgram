using UnityEngine;
using System.Collections.Generic;

public class ModelPlayer:ModelWeaponRole {
    private float m_curFoward;
    public float BeforeMoveFoward => m_curFoward;
    private ModelWeapon m_weapon;
    private GameObject m_bulletObj;

    public ModelPlayer(GameObject node, ModelAttackRoleData attackData):base(node, attackData) {
        m_curFoward = m_transform.rotation.eulerAngles.y;
        SetHands("Glove", "Glove");
        UIPrefab prefab = gameObject.GetComponent<UIPrefab>();
        Transform weaponNode = prefab.GetNode<Transform>("headusOBJexport009");
        m_weapon = weaponNode.GetComponent<ModelWeapon>();
        SetWeapon(m_weapon);
        m_bulletObj = Resources.Load<GameObject>("ModelBullet");
        m_weapon.SetBullet(m_bulletObj, OnTriggerEnter);
    }

    protected override void InitAnimation() {
        AddAnimation(SRoleState.Type.SRoleStand.ToString(), "DrawBlade");
        AddAnimation(SRoleState.Type.SRoleRun.ToString(), "Run00");
        AddAnimation(SRoleState.Type.SRoleReadyFight.ToString(), "DrawBlade");
    }

    protected override void InitAttackSkillAnimation() { }

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
        State = SRoleState.Type.SRoleRun;
    }

    public override void EndRun() {
        base.EndRun();
        m_curFoward = m_transform.rotation.eulerAngles.y;
    }

    public override void Attack() {
        float lastTime = Time.time;
        Collider[] targets = SearchAttactTarget();
        float passTime = Time.time - lastTime;
        switch (m_attackLevel) {
            case ModelAttackLevel.Normal:
            case ModelAttackLevel.SkillOne:
                PlaySkillAnimator(Vector3.zero);
                break;
            case ModelAttackLevel.SkillTwo:
            case ModelAttackLevel.SkillThree:
                PlaySkillAnimator(targets[0].transform.position);
                break;
        }
        if (targets == null)
            return;
        for (int i = 0; i < targets.Length; i++) {
            ModelAttackData attackData = new ModelAttackData(RoleType.Player, (int)PlayerType.Master, m_attackLevel, m_gameObject.name, targets[i].name);
            EventManager.Dispatch(GameMoudle.Monster, GameEvent.Type.Damage, attackData);
        }
    }

    private Collider[] SearchAttactTarget() {
        Collider[] colliders = Physics.OverlapSphere(m_transform.position, m_attackDis, LayerMask.GetMask(GameLayerInfo.Enemy.ToString()));
        float minAngle = GameConst.RoundAngle;
        //ModelAttackData attackData;
        List<Collider> listTarget = new List<Collider>();
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].isTrigger || colliders[i].tag != GameTagInfo.Enemy.ToString())
                continue;
            Vector3 selfToTarget = colliders[i].transform.position - m_transform.position;
            float selfToTargetAngle = Vector3.Angle(selfToTarget, m_transform.forward);
            if (selfToTargetAngle > m_attackAngle)
                continue;
            if (IsShortWeapon)
                listTarget.Add(colliders[i]);
            else if (selfToTargetAngle < minAngle) {
                listTarget.Clear();
                minAngle = selfToTargetAngle;
                listTarget.Add(colliders[i]);
            }
        }
        //m_weapon.Shoot(transform.forward);
        if (IsShortWeapon)
            return listTarget.ToArray();
        if (minAngle == GameConst.RoundAngle)
            return null;
        return listTarget.ToArray();
    }

    public override float Damage(ModelAttackData data) {
        float percent = base.Damage(data);
        DebugTool.Log("ModelPlayer::Damage " + m_healthPoint.ToString());
        return percent;
    }

    public override void Death() { }

    private void OnTriggerEnter(Collider collider) {
        ModelAttackData attackData = new ModelAttackData(RoleType.Player, (int)PlayerType.Master, 
                                                            ModelAttackLevel.Normal, m_gameObject.name, collider.name);
        EventManager.Dispatch(GameMoudle.Monster, GameEvent.Type.Damage, attackData);
    }
}