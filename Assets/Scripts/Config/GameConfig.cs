using UnityEngine;

public static class GameConfig {
    private static bool m_isDebug = true;
    public static bool IsDebug => m_isDebug;
    public static readonly int StartGameLoadTime = 3; // unit:second

    public static void Init() {
        // TODO
        // read json
        // save data (write file)
        // set game data
        QualitySettings.SetQualityLevel(1);
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.runInBackground = true;
#if UNITY_STANDALONE_WIN
        Screen.fullScreen = false;
#endif
    }
}
