using UnityEngine;

public static class GameManager {
    public static LogicScript m_logicScript;
    public static LogicScript LogicScript => m_logicScript;

    static GameManager() {
        GameSetting.Init();
        GameSceneManager.Init();
        ControllerManager.Init();
        // before AssetBundleManager.Init
        ViewManager.Init();
    }

    public static void StartGame(LogicScript startScript) {
        m_logicScript = startScript;
        GameObject.DontDestroyOnLoad(startScript.gameObject);
        AssetBundleManager.Init(() => EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.OpenMainView));
    }

    public static void UpdateGame() {
        EventManager.Dispatch(GameEvent.Type.FrameUpdate, Time.time);
    }
}