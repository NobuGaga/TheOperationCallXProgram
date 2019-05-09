using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager {
    public static LogicScript m_curLogicScript;

    static GameManager() {
        GameSetting.Init();
        ControllerManager.Init();
    }

    public static void StartGame(LogicScript startScript) {
        if (startScript == null)
            return;
        m_curLogicScript = startScript;
        GameObject.DontDestroyOnLoad(startScript.gameObject);

        EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.OpenMainView);
    }

    public static void UpdateGame() {
        //AsyncOperation operation = SceneManager.LoadSceneAsync("");
        
    }
}
