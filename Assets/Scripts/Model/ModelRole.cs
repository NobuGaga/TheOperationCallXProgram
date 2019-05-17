using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody), typeof(Animation))]
public class ModelRole:MonoBehaviour {
    [SerializeField]
    private Rigidbody m_rigidBody;
    [SerializeField]
    private Animation m_animation;
    protected State m_state = State.Stand;
    protected Dictionary<string, string> m_dicStateAnimation;

    protected virtual void Awake() {
        m_rigidBody = GetComponent<Rigidbody>();
        m_animation = GetComponent<Animation>();
    }

    private void Start() {
        PlayAnimation(State.Stand);
    }

    public virtual void Move(Vector3 speed, float rotationY) {
        PlayAnimation(State.Run);
        m_rigidBody.velocity = speed;
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    public virtual void Stop() {
        PlayAnimation(State.Stand);
        m_rigidBody.velocity = Vector3.zero;
    }

    protected virtual void PlayAnimation(State state) {
        string animationName = GetAnimationName(state.ToString());
        if (animationName == string.Empty)
            animationName = GetAnimationName(state);
        if (animationName == string.Empty)
            DebugTool.Log(gameObject.name + " node not exit animation " + animationName);
        else if (!m_animation.IsPlaying(animationName))
            m_animation.Play(animationName);
    }

    protected void AddAnimation(string state, string animationName) {
        if (m_dicStateAnimation == null)
            m_dicStateAnimation = new Dictionary<string, string>();
        if (m_dicStateAnimation.ContainsKey(state))
            m_dicStateAnimation[state] = animationName;
        else
            m_dicStateAnimation.Add(state, animationName);
    }

    protected string GetAnimationName(string state) {
        if (m_dicStateAnimation == null)
            return string.Empty;
        if (m_dicStateAnimation.ContainsKey(state))
            return m_dicStateAnimation[state];
        return string.Empty;
    }

    protected enum State {
        Stand,
        Walk,
        Run,
        Jump,
        ReadyFight,
        Attack,
        Damage,
        Death
    }

    private static readonly Dictionary<State, string> dicStateAnimation = new Dictionary<State, string>() {
        { State.Stand,      "Stand"      },
        { State.Walk,       "Walk"       },
        { State.Run,        "Run"        },
        { State.Jump,       "Jump"       },
        { State.ReadyFight, "ReadyFight" },
        { State.Attack,     "Attack"     },
        { State.Damage,     "Damage"     },
        { State.Death,      "Death"      }
    };

    protected static string GetAnimationName(State state) {
        if (dicStateAnimation.ContainsKey(state))
            return dicStateAnimation[state];
        return string.Empty;
    }
}