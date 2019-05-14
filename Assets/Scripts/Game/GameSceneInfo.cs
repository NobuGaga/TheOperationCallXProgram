using UnityEngine;
using System.Collections.Generic;

public enum GameScene {
    StartScene,
    SelectScene,
    Forest,
    Desert,
    City,
}

public struct GameSceneInfo {
    public GameScene scene;
    private Dictionary<string, GameObject> dicNameNode;
    private string defaultNodeName;
    public GameObject DefaultNode {
        get {
            return dicNameNode[defaultNodeName];
        }
    }

    public GameSceneInfo(GameScene scene, GameObject[] nodes) {
        this.scene = scene;
        dicNameNode = new Dictionary<string, GameObject>();
        defaultNodeName = string.Empty;
        for (int i = 0; i < nodes.Length; i++) {
            string nodeName = nodes[i].name;
            if (i == 0)
                defaultNodeName = nodeName;
            dicNameNode.Add(nodeName, nodes[i]);
        }
    }

    public void Clear() {
        if (dicNameNode != null)
            dicNameNode.Clear();
        defaultNodeName = string.Empty;
    }

    public void SetNode(string nodeName, GameObject node) {
        if (dicNameNode.ContainsKey(nodeName))
            dicNameNode.Remove(nodeName);
        dicNameNode.Add(nodeName, node);
        if (defaultNodeName == string.Empty)
            defaultNodeName = nodeName;
    }

    public GameObject GetNode(string nodeName) {
        if (dicNameNode.ContainsKey(nodeName))
            return dicNameNode[nodeName];
        return null;
    }
}