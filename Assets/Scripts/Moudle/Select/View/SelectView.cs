using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectView:View {
    public SelectView(GameMoudle moudle, GameView view, UIPrefab prefab) : base(moudle, view, prefab) { }

    public void ShowSelectScene(Dictionary<int, GameScene> dicScene) {
        string itemName = "SelectSceneItem";
        Transform parent = GetNode<Transform>("selectSceneContent");
        int sceneCount = dicScene.Count;
        LoadItem(itemName, parent, sceneCount, () => {
            for (int index = 0; index < sceneCount; index++) {
                UIPrefab itemPrefab = GetItem(itemName, index);
                int indexCopy = index + 1;
                itemPrefab.GetNode<Text>("textScene").text = GameSceneInfo.GetName(dicScene[indexCopy]);
                itemPrefab.GetNode<Text>("textBtnOpen").text = CsvTool.Text("common_open");
                itemPrefab.GetNode<Button>("btnOpenScene").onClick.AddListener(
                    () =>
                        EventManager.Dispatch(GameMoudle.Select, GameEvent.Type.Click, indexCopy)
                );
            }
        });
    }
}