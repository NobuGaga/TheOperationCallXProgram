using UnityEngine;

public abstract class ModelRunRole:ModelRole {
    protected float m_rotationY;
    protected Vector3 m_velocity = Vector3.zero;

    public ModelRunRole(GameObject node) : base(node) { }

    public Vector3 Velocity {
        set {
            m_velocity = value;
        }
        get {
            return m_velocity;
        }
    }

    public virtual void Run() {
        m_rigidBody.velocity = m_velocity;
        m_transform.rotation = Quaternion.Euler(0, m_rotationY, 0);
    }

    public virtual void EndRun() {
        m_rigidBody.velocity = Vector3.zero;
    }
}