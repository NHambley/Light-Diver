using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /* 
    Buttons
    Square  = joystick button 0
    X       = joystick button 1
    Circle  = joystick button 2
    Triangle= joystick button 3
    L1      = joystick button 4
    R1      = joystick button 5
    L2      = joystick button 6
    R2      = joystick button 7
    Share	= joystick button 8
    Options = joystick button 9
    L3      = joystick button 10
    R3      = joystick button 11
    PS      = joystick button 12
    PadPress= joystick button 13

Axes:
    LeftStickX      = X-Axis
    LeftStickY      = Y-Axis (Inverted?)
    RightStickX     = 3rd Axis
    RightStickY     = 4th Axis (Inverted?)
    L2              = 5th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
    R2              = 6th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
    DPadX           = 7th Axis
    DPadY           = 8th Axis (Inverted?)
	*/

    // get right stick 
    float horizontal;
    float vertical;

    // get left stick
    float horizontalL;
    float verticalL;

    [SerializeField]
    float rotSpeed;

    [SerializeField, Range(1, 15)]
    float moveSpeed;

    Vector3 joystickPos;
    Vector3 position;
    

    void Start ()
    {
        rotSpeed = 200f;
        moveSpeed = 5f;
        joystickPos = new Vector3(horizontal, vertical, 0);
        position = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // because of funky joystick make sure input is actually occurring
        horizontal = Input.GetAxisRaw("RHorizontal");
        if (horizontal > -0.5f && horizontal < 0.5f)
            horizontal = 0;

        vertical = Input.GetAxisRaw("RVertical");
        if (vertical > -0.5f && vertical < 0.5f)
            vertical = 0;

        horizontalL = Input.GetAxisRaw("LHorizontal");
        verticalL = Input.GetAxisRaw("LVertical");
        joystickPos = new Vector3(horizontalL, verticalL, 0);

        CheckRotation();
        CheckMovement();
    }

    void CheckMovement()
    {
        // use joystick position as a movement vector
        position += new Vector3(horizontalL, verticalL, 0) * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    void CheckRotation()
    {
        transform.Rotate(new Vector3(0, 0, 1) * horizontal * Time.deltaTime * rotSpeed);
    }
}
