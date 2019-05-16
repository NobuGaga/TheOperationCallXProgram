using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager {
    private static GameScene m_curScene;
    private static AsyncOperation m_asyncOperation;
    public static float LoadSceneProcess {
        get {
            if (m_asyncOperation == null)
                return -1;
            return m_asyncOperation.progress;
        }
    }
    private static Dictionary<GameScene, GameSceneInfo> m_dicSceneInfo = new Dictionary<GameScene, GameSceneInfo>();

    public static void Init() {
        m_curScene = GameScene.StartScene;
    }

    private static void SetSceneInfo(GameScene scene, Scene sceneInfo) {
        GameObject[] nodes = sceneInfo.GetRootGameObjects();
        if (m_dicSceneInfo.ContainsKey(scene)) {
            m_dicSceneInfo[scene].Clear();
            if (nodes == null)
                return;
            for (int i = 0; i < nodes.Length; i++)
                m_dicSceneInfo[scene].SetNode(nodes[i].name, nodes[i]);
        } else
            m_dicSceneInfo.Add(scene, new GameSceneInfo(scene, nodes));
    }

    public static GameObject GetNode(string nodeName) {
        if (m_curScene == GameScene.StartScene || IsCacheMoudle(nodeName))
            return GameManager.LogicScript.gameObject;
        if (!m_dicSceneInfo.ContainsKey(m_curScene))
            return null;
        if (nodeName == null)
            return m_dicSceneInfo[m_curScene].DefaultNode;
        return m_dicSceneInfo[m_curScene].GetNode(nodeName);
    }
    
    private static bool IsCacheMoudle(string nodeName) {
        if (nodeName == GameViewInfo.GetViewName(GameMoudle.Loading, GameView.MainView))
            return true;
        return false;
    }

    public static void ChangeScene(GameScene scene) {
        if (m_curScene != GameScene.StartScene)
            EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.OpenMainView);
        m_asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        m_asyncOperation.completed += (m_asyncOperation) => {
            m_curScene = scene;
            m_asyncOperation = null;
            SetSceneInfo(m_curScene, SceneManager.GetSceneByName(m_curScene.ToString()));
            OpenScene();
        };
    }

    private static void OpenScene() {
        switch (m_curScene) {
            case GameScene.SelectScene:
                EventManager.Dispatch(GameMoudle.Select, GameEvent.Type.OpenMainView);
                break;
        }
    }
}