using UnityEngine;

public class ModelBullet:MonoBehaviour {
    private float m_speed;
    private Transform m_target;

    private void Awake() {
        gameObject.SetActive(false);
    }

    public void Update() {
        if (m_target == null)
            return;
        Vector3 lookPoint = LookPoint;
        Vector3 direction = lookPoint - transform.position;
        if (direction.magnitude < 0.08f)
            return;
        transform.LookAt(lookPoint);
        Vector3 rotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z + Time.deltaTime * 179);
        transform.Translate(direction.normalized * Time.deltaTime * m_speed, Space.World);
    }

    public void Shoot(Transform target, float time) {
        gameObject.SetActive(true);
        transform.SetParent(GameSceneManager.GetNode<Transform>("MonsterGroup"), true);
        m_target = target;
        m_speed = distance.magnitude / time;
    }

    private Vector3 LookPoint {
        get {
            Vector3 targetPos = m_target.position;
            Vector3 selfPos = transform.position;
            Vector3 lookPoint = new Vector3(targetPos.x, selfPos.y, targetPos.z);
            return lookPoint;
        }
    }

    private Vector3 distance {
        get {
            return LookPoint - transform.position;
        }
    }
}