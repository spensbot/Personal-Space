using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Input2D
{
    public Input2D(Vector2 v, float a)
    {
        vector = v;
        angle = a;
        magnitude = vector.magnitude;
    }
    public Vector2 vector;
    public float magnitude;
    public float angle;
}

public class InputManager : Singleton<InputManager>
{
    FixedJoystick leftJoystick;
    FixedJoystick rightJoystick;

    public Input2D Input { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        leftJoystick = GameObject.Find("Left Joystick").GetComponent<FixedJoystick>();
        rightJoystick = GameObject.Find("Right Joystick").GetComponent<FixedJoystick>();
        Input = new Input2D(Vector2.zero, 0f);
    }

    public void updateInput()
    {
        Vector2 leftInputVector = new Vector2(leftJoystick.Horizontal, leftJoystick.Vertical);
        Vector2 rightInputVector = new Vector2(rightJoystick.Horizontal, rightJoystick.Vertical);

        //The overall input vector is the sum of the two joystick inputs.
        Vector2 vector = leftInputVector + rightInputVector;
        //However, the overall input magnitude cannot be higher than 1.
        if (vector.magnitude > 1)
        {
            vector.Normalize();
        }

        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

        Input = new Input2D(vector, angle);
    }
}
