using ExtendedBehaviourSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Create_SO_effect
{
    [MenuItem("Assets/Create/ExtendedBehaviourSystem/SO_effect")]
    public static void CreateAsset()
    {
        SO_effect asset = ScriptableObject.CreateInstance<SO_effect>();

        AssetDatabase.CreateAsset(asset, ExtendedBehaviourSystem.Helper.GetPathForAsset() + "/New_SO_effect.asset");
        AssetDatabase.SaveAssets();

        Selection.activeObject = asset;
    }
}
