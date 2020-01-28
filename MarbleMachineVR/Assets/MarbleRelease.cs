using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleRelease : MonoBehaviour
{
    public TriggerAssembly TriggerAssembly;
    public GameObject Marble;
    //public Quaternion ReleasePosition = Quaternion.Euler(transform.position.eulerAngles.x, transform.position.eulerAngles.y-0.2, transform.position.eulerAngles.z);
    
    bool marbleIsLoaded = true;
    float triggerPosition = 0;
    bool isTriggering = false;

    DateTime lastHitTime;

    // Start is called before the first frame update
    void Start()
    {
    }
    
    void FixedUpdate()
    {
        triggerPosition = TriggerAssembly.ActualTriggerPosition;
        if (!isTriggering)
        {
            if (triggerPosition > 0.5)
            {
                isTriggering = true;
                ReleaseMarble();
            }
        }
        else
        {
            if (triggerPosition < 0.2)
                isTriggering = false;
        }
    }

    void Update()
    {
        
    }

    void LoadMarble()
    {
        marbleIsLoaded = true;
    }

    public void ReleaseMarble()
    {
        if (marbleIsLoaded)
        {
            Instantiate(Marble, 
                        new Vector3(transform.position.x, 
                                    transform.position.y-0.04f, 
                                    transform.position.z), 
                         Quaternion.identity);
            //MarbleIsLoaded = false;

            HelperFunctions.Log("hit", DateTime.Now - lastHitTime);
            lastHitTime = DateTime.Now;
        }
    }
}
