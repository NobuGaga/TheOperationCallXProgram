using UnityEngine;
using System.Collections.Generic;

public class ModelSkill {
    private ModelAttackLevel level;
    private List<Animator> listAnimators;
    private Dictionary<int, bool> dicPostitionNode;
    private float attackDelay;
    public float AttackDelayTime {
        get {
            return attackDelay;
        }
    }
    private float startDelay;
    private bool isPlaying = false;

    private string[] m_prefabPaths;
    private int[] m_positionPrefab;
    private Transform m_parent;

    public ModelSkill() { }

    public ModelSkill(ModelAttackLevel level, string[] prefabPaths, int[] positionPrefab, Transform parent, float startDelay, float attackDelay) {
        m_prefabPaths = prefabPaths;
        m_positionPrefab = positionPrefab;
        m_parent = parent;
        SetData(level, prefabPaths, positionPrefab, parent, startDelay, attackDelay);
    }

    private void SetData(ModelAttackLevel level, string[] prefabPaths, int[] positionPrefab, Transform parent, float startDelay, float attackDelay) {
        this.level = level;
        listAnimators = new List<Animator>();
        if (positionPrefab != null && positionPrefab.Length > 0)
            dicPostitionNode = new Dictionary<int, bool>();
        for (int i = 0; i < prefabPaths.Length; i++) {
            string prefabPath = prefabPaths[i];
            GameObject obj = Resources.Load<GameObject>(prefabPath);
            GameObject initObj = GameObject.Instantiate<GameObject>(obj);
            initObj.transform.SetParent(parent, false);
            initObj.SetActive(false);
            Animator animator = initObj.GetComponent<Animator>();
            listAnimators.Add(animator);
            if (positionPrefab != null && i < positionPrefab.Length && positionPrefab[i] != 0)
                dicPostitionNode.Add(i, true);
        }
        this.startDelay = startDelay;
        this.attackDelay = attackDelay;
    }

    public void Play(Vector3 position) {
        if (isPlaying) {
            ModelSkill copy = Copy();
            copy.Play(position);
            return;
        }
        isPlaying = true;
        if (startDelay == 0)
            PlaySkillAnimator(listAnimators, 0, position);
        else
            TimerManager.Register(startDelay, () => PlaySkillAnimator(listAnimators, 0, position));
    }

    private void PlaySkillAnimator(List<Animator> list, int index, Vector3 position) {
        if (index >= list.Count) {
            isPlaying = false;
            return;
        }
        GameObject obj = list[index].gameObject;
        obj.SetActive(true);
        if (dicPostitionNode != null && dicPostitionNode.ContainsKey(index))
            obj.transform.position = position;
        AnimatorClipInfo[] clipInfos = list[index].GetCurrentAnimatorClipInfo(0);
        float time = clipInfos[0].clip.length;
        int indexCopy = ++index;
        TimerManager.Register(time, () => {
            obj.SetActive(false);
            PlaySkillAnimator(list, indexCopy, position);
        });
    }

    public ModelSkill Copy() {
        ModelSkill copy = new ModelSkill();
        copy.SetData(level, m_prefabPaths, m_positionPrefab, m_parent, startDelay, attackDelay);
        return copy;
    }

    ~ModelSkill() {
        
    }
}