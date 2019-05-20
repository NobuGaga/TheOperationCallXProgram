using UnityEngine;

public class ModelPlayer:ModelRole {
    private float m_curFoward;

    protected override void Start() {
        base.Start();
        AddAnimation(State.Stand.ToString(), "DrawBlade");
        AddAnimation(State.Run.ToString(), "Run00");
        AddAnimation(State.ReadyFight.ToString(), "DrawBlade");

        m_curFoward = transform.rotation.eulerAngles.y;
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    /// <param name="speed">虚拟摇杆转换为三维坐标, 该坐标系与 OpenGL 坐标系相同, 经过归一化</param>
    /// <param name="rotationY">Y 轴旋转角度</param>
    public override void Move(Vector3 speed, float rotationY) {
        rotationY += m_curFoward;
        NormalizedAngle(ref rotationY);
        speed = Quaternion.Euler(0, m_curFoward, 0) * speed;
        base.Move(speed, rotationY);
    }

    public void EndMove() {
        m_curFoward = transform.rotation.eulerAngles.y;
    }

    private void NormalizedAngle(ref float angle) {
        if (angle > GameConst.RoundAngle || angle < -GameConst.RoundAngle)
            angle %= GameConst.RoundAngle;
    }
}