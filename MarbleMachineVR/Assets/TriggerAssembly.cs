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

    float previousPosition = 0;
    float triggerPosition = 0; // 0: fully inactive, 1: fully triggered
    float mutePosition = 0; // 0: fully unmuted, 1: fully muted

    List<float> pinPositions;
    float lastPosition = 0;
    int nextPinIndex = 0;

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
        triggerPosition = Mathf.Max(position - pinPositions[nextPinIndex] + 1, 0);

        while (triggerPosition > 1)
        {
            if (nextPinIndex == 0 && position > pinPositions[nextPinIndex])
            {
                position -= 360;
                triggerPosition = Mathf.Max(position - pinPositions[nextPinIndex] + 1, 0);
                nextPinIndex = (nextPinIndex + 1) % pinPositions.Count;
            }
            else
            {
                nextPinIndex = (nextPinIndex + 1) % pinPositions.Count;
                triggerPosition = Mathf.Max(position - pinPositions[nextPinIndex] + 1, 0);
            }
        }

        if (triggerPosition == 0 && previousPosition == 0)
            return;

        // Hitting pin, so move transforms
        Registrator.localPosition = new Vector3(0, 0, triggerPosition / 100);
        Trigger.localPosition = new Vector3(0, 0, ActualTriggerPosition / 100);

        lastPosition = position;
    }

    // Start is called before the first frame update
    void Start()
    {
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
