using UnityEngine;
using System;
using System.IO;

public static class GameSetting {

    public static void Init() {
#if UNITY_IPHONE
        UnityEngine.iOS.Device.SetNoBackupFlag(PathConfig.PersistentDataPath);
#endif
        if (!Directory.Exists(PathConfig.PersistentDataPath))
            Directory.CreateDirectory(PathConfig.PersistentDataPath);
        DebugTool.Log(PathConfig.PersistentDataPath);

        GameConfig.Init();
    }

    private static void checkResolution() {
        int setWidth = Screen.width;
        int setHeight = Screen.height;
        float per = Math.Min(750f / setWidth, 1334f / setHeight);
        if (per < 1) {
            setWidth = Mathf.RoundToInt(setWidth * per);
            setWidth = setWidth % 2 == 0 ? setWidth : setWidth + 1;
            setHeight = Mathf.RoundToInt(setHeight * per);
            setHeight = setHeight % 2 == 0 ? setHeight : setHeight + 1;
            Screen.SetResolution(setWidth, setHeight, true);
        }
    }

    private static void checkRotation() {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
#endif
    }
}
