using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarControllerTry3 : MonoBehaviour
{
    public double currentSpeed;
    public float speed;
    public float retateSpeed;
    public float speedLimit;
    
    void FixedUpdate()
    {
        currentSpeed = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6;
        if (currentSpeed <= speedLimit)
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.S))
            this.gameObject.GetComponent<Rigidbody>().AddForce(-(transform.forward) * retateSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
            this.gameObject.transform.Rotate(0, retateSpeed, 0);
            if (Input.GetKey(KeyCode.A))
            this.gameObject.transform.Rotate(0, -retateSpeed, 0);
    }
}
