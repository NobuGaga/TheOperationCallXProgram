using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class ModelRole {
    protected GameObject m_gameObject;
    public GameObject gameObject {
        get {
            return m_gameObject;
        }
    }
    protected Transform m_transform;
    public Transform transform {
        get {
            return m_transform;
        }
    }
    protected Rigidbody m_rigidBody;
    private Animation m_animation;

    public ModelRole(GameObject node) {
        m_gameObject = node;
        m_transform = m_gameObject.transform;
        m_rigidBody = m_gameObject.GetComponent<Rigidbody>();
        m_animation = m_gameObject.GetComponent<Animation>();
        InitAnimation();
        State = SRoleState.Type.SRoleStand;
    }

    private SRoleState m_curState;
    public SRoleState.Type State {
        set {
            bool isFirst = m_curState == null;
            if (!isFirst && (m_curState.GetState() == value || m_curState.GetState() == SRoleState.Type.SRoleDeath))
                return;
            if (!isFirst)
                m_curState.Exit();
            if (!m_dicStateCache.ContainsKey(value))
                m_curState = Activator.CreateInstance(Type.GetType(value.ToString()), 
                                                        this, m_animation) as SRoleState;
            else
                m_curState = m_dicStateCache[value];
            m_curState.Enter();
        }
        get {
            return m_curState.GetState();
        }
    }
    private Dictionary<SRoleState.Type, SRoleState> m_dicStateCache = new Dictionary<SRoleState.Type, SRoleState>();

    public virtual void Update() {
        m_curState.Update();
    }

    private Dictionary<string, string> m_defaultStateAnimation;

    protected abstract void InitAnimation();

    protected void AddAnimation(string state, string animationName) {
        if (m_defaultStateAnimation == null)
            m_defaultStateAnimation = new Dictionary<string, string>();
        if (m_defaultStateAnimation.ContainsKey(state))
            m_defaultStateAnimation[state] = animationName;
        else
            m_defaultStateAnimation.Add(state, animationName);
    }

    public virtual string GetAnimationName(string state) {
        if (m_defaultStateAnimation == null)
            return string.Empty;
        if (m_defaultStateAnimation.ContainsKey(state))
            return m_defaultStateAnimation[state];
        return string.Empty;
    }

    ~ModelRole() {
        m_dicStateCache?.Clear();
        m_animation = null;
        m_rigidBody = null;
        m_transform = null;
        GameObject.Destroy(m_gameObject);
        m_gameObject = null;
    }
}