using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectView:View {
    public SelectView(int sceneCount, GameMoudle moudle, GameView view, UIPrefab prefab):base(moudle, view, prefab)  {
        Transform parent = GetNode<Transform>("selectSceneContent");
        string itemName = "SelectSceneItem";
        LoadItem(itemName, parent, sceneCount, () => {
            
            for (int index = 0; index < sceneCount; index++) {
                UIPrefab itemPrefab = GetItem(itemName, index);
                itemPrefab.GetNode<Text>("textScene").text = string.Format("关卡 {0}", index + 1);
                itemPrefab.GetNode<Button>("btnOpenScene").onClick.AddListener(
                    () => {
                        OnClickOpenScene(index);
                    }
                );
            }
        });
    }

    private void OnClickOpenScene(int index) {
        DebugTool.Log("OnClickOpenScene " + index);
    }
}