using UnityEngine;
using System;

public class ModelWeapon:MonoBehaviour {
    [SerializeField]
    private WeaponType m_type = WeaponType.ShortRange;
    [SerializeField]
    private WeaponHandType m_hand = WeaponHandType.Right;
    [SerializeField]
    private Transform m_shootPoint;

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

    public void Shoot(Transform target, float time) {
        GameObject bullet = GameObject.Instantiate(m_bulletObj);
        if (m_shootPoint == null)
            m_shootPoint = transform;
        bullet.transform.SetParent(m_shootPoint, false);
        ModelBullet script = bullet.GetComponent<ModelBullet>();
        script.Shoot(target, time);
        TimerManager.Register(time, () => GameObject.DestroyImmediate(bullet));
    }

    public void SetCallBack(Action<Collider> beginFunc) {
        m_beginFunc = beginFunc;
    }

    private void OnTriggerEnter(Collider collider) {
        if (m_type == WeaponType.ShortRange)
            m_beginFunc?.Invoke(collider);
    }
}