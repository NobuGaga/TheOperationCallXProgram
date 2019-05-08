using UnityEngine;

public static class GameManager {
    private static LogicScript m_curLogicScript;


    public static void StartGame(LogicScript startScript) {
        if (startScript == null)
            return;
        m_curLogicScript = startScript;

        //GameObject.DontDestroyOnLoad(startScript.gameObject);

        GameConfig.Init();
        new Loading();

        AssetBundleLoader.Load(m_curLogicScript, "prefabs/moudle/login/mainview", "MainView",
            delegate (GameObject gameObj)
            {
                GameObject gbGreenCubeCopy = UnityEngine.Object.Instantiate(gameObj) as GameObject;
                gbGreenCubeCopy.transform.SetParent(m_curLogicScript.gameObject.transform, false);
                //gbGreenCubeCopy.transform.localScale = new Vector3(100, 100, 100);
            }
        );

        AssetBundleLoader.Load(m_curLogicScript, "prefabs/moudle/loading/mainview", "MainView",
            delegate (GameObject gameObj)
            {
                GameObject gbGreenCubeCopy = UnityEngine.Object.Instantiate(gameObj) as GameObject;
                gbGreenCubeCopy.transform.SetParent(m_curLogicScript.gameObject.transform, false);
                //gbGreenCubeCopy.transform.localScale = new Vector3(100, 100, 100);
            }
        );

        
    }

    public static void UpdateGame() {
            
    }
}
