using UnityEngine;

public struct ModelMonsterHPData {
    public string prefabName;
    public Vector3 position;
    public int positionFix;
    public float scale;

    public ModelMonsterHPData (string prefabName, Vector3 position, int positionFix, float scale) {
        this.prefabName = prefabName;
        this.position = position;
        this.positionFix = positionFix;
        this.scale = scale;
    }
}