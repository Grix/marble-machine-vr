using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Lever : MonoBehaviour
{
    public CircularDrive Drive;
    public Renderer GrabbingHandModel;

    public event EventHandler PositionChanged;

    public float DrivePosition = 0;

    bool isGrabbed = false;
    float lastDriveAngle = 0;
    float maxAngle = 45;
    float minAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxAngle = Drive.maxAngle;
        minAngle = Drive.minAngle;

        if (GrabbingHandModel != null)
            GrabbingHandModel.enabled = false;
        Drive.GetComponent<CircularDrive>().HandAttached += HandCrank_HandAttached;
        Drive.GetComponent<CircularDrive>().HandDetached += HandCrank_HandDetached;
    }

    // Update is called once per frame
    void Update()
    {
        if (Drive.outAngle != lastDriveAngle)
        {
            lastDriveAngle = Drive.outAngle;
            DrivePosition = (Drive.outAngle - minAngle) / (maxAngle - minAngle);
            PositionChanged?.Invoke(this, new EventArgs());
        }
    }

    private void HandCrank_HandAttached(object sender, EventArgs e)
    {
        isGrabbed = true;
        if (GrabbingHandModel != null)
            GrabbingHandModel.enabled = true;
    }

    private void HandCrank_HandDetached(object sender, EventArgs e)
    {
        if (GrabbingHandModel != null)
            GrabbingHandModel.enabled = false;
    }
}
