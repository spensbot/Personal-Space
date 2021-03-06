﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A custom input structure for this game.
//Takes and input vector and normalizes it if greater than 1.
//Also generates angle from right.
public struct Input2D
{
    public Input2D(Vector2 v)
    {
        vector = v;
        if (vector.magnitude > 1)
        {
            vector.Normalize();
        }
        angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        magnitude = vector.magnitude;
    }
    public Vector2 vector;
    public float magnitude;
    public float angle;
}

public class InputManager : Singleton<InputManager>
{
    //FixedJoystick joystick;

    [SerializeField] DynamicJoystick joystick;

    public Input2D Input { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Input = new Input2D(Vector2.zero);
    }

    public void updateInput()
    {
        Vector2 inputVector = new Vector2(joystick.Horizontal, joystick.Vertical);

        Input = new Input2D(inputVector);
    }
}
