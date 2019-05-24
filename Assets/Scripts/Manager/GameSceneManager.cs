using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager {
    private static GameScene curScene;
    private static AsyncOperation asyncOperation;
    public static float LoadSceneProcess {
        get {
            if (asyncOperation == null)
                return -1;
            return asyncOperation.progress;
        }
    }
    private static Dictionary<GameScene, GameSceneInfo> dicSceneInfo = new Dictionary<GameScene, GameSceneInfo>();

    public static void Init() {
        curScene = GameScene.StartScene;
    }

    private static void SetSceneInfo(GameScene scene, Scene sceneInfo) {
        GameObject[] nodes = sceneInfo.GetRootGameObjects();
        if (dicSceneInfo.ContainsKey(scene)) {
            dicSceneInfo[scene].Clear();
            if (nodes == null)
                return;
            for (int i = 0; i < nodes.Length; i++)
                dicSceneInfo[scene].SetNode(nodes[i].name, nodes[i]);
        } else
            dicSceneInfo.Add(scene, new GameSceneInfo(scene, nodes));
    }

    public static Dictionary<string, GameObject> GetAllNode(GameScene scene) {
        if (!dicSceneInfo.ContainsKey(scene))
            return null;
        return dicSceneInfo[scene].AllNameNode;
    }

    public static GameObject GetNode(string nodeName) {
        if (IsCacheMoudle(nodeName))
            return GameManager.LogicScript.gameObject;
        if (!dicSceneInfo.ContainsKey(curScene))
            return null;
        if (nodeName == null)
            return dicSceneInfo[curScene].DefaultNode;
        return dicSceneInfo[curScene].GetNode(nodeName);
    }
    
    public static T GetNode<T>(string nodeName) {
        GameObject node = GetNode(nodeName);
        return node.GetComponent<T>();
    }

    private static bool IsCacheMoudle(string nodeName) {
        if (nodeName == GameConst.GameCamera)
            return true;
        return false;
    }

    public static void ChangeScene(GameScene scene) {
        if (curScene != GameScene.StartScene)
            EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.OpenMainView);
        asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        asyncOperation.completed += (asyncOperation) => {
            curScene = scene;
            asyncOperation = null;
            SetSceneInfo(curScene, SceneManager.GetSceneByName(curScene.ToString()));
            OpenScene();
        };
    }

    private static void OpenScene() {
        switch (curScene) {
            case GameScene.SelectScene:
                EventManager.Dispatch(GameMoudle.Select, GameEvent.Type.OpenMainView);
                break;
            case GameScene.DesertScene:
                EventManager.Dispatch(GameMoudle.VirtualButton, GameEvent.Type.OpenMainView);
                Dictionary<string, GameObject> allNodes = GetAllNode(curScene);
                EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.OpenMainView, allNodes);
                EventManager.Dispatch(GameMoudle.Monster, GameEvent.Type.InitModel, allNodes);
                break;
        }
    }
}