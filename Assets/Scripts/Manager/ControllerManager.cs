using System.Collections.Generic;

public static class ControllerManager {
    private static Dictionary<GameMoudle, Controller> m_dicMoudleController = new Dictionary<GameMoudle, Controller>();

    public static void Init() {
        m_dicMoudleController.Add(GameMoudle.Loading, new CLoading(GameMoudle.Loading, typeof(MLoadingData)));
        m_dicMoudleController.Add(GameMoudle.Select, new CSelect(GameMoudle.Select, typeof(MSelectData)));
    }

    public static Controller GetController(GameMoudle moudle) {
        if (!m_dicMoudleController.ContainsKey(moudle))
            DebugTool.LogError(string.Format("ControllerManager : {0} moudle not register in Init", moudle.ToString()));
        return m_dicMoudleController[moudle];
    }
}