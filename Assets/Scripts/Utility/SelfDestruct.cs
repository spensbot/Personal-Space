using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : Trackable
{
    [SerializeField] float delay;

    protected override void Start()
    {
        Destroy(this.gameObject, delay);
    }
}
