using UnityEngine;
using System;

public class ModelBullet:MonoBehaviour {
    [SerializeField]
    private GameTagInfo m_targetType = GameTagInfo.Enemy;
    [SerializeField]
    private float m_speed = 0.5f;

    private Vector3 m_originPositin;
    private int m_maxDistance;
    private Vector3 m_direction;
    private Action<Collider> m_beginFunc;
    private bool isDestroy = false;

    private void Awake() {
        m_originPositin = transform.position;
        gameObject.SetActive(false);
    }

    public void Update() {
        transform.Translate(m_direction * Time.deltaTime * m_speed, Space.World);
        if ((transform.position - m_originPositin).magnitude > m_maxDistance)
            GameObject.DestroyImmediate(gameObject);
    }

    private void LateUpdate() {
        if (isDestroy)
            GameObject.DestroyImmediate(gameObject);
    }

    public void Shoot(Vector3 direction, int maxDistance, Action<Collider> beginFunc) {
        m_beginFunc = beginFunc;
        m_maxDistance = maxDistance;
        m_direction = direction;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag != m_targetType.ToString())
            return;
        m_beginFunc?.Invoke(collider);
        isDestroy = true;
    }
}