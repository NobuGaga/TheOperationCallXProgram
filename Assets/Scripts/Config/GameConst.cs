public static class GameConst {
    public static readonly string PlatformName =
#if UNITY_EDITOR
    "Windows";
#elif UNITY_ANDROID
    "Android";
#elif UNITY_IPHONE
    "iOS";
#elif UNITY_STANDALONE_WIN
    "Windows";
#else
    string.Empty;
#endif

    public static readonly string GameCamera = "GameCamera";
    public static readonly string PlayerCamera = "PlayerCamera";
}