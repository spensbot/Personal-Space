using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trackable : MonoBehaviour
{
    private static List<GameObject> instances;

    protected virtual void Start()
    {
        if (instances == null)
        {
            instances = new List<GameObject>();
        }
        instances.Add(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        instances.Remove(this.gameObject);
    }

    public static void DestroyAll()
    {
        if (instances != null)
        {
            foreach (GameObject instance in instances)
            {
                Destroy(instance);
            }
            instances.Clear();
        }
    }
}
