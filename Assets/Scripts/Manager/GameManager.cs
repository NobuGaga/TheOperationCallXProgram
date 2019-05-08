public static class GameManager {
    public static LogicScript m_curLogicScript;

    private static void Init() {
        GameConfig.Init();
        ControllerManager.Init();
    }

    public static void StartGame(LogicScript startScript) {
        if (startScript == null)
            return;
        m_curLogicScript = startScript;
        //GameObject.DontDestroyOnLoad(startScript.gameObject);

        Init();

        EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.RefreshMainView);
    }

    public static void UpdateGame() {
            
    }
}
