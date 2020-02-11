using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float delay;

    void Start()
    {
        Destroy(this.gameObject, delay);
    }
}
