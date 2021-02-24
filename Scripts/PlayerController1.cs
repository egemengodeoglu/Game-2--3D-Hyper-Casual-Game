using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float MotorForce;
    public WheelCollider WheelColFR;
    public WheelCollider WheelColFL;
    public WheelCollider WheelColRR;
    public WheelCollider WheelColRL;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical") * MotorForce;

        WheelColRL.motorTorque = v;
        WheelColRR.motorTorque = v;
    }
}
