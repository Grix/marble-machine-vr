using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAssembly : MonoBehaviour
{
    public MarbleMachine MarbleMachine;
    public Transform Registrator;
    public Transform Trigger;
    public int Channel = 0;

    Vector3 baseTriggerPosition;
    Vector3 baseRegistratorPosition;
    float triggerPosition = 0; // 0: fully inactive, 1: fully triggered
    float mutePosition = 0; // 0: fully unmuted, 1: fully muted

    List<float> pinPositions;
    float lastPosition = 0;
    int nextPinIndex = 0;
    float registratorTravel = 2;

    bool IsMuted { get { return mutePosition > 0.5; } }
    public float ActualTriggerPosition { get { return IsMuted ? 0 : triggerPosition; } }

    private void UpdateMutePosition()
    {
        mutePosition = MarbleMachine.MutePosition;
        Trigger.localRotation = Quaternion.Euler(mutePosition * 10, 0, 0);
    }

    private void UpdateTriggerPosition()
    {
        if (pinPositions == null || pinPositions.Count == 0)
            return;

        // Calculate if trigger is hitting pin
        float position = MarbleMachine.Position;
        //if (pinPositions[nextPinIndex] < 3 && position > pinPositions[nextPinIndex])
        //position -= 360;

        float bestTriggerPosition = 0;
        foreach (float pinPosition in pinPositions)
        {
            float fixedPinPosition = pinPosition;
            if (pinPosition < registratorTravel && position > (360 - registratorTravel))
                fixedPinPosition += 360;

            var thisPinTriggerPosition = Mathf.Max(position - fixedPinPosition + registratorTravel, 0);
            if (thisPinTriggerPosition < registratorTravel && thisPinTriggerPosition > bestTriggerPosition)
                bestTriggerPosition = thisPinTriggerPosition;
            else if (bestTriggerPosition > 0)
                break;
        }

        triggerPosition = bestTriggerPosition;

        //triggerPosition = Mathf.Max(position - pinPositions[nextPinIndex] + 2, 0);

        //if (pinPositions.Count > 1)
        //{
            //if (triggerPosition > 2)
            //{
             //   nextPinIndex = (nextPinIndex + 1) % pinPositions.Count;
             //   triggerPosition = Mathf.Max(position - pinPositions[nextPinIndex] + 2, 0);
            //}
        //}


        if (triggerPosition == 0 && lastPosition == 0)
            return;

        // Hitting pin, so move transforms
        Registrator.localPosition = new Vector3(baseRegistratorPosition.x, baseRegistratorPosition.y, baseRegistratorPosition.z + triggerPosition / 100);
        Trigger.localPosition = new Vector3(baseTriggerPosition.x, baseTriggerPosition.y, baseTriggerPosition.z + ActualTriggerPosition / 100);

        lastPosition = position;
    }

    // Start is called before the first frame update
    void Start()
    {
        baseTriggerPosition = Trigger.localPosition;
        baseRegistratorPosition = Registrator.localPosition;
        MarbleMachine.PinPositionsChanged += delegate { pinPositions = MarbleMachine.PinPositions[Channel]; };
        MarbleMachine.MutePositionChanged += delegate { UpdateMutePosition(); };
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMutePosition();
    }

    void FixedUpdate()
    {
        UpdateTriggerPosition();
    }
}
