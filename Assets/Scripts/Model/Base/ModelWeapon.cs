using UnityEngine;
using System;

public class ModelWeapon:MonoBehaviour {
    [SerializeField]
    private WeaponType m_type;
    [SerializeField]
    private WeaponHandType m_hand;

    private GameObject m_bulletObj;
    private Action<Collider> m_beginFunc;

    public bool IsShortWeapon {
        get {
            return m_type == WeaponType.ShortRange;
        }
    }

    public void SetHands(Transform leftHand, Transform rightHand) {
        if (m_hand == WeaponHandType.Left)
            transform.SetParent(leftHand, false);
        else
            transform.SetParent(rightHand, false);
    }

    public void SetBullet(GameObject bullet, Action<Collider> beginFunc) {
        SetCallBack(beginFunc);
        m_bulletObj = bullet;
    }

    public void Shoot(Vector3 direction) {
        GameObject bullet = GameObject.Instantiate(m_bulletObj);
        bullet.transform.SetParent(transform, false);
        ModelBullet script = bullet.GetComponent<ModelBullet>();
        script.Shoot(direction, 10, m_beginFunc);
    }

    public void SetCallBack(Action<Collider> beginFunc) {
        m_beginFunc = beginFunc;
    }

    private void OnTriggerEnter(Collider collider) {
        if (m_type == WeaponType.ShortRange)
            m_beginFunc?.Invoke(collider);
    }
}