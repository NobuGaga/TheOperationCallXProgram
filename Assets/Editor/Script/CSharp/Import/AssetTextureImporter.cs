﻿using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class AssetTextureImporter {
    private static List<string> deleteTextureList = new List<string>();
    private static Texture2D alphaTestTexture = new Texture2D(1024, 1024, TextureFormat.BGRA32, false);

    public static void OnPreprocessTexture(string assetPath) {
        bool isNotResourceTexture = !assetPath.Contains(EditorPathConfig.ResourcesPath);
        if (!assetPath.Contains(EditorPathConfig.AtlasPath) && isNotResourceTexture) {
            deleteTextureList.Add(assetPath);
            return;
        }
        TextureImporter import = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        import.mipmapEnabled = false;
        bool isAlpha = CheckTextureAlpha(assetPath);
        SetAndroidTextureFormat(import, isAlpha);
        SetiOSTextureFormat(import, isAlpha);
        SetWindowsTextureFormat(import, isAlpha);
        if (isNotResourceTexture)
            SetAssetBundleName(import, assetPath);
    }

    private static void SetAssetBundleName(TextureImporter import, string assetPath) {
        Regex filter = new Regex("Atlas/.+/");
        Match match = filter.Match(assetPath);
        if (match.Success) {
            string originPath = match.ToString();
            string[] temp = originPath.Split('/');
            import.assetBundleName = temp[1]; // assetBundleName 会自动转成全小写
        }
    }

    private static bool CheckTextureAlpha(string assetPath) {
        alphaTestTexture.LoadImage(File.ReadAllBytes(assetPath));
        foreach (Color rgba in alphaTestTexture.GetPixels())
            if (rgba.a < 0.98f)
                return true;
        return false;
    }

    private static void SetAndroidTextureFormat(TextureImporter import, bool isAlpha) {
        TextureImporterFormat textureFormat = TextureImporterFormat.ASTC_RGBA_4x4;
        int compressionQuality = 50;
        if (!isAlpha)
            textureFormat = TextureImporterFormat.ASTC_RGB_4x4;
        TextureImporterPlatformSettings andFormat = import.GetPlatformTextureSettings("Android");
        if (!andFormat.overridden || andFormat.format != textureFormat || andFormat.compressionQuality != compressionQuality) {
            andFormat.overridden = true;
            andFormat.format = textureFormat;
            andFormat.compressionQuality = compressionQuality;
            import.SetPlatformTextureSettings(andFormat);
        }
    }

    private static void SetiOSTextureFormat(TextureImporter import, bool isAlpha) {
        TextureImporterFormat textureFormat = TextureImporterFormat.ASTC_RGBA_4x4;
        int compressionQuality = 50;
        if (!isAlpha)
            textureFormat = TextureImporterFormat.ASTC_RGB_4x4;
        TextureImporterPlatformSettings iosFormat = import.GetPlatformTextureSettings("iPhone");
        if (!iosFormat.overridden || iosFormat.format != textureFormat || iosFormat.compressionQuality != compressionQuality) {
            iosFormat.overridden = true;
            iosFormat.format = textureFormat;
            iosFormat.compressionQuality = compressionQuality;
            import.SetPlatformTextureSettings(iosFormat);
        }
    }

    private static void SetWindowsTextureFormat(TextureImporter import, bool isAlpha) {
        TextureImporterFormat textureFormat = TextureImporterFormat.ASTC_RGBA_4x4;
        int compressionQuality = 50;
        if (!isAlpha)
            textureFormat = TextureImporterFormat.ASTC_RGB_4x4;
        TextureImporterPlatformSettings standFormat = import.GetPlatformTextureSettings("Standalone");
        textureFormat = TextureImporterFormat.DXT5;
        if (!isAlpha)
            textureFormat = TextureImporterFormat.DXT1;
        if (!standFormat.overridden || standFormat.format != textureFormat) {
            standFormat.overridden = true;
            standFormat.format = textureFormat;
            standFormat.compressionQuality = compressionQuality;
            import.SetPlatformTextureSettings(standFormat);
        }
    }

    public static void OnPostprocessAllAssets() {
        if (deleteTextureList.Count == 0)
            return;
        DebugTool.LogError(string.Format("导入图片资源不在 {0} 路径下", EditorPathConfig.AtlasPath));
        foreach (string assetPath in deleteTextureList) {
            File.Delete(assetPath);
            // 手动删除 meta 文件, Unity 控制台强制刷新删除 meta 时会报错
            string metaPath = string.Format("{0}.meta", assetPath);
            File.Delete(metaPath);
        }
        deleteTextureList.Clear();
    }
}