using UnityEngine;
using System.Collections.Generic;

public class ModelSkill {
    private ModelAttackLevel level;
    private List<Animator> listAnimators;
    private Dictionary<int, bool> dicPostitionNode;
    private float attackDelay;
    private float AttackDelayTime {
        get {
            return attackDelay;
        }
    }
    private float startDelay;

    public ModelSkill(ModelAttackLevel level, string[] prefabPaths, int[] positionPrefab, Transform parent, float startDelay, float attackDelay) {
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
        if (startDelay == 0)
            PlaySkillAnimator(listAnimators, 0, position);
        else
            TimerManager.Register(startDelay, () => PlaySkillAnimator(listAnimators, 0, position));
    }

    private void PlaySkillAnimator(List<Animator> list, int index, Vector3 position) {
        if (index >= list.Count)
            return;
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
}