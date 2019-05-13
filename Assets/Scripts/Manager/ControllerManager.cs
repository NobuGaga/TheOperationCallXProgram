using System.Collections.Generic;

public static class ControllerManager {
    private static Dictionary<GameMoudle, Controller> m_dicMoudleController = new Dictionary<GameMoudle, Controller>();

    public static void Init() {
        m_dicMoudleController.Add(GameMoudle.Loading, new Loading(GameMoudle.Loading, typeof(LoadingData)));
    }

    public static Controller GetController(GameMoudle moudle) {
        return m_dicMoudleController[moudle];
    }
}
