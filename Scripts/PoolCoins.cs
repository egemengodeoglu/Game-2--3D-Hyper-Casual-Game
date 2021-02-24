using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PoolCoins : PoolObject
{
    public float speed;
    public float target_x, target_y, target_z;
    private List<Vector3> targetList;
    private int current = 1;
    



    private void OnEnable()
    {
        targetList = new List<Vector3>();
        targetList.Add(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z));
        targetList.Add(new Vector3(transform.localPosition.x + target_x, transform.localPosition.y + target_y, transform.localPosition.z + target_z));

        
    }


    void Update()
    {
        transform.Rotate(0f, 0f, 1f);
        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetList[current], step);
        if (Vector3.Distance(targetList[current], transform.localPosition) < 0.001f)
        {
            if (current == 0)
            {
                current = 1;
            }
            else
            {
                current = 0;
            }
        }

    }

}
