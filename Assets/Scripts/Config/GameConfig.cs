using UnityEngine;

public static class GameConfig {

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
