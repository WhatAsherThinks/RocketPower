using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //SerializeField makes it avaialble to Unity
    [SerializeField] float rcsThrust = 100f; //rcs = reaction control system
    [SerializeField] float mainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource boosterSound;
    bool soundOn;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        boosterSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("ok");
                break;
            default:
                print("dead");
                break;
        }
        print("collided");
    }

    private void Rotate()
    {
        //freeseRotation controls whether Unity's physics will control rotation
        rigidBody.freezeRotation = true; //turns on manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
            print("Rotating Left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
            print("Rotating Right");
        }

        rigidBody.freezeRotation = false; // turns off manual control of rotation

    }

    private void Thrust()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * rotationThisFrame);
            //To make sure the sound doesnt layer. 
            if (!boosterSound.isPlaying)
            {
                boosterSound.Play();
            }
            print("space pressed");
        }
        else
        {
            boosterSound.Stop();
        }
    }
}
