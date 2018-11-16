using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    GameObject firepoint;

    int bulletCount; 
    // Use this for initialization
    void Start ()
    {
        firepoint = gameObject.transform.Find("Firepoint").gameObject;
        bulletCount = 20;
    }

    // Update is called once per frame
    void Update ()
    {
        CheckInput();
	}

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}
