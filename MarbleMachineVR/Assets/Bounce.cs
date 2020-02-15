using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float BounceStrength = 3;
    public float SpringStrength = 1;
    float angleSpeed = 0;
    bool isBouncing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBouncing)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x + angleSpeed, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z));
            if (angleSpeed > BounceStrength)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z));
                isBouncing = false;
            }
            else
                angleSpeed += SpringStrength; // Todo more realistic spring physics
        }
    }

    public void DoBounce()
    {
        angleSpeed = -BounceStrength;
        isBouncing = true;
    }
}
