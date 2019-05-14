using System.Collections.Generic;

public class MSelectData : Model {
    public int SceneCount => dicSceneIndex.Count;
    Dictionary<int, GameScene> dicSceneIndex = new Dictionary<int, GameScene>() {
        {1, GameScene.Forest},
        {2, GameScene.Desert},
        {3, GameScene.City}
    };

    public override void Init() {
        

    }

    public override void Dispose() {

    }
}