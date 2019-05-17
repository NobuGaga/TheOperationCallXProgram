using System.Collections.Generic;

public class MSelectData:Model {
    private readonly Dictionary<int, GameScene> m_dicSceneIndex = new Dictionary<int, GameScene>() {
        { 1, GameScene.Blade_Warrior_Demo },
        { 2, GameScene.DesertScene },
    };
    public Dictionary<int, GameScene> Scenes => m_dicSceneIndex;

    public override void Init() {
           
    }

    public GameScene GetScene(int index) {
        if (m_dicSceneIndex.ContainsKey(index))
            return m_dicSceneIndex[index];
        return GameScene.StartScene;
    }

    public override void Dispose() {

    }
}