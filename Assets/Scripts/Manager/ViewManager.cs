using UnityEngine;
using System;
using System.Collections.Generic;

public static class ViewManager {
    private static Dictionary<string, GameViewInfo> dicViewNameInfo = new Dictionary<string, GameViewInfo>();

    public static void Init() {
        GameViewInfo info = new GameViewInfo(GameMoudle.Loading, GameView.MainView);
        dicViewNameInfo.Add(info.Name, info);
        info = new GameViewInfo(GameMoudle.Select, GameView.MainView);
        dicViewNameInfo.Add(info.Name, info);
        info = new GameViewInfo(GameMoudle.VirtualButton, GameView.MainView);
        dicViewNameInfo.Add(info.Name, info);
        info = new GameViewInfo(GameMoudle.Player, GameView.MainView);
        dicViewNameInfo.Add(info.Name, info);
    }

    public static void Open(string viewName, Action<GameObject> callback) {
        GameViewInfo viewInfo = GetViewInfo(viewName);
        AssetBundleManager.Load(viewInfo.AssetBundleName, viewInfo.Name,
            (GameObject gameObj) => {
                GameObject view = UnityEngine.Object.Instantiate(gameObj) as GameObject;
                GameObject parent = GameSceneManager.GetNode(viewInfo.ParentName);
                if (parent == null)
                    DebugTool.LogError(string.Format("view name : {0}, parent node name : {1} not exit", viewName, viewInfo.ParentName));
                view.transform.SetParent(parent.transform, false);
                callback(view);
            }
        );
    }

    public static void LoadItem(string viewName, string objName, Action<GameObject> callback) {
        GameViewInfo viewInfo = GetViewInfo(viewName);
        AssetBundleManager.Load(viewInfo.AssetBundleName, objName, callback);
    }

    private static GameViewInfo GetViewInfo(string viewName) {
        if (!dicViewNameInfo.ContainsKey(viewName))
            DebugTool.LogError("ViewManager::Open not exit view : " + viewName);
        return dicViewNameInfo[viewName];
    }

    public static GameViewInfo GetViewInfo(GameMoudle moudle, GameView view) {
        string name = GameViewInfo.GetViewName(moudle, view);
        if (!dicViewNameInfo.ContainsKey(name))
            DebugTool.LogError("ViewManager::view info is not exit view name\t" + name);
        return dicViewNameInfo[name];
    }
}