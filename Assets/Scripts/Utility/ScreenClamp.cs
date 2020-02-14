using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenClamp : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void LateUpdate()
    {
        clampRigidBodyToScreenBounds(rb);
    }

    public void clampRigidBodyToScreenBounds(Rigidbody2D rb)
    {
        Rect playRect = ScreenManager.Instance.playRect;
        Vector2 newPosition;
        newPosition.x = Mathf.Clamp(rb.position.x, playRect.xMin, playRect.xMax);
        newPosition.y = Mathf.Clamp(rb.position.y, playRect.yMin, playRect.yMax);
        rb.position = newPosition;
    }
}
