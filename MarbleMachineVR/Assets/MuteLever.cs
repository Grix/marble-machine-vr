using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Lever))]
public class MuteLever : MonoBehaviour
{

    public MarbleMachine MarbleMachine;
    public Lever Lever;

    // Start is called before the first frame update
    void Start()
    {
        Lever = GetComponent<Lever>();
        Lever.PositionChanged += delegate { MarbleMachine.MutePosition = Lever.DrivePosition; };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
