using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //send the bullet along the player's forward vector upon instantiation
    Vector2 position;
    [SerializeField]
    float speed;
    [SerializeField]
    float drag;
    GameObject player;
    Vector2 travel;

	// Use this for initialization
	void Start ()
    {
        // at start get player forward vector
        player = GameObject.FindGameObjectWithTag("Player");
        travel = -player.transform.right;
        position = transform.position;

        speed = 15f;
        drag = 0.5f;

	}
	
	// Update is called once per frame
	void Update ()
    {
        position += travel * speed * Time.deltaTime;
        
        transform.position = position;
	}
}
