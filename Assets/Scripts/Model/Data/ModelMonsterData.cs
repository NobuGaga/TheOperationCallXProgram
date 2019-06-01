using UnityEngine;
using System.Collections.Generic;

public struct ModelMonsterData {
    private MonsterType type;
    public MonsterType Type => type;
    private Dictionary<GameScene, Vector3[]> dicScenePos;
    private string prefabName;
    public string PrefabName => prefabName;
    private GameObject prefab;
    public GameObject Prefab => prefab;
    private int maxHP;
    private float speed;
    public float Speed => speed;
    private float attackDis;
    private int attackAngle;
    public int headHeight;
    public int trackDis;

    public ModelMonsterData(MonsterType type, string prefabName, int maxHP, float speed, int headHeight, int trackDis, float attackDis, int attackAngle, Dictionary<GameScene, Vector3[]> dicScenePos = null) {
        this.type = type;
        if (dicScenePos == null)
            this.dicScenePos = new Dictionary<GameScene, Vector3[]>();
        else
            this.dicScenePos = dicScenePos;
        prefab = Resources.Load<GameObject>(prefabName);
        string[] paths = prefabName.Split('/');
        this.prefabName = paths[paths.Length - 1];
        this.maxHP = maxHP;
        this.speed = speed;
        this.headHeight = headHeight;
        this.trackDis = trackDis;
        this.attackDis = attackDis;
        this.attackAngle = attackAngle;
    }

    public void AddScenePosition(GameScene scene, Vector3 start, Vector3 end) {
        if (dicScenePos == null)
            dicScenePos = new Dictionary<GameScene, Vector3[]>();
        if (!dicScenePos.ContainsKey(scene))
            dicScenePos.Add(scene, new Vector3[2]);
        dicScenePos[scene][0] = start;
        dicScenePos[scene][1] = end;
    }

    public Vector3 GetScenePostion() {
        return GetScenePostion(GameSceneManager.CurScene);
    }

    public Vector3 GetScenePostion(GameScene scene) {
        if (!dicScenePos.ContainsKey(scene))
            return Vector3.zero;
        Vector3 start = dicScenePos[scene][0];
        Vector3 end = dicScenePos[scene][1];
        float x = Random.Range(start.x, end.x);
        float y = Random.Range(start.y, end.y);
        float z = Random.Range(start.z, end.z);
        return new Vector3(x, y, z);
    }

    public ModelAttackRoleData AttackRoleData {
        get {
            return new ModelAttackRoleData(maxHP, attackDis, attackAngle);
        }
    }
}