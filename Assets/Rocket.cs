using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //SerializeField makes it avaialble to Unity
    [SerializeField] float rcsThrust = 100f; //rcs = reaction control system
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float loadLevelDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip dyingSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    [SerializeField] bool collisionEnabled = true;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            //toggle for collision
            collisionEnabled = !collisionEnabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //ignore collisions when dead
        if(state != State.Alive || !collisionEnabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("ok");
                break;
            case "Finish":
                print("win");
                StartSuccessSequence();
                break;
            default:
                print("dead");
                StartDeathSequence();
                break;
        }
        print("collided");
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        Invoke("LoadNextLevel", loadLevelDelay); 
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(dyingSound);
        deathParticles.Play();
        Invoke("LoadLevel1", loadLevelDelay);
    }

    private void LoadLevel1()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentScene + 1;
        /* if the next scene is larger than index in build settings
        (ex: 9 scenes in build settings = 8index. So if the next level is index9, go to level 1.
        There is no index 9 at the moment.) */
        if (nextSceneIndex ==  SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel1();
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            print("space pressed");
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        //multiplying by time.deltatime makes it frame rate independent
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        //To make sure the sound doesnt layer. 
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}
