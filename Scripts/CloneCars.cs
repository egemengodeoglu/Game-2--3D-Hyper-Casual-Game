using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneCars : PoolObject
{
    public string carname;
    public float speed;
    public float body;
    void Update()
    {
        transform.Rotate(0f, 1f, 0f);
    }
}
