using System.IO;
using UnityEditor;

namespace Kugushev.Editor
{
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            const string assetBundleDirectory = "Assets/Kugushev/AssetBundles";
            if (!Directory.Exists(assetBundleDirectory))
                Directory.CreateDirectory(assetBundleDirectory);
            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows);
        }
    }
}