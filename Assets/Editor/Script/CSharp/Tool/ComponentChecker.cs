using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public static class ComponentChecker {
    public static void CheckRenderMaterial() {
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
                        DebugTool.LogWarning(string.Format("ComponentChecker::ComponentChecker destroy node \"{0}\" is part of prefab, please unpack prefab manually", destroyObj.name));
                        continue;
                    }
                    DebugTool.Log("ComponentChecker::ComponentChecker destroy node " + destroyObj.name);
                    GameObject.DestroyImmediate(destroyObj);
                    break;
                }
            }
        }
    }

    public static void CheckMissingComponent() {
        Dictionary<string, bool> dicNodeCache = new Dictionary<string, bool>();
        GameObject[] selectNodes = Selection.gameObjects;
        bool isHasMissing = false;
        for (int i = 0; i < selectNodes.Length; i++) {
            GameObject node = selectNodes[i];
            string nodeName = node.name;
            if (dicNodeCache.ContainsKey(node.name))
                continue;
            if (CheckMissingComponent(node))
                isHasMissing = true;
            dicNodeCache.Add(nodeName, true);
            Transform[] trans = node.GetComponentsInChildren<Transform>();
            for (int j = 0; j < trans.Length; j++) {
                nodeName = trans[j].name;
                if (dicNodeCache.ContainsKey(nodeName))
                    continue;
                if (CheckMissingComponent(trans[j].gameObject))
                    isHasMissing = true;
                dicNodeCache.Add(nodeName, true);
            }
        }
        // 重新刷新或者播放时删除的丢失组件仍然会回来, 需要将场景另存为方可保存节点组件信息
        if (isHasMissing)
            DebugTool.Log("please save as scene manually");
    }

    private static bool CheckMissingComponent(GameObject node) {
        SerializedObject serializedObject = new SerializedObject(node);
        SerializedProperty properties = serializedObject.FindProperty("m_Component");
        Component[] components = node.GetComponents<Component>();
        bool isHasMissing = false;
        for (int index = 0; index < components.Length; index++)
            if (components[index] == null) {
                properties.DeleteArrayElementAtIndex(index);
                isHasMissing = true;
            }
        if (isHasMissing)
            serializedObject.ApplyModifiedProperties();
        return isHasMissing;
    }
}