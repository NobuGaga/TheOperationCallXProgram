using UnityEngine;
using System.Collections.Generic;

public class ModelDamageEffect {
    private GameObject m_prefab;
    private Vector3 m_originPos;
    private float m_lastTime;
    private Stack<GameObject> effectPool;

    public ModelDamageEffect(string prefabPath) {
        m_prefab = Resources.Load<GameObject>(prefabPath);
        effectPool = new Stack<GameObject>();
        GameObject effect = GameObject.Instantiate(m_prefab);
        m_originPos = effect.transform.position;
        Animator animator = effect.GetComponent<Animator>();
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        m_lastTime = clipInfos[0].clip.length;
        effect.SetActive(false);
        effectPool.Push(effect);
    }

    public void Show(Transform parent) {
        GameObject effect = null;
        if (effectPool.Count != 0)
            effect = effectPool.Pop();
        if (effect == null)
            effect = GameObject.Instantiate(m_prefab, parent, false);
        else {
            Transform trans = effect.transform;
            trans.SetParent(parent, false);
            trans.position = m_originPos;
            effect.SetActive(true);
        }
        TimerManager.Register(m_lastTime, () => {
            effect.SetActive(false);
            effect.transform.SetParent(effect.transform.parent);
            effectPool.Push(effect);
        });
    }
}