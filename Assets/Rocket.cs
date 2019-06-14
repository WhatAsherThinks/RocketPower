using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //SerializeField makes it avaialble to Unity
    [SerializeField] float rcsThrust = 100f; //rcs = reaction control system
    [SerializeField] float mainThrust = 150f;

    Rigidbody rigidBody;
    AudioSource boosterSound;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        boosterSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("ok");
                break;
            case "Finish":
                print("win");
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); //parameterize time
                break;
            default:
                print("dead");
                state = State.Dying;
                Invoke("LoadLevel1", 1f);
                break;
        }
        print("collided");
    }

    private void LoadLevel1()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
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
