using System.IO;

public static class StringTool {
   public static string RemoveExtension(string originString) {
        return originString.Replace(Path.GetExtension(originString), string.Empty);
    }
}