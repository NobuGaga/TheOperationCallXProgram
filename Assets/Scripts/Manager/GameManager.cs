using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public static class GameManager {
    private static MonoBehaviour m_curLogicScript;

    public static void StartGame(MonoBehaviour startScript) {
        if (startScript == null)
            return;
        m_curLogicScript = startScript;

        AssetBundleLoader.Load(m_curLogicScript, "prefabs/greencube", "GreenCube", 
            delegate (GameObject gameObj) {
                GameObject gbGreenCubeCopy = UnityEngine.Object.Instantiate(gameObj) as GameObject;
                gbGreenCubeCopy.transform.SetParent(m_curLogicScript.gameObject.transform, false);
                gbGreenCubeCopy.transform.localScale = new Vector3(100, 100, 100);
            }
        );

        //Type logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        //MethodInfo[] logEntriesMethods = logEntries.GetMethods(BindingFlags.Static | BindingFlags.Public);
        //foreach (MethodInfo method in logEntriesMethods)
        //    Debug.Log(method.Name);
        //var clearMethod = LogEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        //clearMethod.Invoke(null, null)
    }

    public static void UpdateGame() {
            
    }
}
