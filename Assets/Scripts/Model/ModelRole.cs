using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(Animation))]
public abstract class ModelRole:MonoBehaviour {
    protected Rigidbody m_rigidBody;
    private Animation m_animation;
    private Dictionary<string, string> m_defaultStateAnimation;

    private RoleState m_curState;
    public RoleState.Type State {
        set {
            bool isFirst = m_curState == null;
            if (!isFirst && m_curState.GetState() == value)
                return;
            if (!isFirst)
                m_curState.Exit();
            if (!m_dicStateCache.ContainsKey(value))
                m_curState = Activator.CreateInstance(Type.GetType(value.ToString()), 
                                                        this, m_animation) as RoleState;
            else
                m_curState = m_dicStateCache[value];
            m_curState.Enter();
        }
        get {
            return m_curState.GetState();
        }
    }
    private Dictionary<RoleState.Type, RoleState> m_dicStateCache = new Dictionary<RoleState.Type, RoleState>();

    protected float m_rotationY;
    protected Vector3 m_velocity = Vector3.zero;
    public Vector3 Velocity {
        set {
            m_velocity = value;
        }
        get {
            return m_velocity;
        }
    }

    protected virtual void Awake() {
        m_rigidBody = GetComponent<Rigidbody>();
        m_animation = GetComponent<Animation>();
    }

    protected virtual void Start() {
        InitAnimation();
        State = RoleState.Type.SRoleStand;
    }

    public void UpdateState() {
        m_curState.Update();
    }

    protected abstract void InitAnimation();

    protected void AddAnimation(string state, string animationName) {
        if (m_defaultStateAnimation == null)
            m_defaultStateAnimation = new Dictionary<string, string>();
        if (m_defaultStateAnimation.ContainsKey(state))
            m_defaultStateAnimation[state] = animationName;
        else
            m_defaultStateAnimation.Add(state, animationName);
    }

    public string GetAnimationName(string state) {
        if (m_defaultStateAnimation == null)
            return string.Empty;
        if (m_defaultStateAnimation.ContainsKey(state))
            return m_defaultStateAnimation[state];
        return string.Empty;
    }

    public virtual void Run() {
        m_rigidBody.velocity = m_velocity;
        transform.rotation = Quaternion.Euler(0, m_rotationY, 0);
    }

    public virtual void EndRun() {
        m_rigidBody.velocity = Vector3.zero;
    }

    public virtual void Attack(ModelAttackLevel level) { }

    public virtual void Damage() { }

    public virtual bool IsHPZero {
        get {
            return false;
        }
    }
}