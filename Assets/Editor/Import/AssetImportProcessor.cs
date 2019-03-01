using UnityEditor;
using UnityEngine;

public class AssetImportProcessor : AssetPostprocessor {
    /// <summary>
    /// 模型导入之前调用
    /// </summary>
    public void OnPreprocessModel() {
        Debug.Log("OnPreprocessModel = " + assetPath);
    }
    /// <summary>
    /// 模型导入之后调用
    /// </summary>
    public void OnPostprocessModel(GameObject go) {
        Debug.Log("OnPostprocessModel = " + go.name);
    }

    /// <summary>
    /// 纹理导入之前调用，针对入到的纹理进行设置
    /// </summary>
    public void OnPreprocessTexture() {
        Debug.Log("OnPreprocessTexture = " + assetPath);
        AssetTextureImporter.OnPreprocessTexture(assetPath);
    }
    public void OnPostprocessTexture(Texture2D texture) {
        Debug.Log("OnPostProcessTexture = " + assetPath);
    }

    public void OnPreprocessAudio() {
        AudioImporter audio = assetImporter as AudioImporter;
        //audio.format = AudioImporterFormat.Compressed;
    }
    public void OnPostprocessAudio(AudioClip clip) {

    }

    /// <summary>
    /// 所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的
    /// </summary>
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        Debug.Log("OnPostprocessAllAssets");
        foreach (string str in importedAssets)
            Debug.Log("importedAsset = " + str);
        foreach (string str in deletedAssets)
            Debug.Log("deletedAssets = " + str);
        foreach (string str in movedAssets)
            Debug.Log("movedAssets = " + str);
        foreach (string str in movedFromAssetPaths)
            Debug.Log("movedFromAssetPaths = " + str);
        AssetTextureImporter.OnPostprocessAllAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }
}
