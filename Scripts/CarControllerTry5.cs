using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerTry5 : MonoBehaviour
{
    public double currentSpeed;
    public float speed;
    public float rightLeftSpeed;
    public float speedLimit;

    void FixedUpdate()
    {
        currentSpeed = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6;
        if (currentSpeed <= speedLimit)
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            this.gameObject.GetComponent<Rigidbody>().AddForce((transform.right)* Time.deltaTime* rightLeftSpeed);
        if (Input.GetKey(KeyCode.A))
            this.gameObject.GetComponent<Rigidbody>().AddForce(-(transform.right) * Time.deltaTime * rightLeftSpeed);
    }
}
