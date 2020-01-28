using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleMachineWheel : MonoBehaviour
{
    public MarbleMachine MarbleMachine;
    public float GearRatio = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler((MarbleMachine.Position * GearRatio) % 360, 0, 90);
    }
}
