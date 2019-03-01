using UnityEditor;

public static class AssetBundlePacker{
    private static string PlatformName() {
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
                return string.Empty;
        }
    }

    public static void Pack() {
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        string path = string.Format("Packages/{0}/", PlatformName());
    }
}
