using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // attribute that doesnt allow for more than one of same script

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;

    [Range(0, 1)]
    [SerializeField]
    float movementFactor; // 0 = not moved, 1 = fully moved

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
