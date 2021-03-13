using System;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Unit.Utils
{
    internal static class AssetBundleHelper
    {
        private const string AssetBundlesFolder = "Assets/Kugushev/AssetBundles/";
        private const string TestsExecutionBundle = AssetBundlesFolder + "testsexecution";

        private static readonly Lazy<AssetBundle> AssetBundle = new Lazy<AssetBundle>(LoadAssetBundle, true);

        public static T LoadAsset<T>(string name) where T : UnityEngine.Object
        {
            var result = AssetBundle.Value.LoadAsset<T>(name);
            if (ReferenceEquals(result, null))
                throw new Exception($"Unable to load {name}");

            return result;
        }

        private static AssetBundle LoadAssetBundle()
        {
            var bundle = UnityEngine.AssetBundle.LoadFromFile(TestsExecutionBundle);
            if (bundle == null)
                throw new Exception("Bundle is null");
            return bundle;
        }
    }
}