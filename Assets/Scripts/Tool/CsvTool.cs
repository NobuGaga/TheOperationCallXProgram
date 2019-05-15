using System.Collections.Generic;

public static class CsvTool {
    private static readonly Dictionary<string, string> m_dicText = new Dictionary<string, string>() {
        { "common_open", "进入" }
    };

    public static string Text(string key) {
        if (m_dicText.ContainsKey(key))
            return m_dicText[key];
        return string.Empty;
    }
}