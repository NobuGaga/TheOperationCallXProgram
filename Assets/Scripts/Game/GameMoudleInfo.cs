using System.Collections.Generic;

public enum GameMoudle {
    Loading,
    Select,
    VirtualButton,
    Player,
}

public enum GameView {
    MainView,
}

public struct GameViewInfo {
    private GameMoudle moudle;
    private string name;
    private bool isUI;
    private string assetBundleName;

    public GameMoudle Moudle => moudle;
    public string Name => name;
    public bool IsUI => isUI;
    public string AssetBundleName => assetBundleName;
    public string ParentName {
        get {
            return GetParent(moudle, name);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="moudle">模块类型</param>
    /// <param name="name">界面名</param>
    /// <param name="isUI">是否是UI界面</param>
    public GameViewInfo(GameMoudle moudle, string name, bool isUI) {
        this.moudle = moudle;
        this.name = name;
        this.isUI = isUI;
        assetBundleName = string.Empty;
        SetAssetBundleName();
    }

    /// <summary>
    /// </summary>
    /// <param name="moudle">模块类型</param>
    /// <param viewType="viewType">界面类型</param>
    public GameViewInfo(GameMoudle moudle, GameView viewType) {
        this.moudle = moudle;
        name = GetViewName(moudle, viewType);
        isUI = true;
        assetBundleName = string.Empty;
        SetAssetBundleName();
    }

    private void SetAssetBundleName() {
        if (isUI)
            assetBundleName = string.Format("{0}{1}/ui", PathConfig.AssetBundleMoudlePath, moudle.ToString().ToLower());
        else
            assetBundleName = string.Format("{0}{1}/{2}", PathConfig.AssetBundleMoudlePath, moudle.ToString().ToLower(), name.ToLower());
    }

    public static string GetViewName(GameMoudle moudle, GameView viewType) {
        return string.Format("{0}{1}", moudle.ToString(), viewType.ToString());
    }

    private static Dictionary<string, string> dicViewParent = new Dictionary<string, string>();
    private static string GetParent(GameMoudle moudle, string viewName) {
        if (!dicViewParent.ContainsKey(viewName)) {
            DebugTool.LogError(string.Format("GameViewInfo moudle:{0}, view:{1}, not extt parent", moudle.ToString(), viewName));
            return string.Empty;
        }
        return dicViewParent[viewName];
    }

    static GameViewInfo() {
        dicViewParent.Add(GetViewName(GameMoudle.Loading, GameView.MainView), GameConst.GameCamera);
        dicViewParent.Add(GetViewName(GameMoudle.Select, GameView.MainView), "UICanvas");
        dicViewParent.Add(GetViewName(GameMoudle.VirtualButton, GameView.MainView), GameConst.GameCamera);
    }
}