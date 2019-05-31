using UnityEngine;
using System;

public class ModelWeapon:MonoBehaviour {
    [SerializeField]
    private WeaponType m_type = WeaponType.ShortRange;
    [SerializeField]
    private WeaponHandType m_hand = WeaponHandType.Right;
    private Transform m_bulletParent;;

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
        m_bulletParent = leftHand.parent;
    }

    public void SetBullet(GameObject bullet, Action<Collider> beginFunc) {
        SetCallBack(beginFunc);
        m_bulletObj = bullet;
    }

    public void Shoot(Transform target, float time) {
        CreateBullet(time).Shoot(target, time);
    }

    public void Shoot(float time, float speed, int angle = 0) {
        CreateBullet(time).Shoot(speed, angle);
    }

    private ModelBullet CreateBullet(float time) {
        GameObject bullet = GameObject.Instantiate(m_bulletObj);
        bullet.transform.SetParent(m_bulletParent, false);
        ModelBullet script = bullet.GetComponent<ModelBullet>();
        TimerManager.Register(time, () => GameObject.DestroyImmediate(bullet));
        return script;
    }

    public void SetCallBack(Action<Collider> beginFunc) {
        m_beginFunc = beginFunc;
    }

    private void OnTriggerEnter(Collider collider) {
        if (m_type == WeaponType.ShortRange)
            m_beginFunc?.Invoke(collider);
    }
}