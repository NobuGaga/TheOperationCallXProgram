using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class ModelWeaponRole:ModelAttackRole {
    private ModelWeapon m_weapon;
    private Transform m_leftHand;
    private Transform m_rightHand;
    private Dictionary<string, Transform> m_dicNodeName;

    public ModelWeaponRole(GameObject node, int healthPoint, string leftHandName = "", string rightHandName = ""):base(node, healthPoint) {
        SetHands(leftHandName, rightHandName);
    }

    protected void SetHands(string leftHandName, string rightHandName) {
        if (leftHandName == null || rightHandName == null ||
            leftHandName == string.Empty || rightHandName == string.Empty)
            return;
        Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
        if (trans == null)
            return;
        for (int i = 0; i < trans.Length; i++) {
            string nodeName = trans[i].name;
            if (leftHandName == nodeName)
                m_leftHand = trans[i];
            if (rightHandName == nodeName)
                m_rightHand = trans[i];
            if (m_leftHand != null && m_rightHand != null)
                break;
        }
    }

    public void SetWeapon(ModelWeapon weapon, Action<Collider> attackCallBack = null) {
        if (weapon == null) {
            DebugTool.LogError("ModelWeaponRole::SetWeapon weapon is null");
            return;
        }
        m_weapon = weapon;
        m_weapon.SetHands(m_leftHand, m_rightHand);
        m_leftHand = null;
        m_rightHand = null;
        if (m_weapon.IsShortWeapon && attackCallBack != null)
            m_weapon.SetCallBack(attackCallBack);
    }
}