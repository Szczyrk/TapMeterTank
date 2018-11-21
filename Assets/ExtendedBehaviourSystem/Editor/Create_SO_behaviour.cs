using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ExtendedBehaviourSystem;

public class Create_SO_behaviour
{
    [MenuItem("Assets/Create/ExtendedBehaviourSystem/SO_behaviour")]
    public static void CreateAsset()
    {
        SO_behaviour asset = ScriptableObject.CreateInstance<SO_behaviour>();

        AssetDatabase.CreateAsset(asset, ExtendedBehaviourSystem.Helper.GetPathForAsset() + "/New_SO_behaviour.asset");
        AssetDatabase.SaveAssets();

        Selection.activeObject = asset;
    }
}
