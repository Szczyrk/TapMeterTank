using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ExtendedBehaviourSystem
{
    public static class Helper
    {
        /// <summary>
        /// Used to create a SO at selected path (by right-clicking on a folder)
        /// </summary>
        /// <returns>Path where to create an asset.</returns>
        public static string GetPathForAsset()
        {
            // get path of selection in assets
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            // if no selection
            if (path == "")
            {
                path = "Assets";
            }
            // if not a folder
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(path), "");
            }

            return path;
        }
    }
}
