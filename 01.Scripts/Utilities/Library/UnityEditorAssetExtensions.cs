#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Parkjung2016.Library
{
    public static class UnityEditorAssetExtensions
    {
        /// <summary>
        /// 폴더 에셋으로부터 Assets로 시작하는 로컬 경로 얻기
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetLocalPath(this DefaultAsset @this)
        {
            bool success =
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(@this, out string guid, out long _);
            if (success) return AssetDatabase.GUIDToAssetPath(guid);
            else return null;
        }
        /// <summary>
        /// 폴더 에셋으로부터 절대 경로 얻기
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetAbsolutePath(this DefaultAsset @this)
        {
            string path = GetLocalPath(@this);
            if (path == null) return null;
            path = path.Substring(path.IndexOf('/') + 1);
            return Application.dataPath + "/" + path;
        }
/// <summary>
/// 폴더 에셋으로부터 DirectoryInfo 객체 얻기
/// </summary>
/// <param name="this"></param>
/// <returns></returns>
        public static DirectoryInfo GetDirectoryInfo(this DefaultAsset @this)
        {
            string absPath = GetAbsolutePath(@this);
            return (absPath != null) ? new DirectoryInfo(absPath) : null;
        }
    }
}
#endif