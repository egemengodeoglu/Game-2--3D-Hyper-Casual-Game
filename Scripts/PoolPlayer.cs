using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wheel : System.Object
{
    public Transform wheel;
    public bool isTurn = false;
}

public class PoolPlayer : PoolObject
{
    public Action<string, int> OnPlayerEvent;
    public double currentSpeed;
    public float speed, speedLimit, turnSpeed;
    private float horizontal;
    public List<Wheel> wheels;
    Animator anim;
    public ParticleSystem crashEffects;
    public ParticleSystem egzoz;


    public void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = GameDataReferences.Instance.effectsVolume;
        crashEffects.gameObject.GetComponent<AudioSource>().volume = GameDataReferences.Instance.effectsVolume;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        currentSpeed = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6;
        if (currentSpeed <= speedLimit)
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed * Time.deltaTime);
        horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * Time.deltaTime * Input.GetAxis("Horizontal") * turnSpeed);
            anim.SetInteger("TurnInt", (int)(horizontal*10));
        }
        turnWheel();
    }

    public void turnWheel()
    {
        foreach(Wheel wheel in wheels)
        {
            wheel.wheel.Rotate(2f, 0, 0);
        }
        
    }


    public void OnTriggerEnter(Collider others)
    {
        if (enabled)
        {
            if (others.gameObject.GetComponent<ObjectType>().value == 2)
            {
                PoolManager.Instance.NotUsedObject(others.gameObject.GetComponent<PoolObject>());
                OnPlayerEvent.Invoke("Coin", 0);
            }
            else if (others.gameObject.GetComponent<ObjectType>().value == 3)
            {
                if (OnPlayerEvent != null)
                {
                    OnPlayerEvent.Invoke("Finish", 0);
                }
            }
            else if (others.gameObject.GetComponent<ObjectType>().value == 4)
            {
                if (OnPlayerEvent != null)
                {
                    OnPlayerEvent.Invoke("Die", 0);
                }
            }
            else if (others.gameObject.GetComponent<ObjectType>().value == 5)
            {
                //rb.AddForce(new Vector3(0.0f, 0.0f, (3.0f) * boostSpeed));
            }
            else if (others.gameObject.GetComponent<ObjectType>().value == 6)
            {
                OnPlayerEvent.Invoke("Recycle", (int)((transform.position.z+30) / 120));
                PoolManager.Instance.NotUsedObject(others.gameObject.GetComponent<PoolRepeater>());
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (enabled)
        {
            if (OnPlayerEvent != null)
            {
                if (collision.gameObject.GetComponent<ObjectType>().value == 0)
                {
                    OnPlayerEvent.Invoke("Die", 0);
                }
                else if (collision.gameObject.GetComponent<ObjectType>().value == 2)
                {
                    Destroy(collision.gameObject);
                    OnPlayerEvent.Invoke("Coin", 0);
                }
                else if (collision.gameObject.GetComponent<ObjectType>().value == 7)
                {
                    Instantiate(crashEffects, collision.contacts[0].point, Quaternion.identity); 
                    //Instantiate(crashSound,collision.contacts[0].point, Quaternion.identity);
                    OnPlayerEvent.Invoke("Coin", 0);
                }
                else if (collision.gameObject.GetComponent<ObjectType>().value == 99)
                {
                    Destroy(collision.gameObject);
                }
            }
        }

    }
}
