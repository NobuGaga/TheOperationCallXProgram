using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 角色状态的抽象基类
/// </summary>
public abstract class RoleState {
    protected ModelRole m_role;
    private Animation m_animation;

    public RoleState(ModelRole role, Animation animation) {
        m_role = role;
        m_animation = animation;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    public virtual void Enter() {
        PlayAnimation(GetState());
    }

    /// <summary>
    /// 状态更新
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// 离开状态
    /// </summary>
    public abstract void Exit();

    public abstract Type GetState();

    /// <summary>
    /// 状态枚举, 必须与类名一致
    /// </summary>
    public enum Type {
        SRoleStand,
        SRoleWalk,
        SRoleRun,
        SRoleJump,
        SRoleReadyFight,
        SRoleAttack,
        SRoleDamage,
        SRoleDeath,
    }

    protected void PlayAnimation(Type state) {
        string animationName = m_role.GetAnimationName(state.ToString());
        if (animationName == string.Empty)
            animationName = GetAnimationName(state);
        if (animationName == string.Empty)
            DebugTool.Log(m_role.gameObject.name + " node not exit animation " + animationName);
        else if (!m_animation.IsPlaying(animationName))
            m_animation.Play(animationName);
    }

    private static readonly Dictionary<Type, string> defaultStateAnimation = new Dictionary<Type, string>() {
        { Type.SRoleStand,      "Stand"      },
        { Type.SRoleWalk,       "Walk"       },
        { Type.SRoleRun,        "Run"        },
        { Type.SRoleJump,       "Jump"       },
        { Type.SRoleReadyFight, "ReadyFight" },
        { Type.SRoleAttack,     "Attack"     },
        { Type.SRoleDamage,     "Damage"     },
        { Type.SRoleDeath,      "Death"      },
    };

    private static string GetAnimationName(Type state) {
        if (defaultStateAnimation.ContainsKey(state))
            return defaultStateAnimation[state];
        return string.Empty;
    }
}