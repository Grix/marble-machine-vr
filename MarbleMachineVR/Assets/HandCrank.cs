using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandCrank : MonoBehaviour
{
    public MarbleMachine MarbleMachine;
    public Renderer GrabbingHandModel;
    public GameObject Handle;

    bool isGrabbed = false;
    float previousAngle = 0;
    float torqueRatio = 10;
    float roationRadius = 0.5f;
    Hand grabbingHand;

    public delegate void CrankTurnedDelegate(float deltaAngle);
    public event CrankTurnedDelegate CrankTurned;

    // Start is called before the first frame update
    void Start()
    {
        if (GrabbingHandModel != null)
            GrabbingHandModel.enabled = false;
        GetComponent<CircularDrive>().HandAttached += HandCrank_HandAttached;
        GetComponent<CircularDrive>().HandDetached += HandCrank_HandDetached;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            /*// Calculate how much the user tries to crank the handle
            var handPosition = transform.InverseTransformPoint(grabbingHand.transform.position);
            var handAngle = Mathf.Acos(handPosition.x / handPosition.z);
            var handleAngle = Mathf.Acos(transform.localPosition.x / transform.localPosition.z);

            //Debug.Log("hand "+handAngle);
            //Debug.Log("handle " + transform.localPosition.x.ToString() + " " + transform.localPosition.z.ToString());

            var deltaAngle = handleAngle - handAngle;
            if (deltaAngle != 0)
                CrankTurned?.Invoke(deltaAngle/(2*Mathf.PI)*360);

            // Check if no longer grabbing the handle
            GrabTypes endingGrabType = grabbingHand.GetGrabEnding();
            if (endingGrabType != GrabTypes.None)
            {
                isGrabbed = false;
            }*/
            float angle = GetComponent<CircularDrive>().outAngle;
            //MarbleMachine.InputTorque((angle - previousAngle)*torqueRatio);
            //HelperFunctions.Log(angle, angle - previousAngle);
            var inputSpeed = (angle - previousAngle) / Time.deltaTime;
            //HelperFunctions.Log("handcrank", angle, previousAngle, inputSpeed);
            MarbleMachine.InputTorqueAsSpeed(inputSpeed);
            previousAngle = angle;
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
        isGrabbed = false;
        if (GrabbingHandModel != null)
            GrabbingHandModel.enabled = false;
    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
        /*GrabTypes startingGrabType = hand.GetGrabStarting(GrabTypes.Grip);
        if (startingGrabType != GrabTypes.None)
        {
            isGrabbed = true;
            grabbingHand = hand;
            hand.AttachObject(gameObject, startingGrabType, Hand.AttachmentFlags.DetachFromOtherHand);
            //hand.HideGrabHint();
        }*/
    }
}
