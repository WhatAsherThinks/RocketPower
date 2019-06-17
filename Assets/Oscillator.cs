using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // attribute that doesnt allow for more than one of same script

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f; //time it takes for one cycle

    [Range(0, 1)][SerializeField]float movementFactor; // 0 = not moved, 1 = fully moved

    Vector3 startingPos; // must be stored for absolute movement

    
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon) // smallest non zero float. This protects you if period is 0
        {
            return;
        }
        // game time divided by period specified above. grows continuosly from 0
        //automatically framerate independent
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2; //about 6.28 close to the value of tau (1 full cycle or 2pi)
        float rawSinWave = Mathf.Sin(cycles * tau); //Varries between -1 and 1

        //print(rawSinWave);

        //we need the movemet factor to be between 0 and 1.
        // Sine wave is -1 and 1. divide by 2 and you get -.5 and .5 that is why you must add .5 to get 0 and 1
        movementFactor = rawSinWave / 2f + .5f;
        
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset; 
    }
}
