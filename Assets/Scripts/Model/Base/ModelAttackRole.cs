using UnityEngine;
using System.Collections.Generic;

public abstract class ModelAttackRole:ModelRunRole {
    protected ModelHPData m_healthPoint;
    public bool IsHPZero {
        get {
            return m_healthPoint.IsZero;
        }
    }

    protected float m_attackDis;
    protected int m_attackAngle;
    private GameObject m_damageText;
    protected ModelAttackLevel m_attackLevel = ModelAttackLevel.Normal;
    public ModelAttackLevel AttackLevel {
        set {
            m_attackLevel = value;
        }
    }
    private Dictionary<ModelAttackLevel, string> m_AttackAnimation;
    protected void AddAttackAnimation(ModelAttackLevel level, string animationName) {
        if (m_AttackAnimation == null)
            m_AttackAnimation = new Dictionary<ModelAttackLevel, string>();
        if (m_AttackAnimation.ContainsKey(level))
            m_AttackAnimation[level] = animationName;
        else
            m_AttackAnimation.Add(level, animationName);
    }

    private string GetAttackAnimationName() {
        if (m_AttackAnimation == null)
            return string.Empty;
        if (m_AttackAnimation.ContainsKey(m_attackLevel))
            return m_AttackAnimation[m_attackLevel];
        return string.Empty;
    }

    public override string GetAnimationName(string state) {
        string animationName = string.Empty;
        if (State == SRoleState.Type.SRoleAttack)
            animationName = GetAttackAnimationName();
        if (animationName == string.Empty)
            animationName = base.GetAnimationName(state);
        return animationName;
    }

    private Dictionary<ModelAttackLevel, string[]> m_SkillAnimation;
    private Dictionary<ModelAttackLevel, List<Animator>> m_SkillAnimator;
    protected void AddSkillAnimation(ModelAttackLevel level, string[] animationNames) {
        if (m_SkillAnimation == null)
            m_SkillAnimation = new Dictionary<ModelAttackLevel, string[]>();
        if (m_SkillAnimation.ContainsKey(level))
            m_SkillAnimation[level] = animationNames;
        else
            m_SkillAnimation.Add(level, animationNames);
    }

    private string[] GetSkillAnimationName() {
        if (m_SkillAnimation == null)
            return null;
        if (m_SkillAnimation.ContainsKey(m_attackLevel))
            return m_SkillAnimation[m_attackLevel];
        return null;
    }

    private void InitSkillAnimator() {
        if (m_SkillAnimation == null || m_SkillAnimation.Count == 0)
            return;
        m_SkillAnimator = new Dictionary<ModelAttackLevel, List<Animator>>();
        foreach (var keyValue in m_SkillAnimation) {
            if (keyValue.Value == null || keyValue.Value.Length == 0)
                continue;
            List<Animator> listAnimator = new List<Animator>();
            for (int i = 0; i < keyValue.Value.Length; i++) {
                string prefabPath = keyValue.Value[i];
                GameObject obj = Resources.Load<GameObject>(prefabPath);
                GameObject initObj = GameObject.Instantiate<GameObject>(obj);
                initObj.transform.SetParent(transform, false);
                initObj.SetActive(false);
                Animator animator = initObj.GetComponent<Animator>();
                listAnimator.Add(animator);
            }
            m_SkillAnimator.Add(keyValue.Key, listAnimator);
        }
    }

    public void PlaySkillAnimator() {
        if (m_SkillAnimator == null)
            return;
        if (!m_SkillAnimation.ContainsKey(m_attackLevel))
            return;
        List<Animator> list = m_SkillAnimator[m_attackLevel];
        PlaySkillAnimator(list, 0);
    }

    private void PlaySkillAnimator(List<Animator> list, int index) {
        if (index >= list.Count)
            return;
        GameObject obj = list[index].gameObject;
        obj.SetActive(true);
        AnimatorClipInfo[] clipInfos = list[index].GetCurrentAnimatorClipInfo(0);
        float time = clipInfos[0].clip.length;
        int indexCopy = ++index;
        TimerManager.Register(time, () => {
            obj.SetActive(false);
            PlaySkillAnimator(list, indexCopy);
        });
    }

    private Dictionary<ModelAttackLevel, float> m_dicAttackDelayTime;
    protected void AddAttackDelay(ModelAttackLevel level, float time) {
        if (m_dicAttackDelayTime == null)
            m_dicAttackDelayTime = new Dictionary<ModelAttackLevel, float>();
        if (m_dicAttackDelayTime.ContainsKey(level))
            m_dicAttackDelayTime[level] = time;
        else
            m_dicAttackDelayTime.Add(level, time);
    }
    public float AttackDelayTime {
        get {
            if (m_dicAttackDelayTime == null || !m_dicAttackDelayTime.ContainsKey(m_attackLevel))
                return 0;
            return m_dicAttackDelayTime[m_attackLevel];
        }
    }

    public ModelAttackRole(GameObject node, ModelAttackRoleData data):base(node) {
        InitAttackSkillAnimation();
        InitSkillAnimator();
        m_healthPoint = new ModelHPData(data.MaxHP);
        m_attackDis = data.attackDis;
        m_attackAngle = data.attackAngle;
    }

    protected abstract void InitAttackSkillAnimation();

    public abstract void Attack();
    public virtual float Damage(ModelAttackData data) {
        int damage = data.Damage;
        KeyValuePair<Transform, string> message = new KeyValuePair<Transform, string>(transform, damage.ToString());
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.ShowDamageText, message);
        m_healthPoint -= damage;
        SRoleState.Type state;
        if (IsHPZero)
            state = SRoleState.Type.SRoleDeath;
        else
            state = SRoleState.Type.SRoleDamage;
        State = state;
        return m_healthPoint.Percent;
    }
    public abstract void Death();
}