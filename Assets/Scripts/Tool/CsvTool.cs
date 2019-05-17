using System.Collections.Generic;

public static class CsvTool {
    private static readonly Dictionary<string, string> dicText = new Dictionary<string, string>() {
        { "common_open", "进入" }
    };

    public static string Text(string key) {
        if (dicText.ContainsKey(key))
            return dicText[key];
        return string.Empty;
    }
}