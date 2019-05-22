using UnityEditor;
using UnityEngine;

public static class MaterialChecker {
    public static void CheckMeshRender() {
        Transform[] trans = Selection.transforms;
        for (int i = 0; i < trans.Length; i++) {
            Renderer[] meshs = trans[i].GetComponentsInChildren<Renderer>();
            if (meshs == null)
                continue;
            for (int j = 0; j < meshs.Length; j++) {
                var materials = meshs[j].sharedMaterials;
                if (materials == null)
                    continue;
                for (int k = 0; k < materials.Length; k++) {
                    if (materials[k] != null)
                        continue;
                    GameObject destroyObj = meshs[j].gameObject;
                    if (PrefabUtility.IsPartOfAnyPrefab(destroyObj)) {
                        DebugTool.LogWarning(string.Format("MaterialChecker::MaterialChecker destroy node \"{0}\" is part of prefab, please unpack prefab manually", destroyObj.name));
                        continue;
                    }
                    DebugTool.Log("MaterialChecker::MaterialChecker destroy node " + destroyObj.name);
                    GameObject.DestroyImmediate(destroyObj);
                    break;
                }
            }
        }
    }
}