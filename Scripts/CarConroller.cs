using System;
using System.Collections.Generic;
using UnityEngine;

public class CarConroller : MonoBehaviour
{
    public Action<String, String> OnPlayerEvent;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }



    public void OnTriggerEnter(Collider others)
    {
        if (others.gameObject.GetComponent<ObjectType>().value == 2)
        {
            PoolManager.Instance.NotUsedObject(others.gameObject.GetComponent<PoolObject>());
            OnPlayerEvent.Invoke("Coin", "");
        }
        else if (others.gameObject.GetComponent<ObjectType>().value == 3)
        {
            if (OnPlayerEvent != null)
            {
                OnPlayerEvent.Invoke("Finish", "");
            }
        }
        else if (others.gameObject.GetComponent<ObjectType>().value == 4)
        {
            if (OnPlayerEvent != null)
            {
                OnPlayerEvent.Invoke("Die", "");
            }
        }
        else if (others.gameObject.GetComponent<ObjectType>().value == 5)
        {
            //rb.AddForce(new Vector3(0.0f, 0.0f, (3.0f) * boostSpeed));
        }
        else if (others.gameObject.GetComponent<ObjectType>().value == 6)
        {
            OnPlayerEvent.Invoke("Recycle", transform.position.z.ToString());
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (OnPlayerEvent != null)
        {
            if (collision.gameObject.GetComponent<ObjectType>().value == 0)
            {
                OnPlayerEvent.Invoke("Die", "");
            }
            else if (collision.gameObject.GetComponent<ObjectType>().value == 2)
            {
                Destroy(collision.gameObject);
                OnPlayerEvent.Invoke("Coin", "");
            }
            else if (collision.gameObject.GetComponent<ObjectType>().value == 99)
            {
                Destroy(collision.gameObject);
                
            }
        }

    }

}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}

