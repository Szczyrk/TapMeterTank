using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTank : MonoBehaviour {

    GameObject myTank;

    void SwapPrefabs(GameObject oldGameObject)
    {
        // Determine the rotation and position values of the old game object.
        // Replace rotation with Quaternion.identity if you do not wish to keep rotation.
        Quaternion rotation = oldGameObject.transform.rotation;
        Vector3 position = oldGameObject.transform.position;

        // Instantiate the new game object at the old game objects position and rotation.
        GameObject newGameObject = Instantiate(myTank, position, rotation);

        // If the old game object has a valid parent transform,
        // (You can remove this entire if statement if you do not wish to ensure your
        // new game object does not keep the parent of the old game object.
        if (oldGameObject.transform.parent != null)
        {
            // Set the new game object parent as the old game objects parent.
            newGameObject.transform.SetParent(oldGameObject.transform.parent);
        }
        newGameObject.tag = "Player";
        // Destroy the old game object, immediately, so it takes effect in the editor.
        DestroyImmediate(oldGameObject);
        DestroyImmediate(myTank);
    }



	// Use this for initialization
	void Awake () {
        if (GameObject.FindGameObjectWithTag("Choose"))
        {
            myTank = GameObject.FindGameObjectWithTag("Choose");
            SwapPrefabs(GameObject.FindGameObjectWithTag("Player").transform.gameObject);
        }
    }
	
}
