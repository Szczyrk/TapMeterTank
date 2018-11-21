using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ExtendedBehaviourSystem;

public class Create_SO_process
{
    [MenuItem("Assets/Create/ExtendedBehaviourSystem/SO_process")]
    public static void CreateAsset()
    {
        SO_process asset = ScriptableObject.CreateInstance<SO_process>();

        AssetDatabase.CreateAsset(asset, ExtendedBehaviourSystem.Helper.GetPathForAsset() + "/New_SO_process.asset");
        AssetDatabase.SaveAssets();

        Selection.activeObject = asset;
    }
}
