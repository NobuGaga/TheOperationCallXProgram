using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPrefab:MonoBehaviour {
    [SerializeField]
    private GameObject[] m_useNode = null;
    private Dictionary<string, GameObject> m_dicUseNodeName;

    protected virtual void Awake() {
        if (m_useNode == null)
            return;
        m_dicUseNodeName = new Dictionary<string, GameObject>();
        foreach (GameObject gameObj in m_useNode) {
            if (gameObj == null)
                continue;
            string nodeName = gameObj.name;
            m_dicUseNodeName.Add(nodeName, gameObj);
            if (nodeName.Contains("text"))
                gameObj.GetComponent<Text>().text = string.Empty;
        }
    }

    public T GetNode<T>(string nodeName) {
        GameObject go = GetNode(nodeName);
        if (go)
            return go.GetComponent<T>();
        return default;
    }

    public GameObject GetNode(string nodeName) {
        if (m_dicUseNodeName == null) {
            DebugTool.LogError("Dictionary node is null");
            return null;
        }
        if (m_dicUseNodeName.ContainsKey(nodeName))
            return m_dicUseNodeName[nodeName];
        DebugTool.LogError(string.Format("{0} is not exit in prefab", nodeName));
        return null;
    }

    ~UIPrefab() {
        m_dicUseNodeName.Clear();
        if (m_useNode != null)
            for (int i = 0; i < m_useNode.Length; i++)
                m_useNode[i] = null;
    }
}