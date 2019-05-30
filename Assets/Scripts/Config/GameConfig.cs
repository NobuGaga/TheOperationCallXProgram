using UnityEngine;

public static class GameConfig {
    private static bool isDebug = true;
    public static bool IsDebug => isDebug;
    public static readonly int StartGameLoadTime = 3; // unit:second
    public static readonly GameCameraType CameraType = GameCameraType.Fix;
    public static readonly int PlayerMoveFix = 1;
    public static readonly int CameraMoveFixModeTime = 30;
    public static readonly int CameraMoveThirdModeTime = 10;
    public static readonly float CameraHeightFix = 0.17f;
    public static readonly bool isDamageUseTextImage = true;

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

public enum GameCameraType {
    // 固定视角
    Fix,
    // 第一人称
    FirstPerson,
    // 第三人称
    ThirdPerson,
}