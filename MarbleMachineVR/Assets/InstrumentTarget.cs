using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentTarget : MonoBehaviour
{
    public float Pitch;
    public GameObject Marble;
    public Bounce PivotArm;

    DateTime lastHitTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.StartsWith("Marble"))
        {
            GetComponent<AudioSource>().Play();
            PivotArm?.DoBounce();
            //HelperFunctions.Log("hit", DateTime.Now - lastHitTime);
            //lastHitTime = DateTime.Now;
        }
        else
            HelperFunctions.Log("wrong obj: "+ collision.gameObject.name);
    }
}
