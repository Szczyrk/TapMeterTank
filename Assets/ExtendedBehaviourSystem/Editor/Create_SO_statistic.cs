using ExtendedBehaviourSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Create_SO_statistic
{
    [MenuItem("Assets/Create/ExtendedBehaviourSystem/SO_statistic")]
    public static void CreateAsset()
    {
        SO_statistic asset = ScriptableObject.CreateInstance<SO_statistic>();

        AssetDatabase.CreateAsset(asset, ExtendedBehaviourSystem.Helper.GetPathForAsset() + "/New_SO_statistic.asset");
        AssetDatabase.SaveAssets();

        Selection.activeObject = asset;
    }
}
