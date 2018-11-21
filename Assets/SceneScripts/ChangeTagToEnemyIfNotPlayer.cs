using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTagToEnemyIfNotPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
    if (this.tag != "Player")
        this.tag = "Enemy";
}
	
	// Update is called once per frame
	void Update () {
		
	}
}
