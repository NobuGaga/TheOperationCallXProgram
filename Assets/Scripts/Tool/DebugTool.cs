using UnityEngine;

public static class DebugTool {
    private enum LogLevel {
        Normal,
        Warning,
        Error,
    }

    private static void Log(LogLevel level, object message, Object context = null) {
        if (!GameConfig.IsDebug)
            return;
        switch (level) {
            case LogLevel.Normal:
                Debug.Log(message, context);
                break;
            case LogLevel.Warning:
                Debug.LogWarning(message, context);
                break;
            case LogLevel.Error:
                Debug.LogError(message, context);
                break;
        }
    }

    public static void Log(object message, Object context = null) {
        Log(LogLevel.Normal, message, context);
    }
    public static void LogWarning(object message, Object context = null) {
        Log(LogLevel.Warning, message, context);
    }
    public static void LogError(object message, Object context = null) {
        Log(LogLevel.Error, message, context);
    }

    private static bool isTestLastTime = false;
    private static System.Action testLastTimeFunction;
    public static void StartTestLastTime() {
        if (isTestLastTime)
            return;
        long lastTime = 0;
        testLastTimeFunction = () => {
            if (isTestLastTime) {
                long nowTime = System.DateTime.Now.Ticks;
                Log("DebugTool Test Time Now " + nowTime);
                Log("DebugTool Last Time " + (nowTime - lastTime).ToString());
            }
            else {
                lastTime = System.DateTime.Now.Ticks;
                Log("DebugTool Test Time Now " + lastTime);
            }
        };
        testLastTimeFunction();
        isTestLastTime = true;
    }
    public static void EndTestLastTime() {
        testLastTimeFunction();
        isTestLastTime = false;
    }
}
