using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager {
    private static GameScene m_curScene;
    public static GameScene CurScene => m_curScene;
    private static AsyncOperation m_asyncOperation;
    public static float LoadSceneProcess {
        get {
            if (m_asyncOperation == null)
                return -1;
            return m_asyncOperation.progress;
        }
    }
    public static LogicScript m_curLogicScript;
    public static LogicScript LogicScript => m_curLogicScript;

    static GameManager() {
        m_curScene = GameScene.StartScene;
        GameSetting.Init();
        ControllerManager.Init();
        ViewManager.Init();
    }

    public static void StartGame(LogicScript startScript) {
        m_curLogicScript = startScript;
        GameObject.DontDestroyOnLoad(startScript.gameObject);
        EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.OpenMainView);
    }

    public static void UpdateGame() {
        EventManager.Dispatch(GameEvent.Type.FrameUpdate, Time.time);
    }

    public static void ChangeScene(GameScene scene) {
        m_asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        m_asyncOperation.completed += (m_asyncOperation) => {
            m_curScene = scene;
            m_asyncOperation = null;
        };
    }
}