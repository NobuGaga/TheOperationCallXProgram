using UnityEngine;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(SphereCollider))]
public class ModelMonsterVision:MonoBehaviour {
    [SerializeField]
    private SphereCollider m_collider;
    public float Area {
        get {
            return m_collider.radius;
        }
    }
    private Action<Collider> m_beginFunc;
    private Action<Collider> m_ingFunc = null;
    private Action<Collider> m_endFunc = null;

    private void Awake() {
        m_collider = GetComponent<SphereCollider>();
    }

    public void SetCallBack(Action<Collider> beginFunc, Action<Collider> ingFunc = null, Action<Collider> endFunc = null) {
        m_beginFunc = beginFunc;
        if (ingFunc != null)
            m_ingFunc = ingFunc;
        if (endFunc != null)
            m_endFunc = endFunc;
    }

    public void SetVision(int headHeight, int rang) {
        Vector3 center = m_collider.center;
        center.y = headHeight;
        m_collider.center = center;
        m_collider.radius = rang;
    }

    private void OnTriggerEnter(Collider collider) {
        m_beginFunc?.Invoke(collider);
    }

    private void OnTriggerStay(Collider collider) {
        m_ingFunc?.Invoke(collider);
    }

    private void OnTriggerExit(Collider collider) {
        m_endFunc?.Invoke(collider);
    }
}