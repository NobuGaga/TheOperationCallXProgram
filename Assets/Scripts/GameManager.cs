using UnityEngine;

public static class GameManager {
    private static MonoBehaviour m_curLogicScript;

    public static void StartGame(MonoBehaviour startScript) {
        if (startScript == null)
            return;
        m_curLogicScript = startScript;

        //GameObject loadGreenCubeCopy = Resources.Load<GameObject>("Prefabs/GreenCube");
        //GameObject gbGreenCubeCopy = Object.Instantiate(loadGreenCubeCopy) as GameObject;
        //gbGreenCubeCopy.transform.SetParent(m_curLogicScript.gameObject.transform, false);

        AssetBundleLoader.Load<GameObject>(m_curLogicScript, "prefabs/greencube", "GreenCube", 
            delegate (GameObject gameObj) {
                GameObject gbGreenCubeCopy = Object.Instantiate(gameObj) as GameObject;
                gbGreenCubeCopy.transform.SetParent(m_curLogicScript.gameObject.transform, false);
                gbGreenCubeCopy.transform.localScale = new Vector3(100, 100, 100);
            }
        );
    }

    public static void UpdateGame() {
            
    }
}
