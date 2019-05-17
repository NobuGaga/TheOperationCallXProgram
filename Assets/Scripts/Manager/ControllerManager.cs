using System.Collections.Generic;

public static class ControllerManager {
    private static Dictionary<GameMoudle, Controller> dicMoudleController = new Dictionary<GameMoudle, Controller>();

    public static void Init() {
        dicMoudleController.Add(GameMoudle.Loading, new CLoading(GameMoudle.Loading));
        dicMoudleController.Add(GameMoudle.Select, new CSelect(GameMoudle.Select, typeof(MSelectData)));
        dicMoudleController.Add(GameMoudle.VirtualButton, new CVirtualButton(GameMoudle.VirtualButton));
        dicMoudleController.Add(GameMoudle.Player, new CPlayer(GameMoudle.Player, typeof(MPlayerData)));
    }

    public static Controller GetController(GameMoudle moudle) {
        if (!dicMoudleController.ContainsKey(moudle))
            DebugTool.LogError(string.Format("ControllerManager : {0} moudle not register in Init", moudle.ToString()));
        return dicMoudleController[moudle];
    }
}