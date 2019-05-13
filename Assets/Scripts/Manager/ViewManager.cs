using UnityEngine;
using System;
using System.Collections.Generic;

public static class ViewManager {
    public static Transform RootNode => GameManager.LogicScript.gameObject.transform;

    private static Dictionary<string, GameViewInfo> m_dicViewNameInfo = new Dictionary<string, GameViewInfo>();

    public static void Init() {
        GameViewInfo info = new GameViewInfo(GameMoudle.Loading, GameView.MainView);
        m_dicViewNameInfo.Add(info.Name, info);
    }

    public static void Open(string viewName, Action<GameObject> callback) {
        if (!m_dicViewNameInfo.ContainsKey(viewName)) {
            DebugTool.LogError("ViewManager::Open not exit view : " + viewName);
            return;
        }
        GameViewInfo viewInfo = m_dicViewNameInfo[viewName];
        AssetBundleManager.Load(viewInfo.AssetBundleName, viewInfo.Name,
            delegate (GameObject gameObj) {
                GameObject view = UnityEngine.Object.Instantiate(gameObj) as GameObject;
                view.transform.SetParent(RootNode, false);
                callback(view);
            }
        );
    }
}