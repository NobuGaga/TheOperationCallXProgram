using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectView:View {
    public SelectView(GameMoudle moudle, GameView view, UIPrefab prefab) : base(moudle, view, prefab) { }

    public void ShowSelectScene(Dictionary<int, GameScene> dicScene) {
        string itemName = "SelectSceneItem";
        Transform parent = GetNode<Transform>("selectSceneContent");
        int sceneCount = dicScene.Count;
        int index = 1;
        LoadItem(itemName, parent, sceneCount, (UIPrefab itemPrefab) => {
            itemPrefab.GetNode<Text>("textScene").text = GameSceneInfo.GetName(dicScene[index]);
            itemPrefab.GetNode<Text>("textBtnOpen").text = CsvTool.Text("common_open");
            int indexCopy = index;
            itemPrefab.GetNode<Button>("btnOpenScene").onClick.AddListener(
                () =>
                    EventManager.Dispatch(GameMoudle.Select, GameEvent.Type.Click, indexCopy)
            );
            index++;
        });
    }
}