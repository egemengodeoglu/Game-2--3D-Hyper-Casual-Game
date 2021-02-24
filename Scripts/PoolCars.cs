using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCars : PoolObject
{
    public string carName;
    public float carSpeed, carBody, speed;
    public float target_x, target_y, target_z;
    private Vector3 targetPosition;
    public GameObject[] wheels;

    private void OnEnable()
    {
        targetPosition = new Vector3(transform.localPosition.x + target_x, transform.localPosition.y + target_y, transform.localPosition.z + target_z);
    }

    void Update()
    {
        if (speed > 0)
        {
            float step = speed * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, step);
            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(2f, 0f, 0f);
            }
        }
        else
        {
            transform.Rotate(0f, 1f, 0f);
        }
        
    }
}
