using UnityEngine;

public static class GameManager {
    private static MonoBehaviour m_curLogicScript;


    public static void StartGame(MonoBehaviour startScript) {
        if (startScript == null)
            return;
        m_curLogicScript = startScript;

        //GameObject.DontDestroyOnLoad(startScript.gameObject);

        GameConfig.Init();
        new Loading();

        AssetBundleLoader.Load(m_curLogicScript, "prefabs/greencube", "GreenCube",
            delegate (GameObject gameObj) {
                GameObject gbGreenCubeCopy = UnityEngine.Object.Instantiate(gameObj) as GameObject;
                gbGreenCubeCopy.transform.SetParent(m_curLogicScript.gameObject.transform, false);
                gbGreenCubeCopy.transform.localScale = new Vector3(100, 100, 100);
            }
        );
    }

    public static void UpdateGame() {
            
    }
}
