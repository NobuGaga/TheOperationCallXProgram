using UnityEngine;
using System.Collections.Generic;

public enum GameScene {
    StartScene,
    SelectScene,
    Blade_Warrior_Demo,
    DesertScene,
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

    private static readonly Dictionary<GameScene, string> m_dicSceneName = new Dictionary<GameScene, string>() {
        { GameScene.Blade_Warrior_Demo, "3D模型Demo" },
        { GameScene.DesertScene,        "沙漠"       },
    };
    public static string GetName(GameScene scene) {
        if (m_dicSceneName.ContainsKey(scene))
            return m_dicSceneName[scene];
        return string.Empty;
    }
}