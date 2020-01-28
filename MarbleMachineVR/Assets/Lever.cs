using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Lever : MonoBehaviour
{
    public CircularDrive drive;

    public event EventHandler PositionChanged;

    public float DrivePosition = 0;

    float lastDriveAngle = 0;
    float maxAngle = 45;
    float minAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxAngle = drive.maxAngle;
        minAngle = drive.minAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (drive.outAngle != lastDriveAngle)
        {
            lastDriveAngle = drive.outAngle;
            DrivePosition = (drive.outAngle - minAngle) / (maxAngle - minAngle);
            PositionChanged?.Invoke(this, new EventArgs());
        }
    }
}
