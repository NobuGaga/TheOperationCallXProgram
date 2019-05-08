using UnityEditor;

public static class EditorConst {
    public static string PlatformName() {
        switch (EditorUserBuildSettings.activeBuildTarget) {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            default:
                return "UnusePlatform";
        }
    }

    public static readonly string MoudleString = "Moudle";
    public static readonly string UIString = "UI";
    public static readonly int UIStringLength = UIString.Length;
}