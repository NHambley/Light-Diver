using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = this.player.transform.position;
        playerPos.z = this.transform.position.z;
        this.transform.position = playerPos;
	}
}
