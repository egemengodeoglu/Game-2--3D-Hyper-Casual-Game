using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public Action<String, String> OnPlayerEvent;
    public float currentspeed;
    private float verticalInput = 2f;

    public WheelCollider frontRight, frontLeft;
    public WheelCollider backRight, backLeft;
    public Transform frontRightT, frontLeftT, backRightT, backLeftT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;

    void Start()
    {

        //Debug.Log("Name : "+ SelectedCarScript.Instance.selected.name);
    }


    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = transform.position;
        Quaternion _quat = transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        frontLeft.steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");
        frontRight.steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");
        frontLeft.motorTorque = verticalInput * motorForce;
        frontRight.motorTorque = verticalInput * motorForce;
        UpdateWheelPose(frontLeft, frontLeftT);
        UpdateWheelPose(frontRight, frontRightT);
        UpdateWheelPose(backLeft, backLeftT);
        UpdateWheelPose(backRight, backRightT);
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
