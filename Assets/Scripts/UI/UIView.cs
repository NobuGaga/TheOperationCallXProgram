﻿using UnityEngine;
using System.Collections.Generic;

public class UIView: MonoBehaviour {
    [SerializeField]
    private GameObject[] m_useNode = null;
    private Dictionary<string, GameObject> m_dicUseNodeName;

    private void Awake() {
        if (m_useNode == null)
            return;
        m_dicUseNodeName = new Dictionary<string, GameObject>();
        foreach (GameObject gameObject in m_useNode)
            if (gameObject)
                m_dicUseNodeName.Add(gameObject.name, gameObject);
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
        DebugTool.LogError(string.Format("{0} is not exit in view", nodeName));
        return null;
    }
}