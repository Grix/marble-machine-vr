using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarbleMachine : MonoBehaviour
{
    public float Position { get { return position; } } // In degrees
    float position = 0;
    float inputTorque = 0;
    float speed = 30;
    float flywheelSpeed = 0;
    float frictionConstant = 1f;
    bool flywheelIsEngaged = true;
    float mutePosition = 0; // 0: fully unmuted, 1: fully muted

    const int NumberOfBars = 16;
    const float FlywheelInertiaRatio = 10;

    //List<InstrumentChannel> instrumentChannels = new List<InstrumentChannel>();

    public List<List<float>> PinPositions { get; } = new List<List<float>>();

    public event EventHandler PinPositionsChanged;
    public event EventHandler MutePositionChanged;

    public float MutePosition { get { return mutePosition; }
        set
        {
            mutePosition = Mathf.Clamp(value,0,1);
            MutePositionChanged?.Invoke(this, new EventArgs());
        }
    }

    void Start()
    {
        Invoke("LoadTestPattern", 1);
    }

    void LoadTestPattern()
    {
        for (int i = 0; i < 4; i++)
            PinPositions.Add(new List<float>());
        for (int i = 0; i < 360; i += 8)
            PinPositions[0].Add(i);
        for (int i = 8; i < 360; i += 16)
            PinPositions[1].Add(i);
        for (int i = 0; i < 360; i += 4)
            PinPositions[2].Add(i);

        PinPositionsChanged?.Invoke(this, new EventArgs());
    }

    // Received input torque from crank or pedal etc.
    void InputTorque(float deltaAngle)
    {
        //Debug.Log(deltaAngle.ToString());
        inputTorque = Mathf.Max(deltaAngle, 10*Time.deltaTime);
    }
    
    void FixedUpdate()
    {
        UpdateWheelPosition();
    }

    void UpdateWheelPosition()
    {
        var friction = frictionConstant;// * 1/Math.Log(speed+1.1);
        var deltaSpeed = (inputTorque - (float)friction) * Time.fixedDeltaTime;
        if (flywheelIsEngaged)
        {
            //if (flywheelMoment > otherMoment*flywheelInertiaRatio)

            speed += (float)deltaSpeed / FlywheelInertiaRatio;
            flywheelSpeed += deltaSpeed - deltaSpeed / FlywheelInertiaRatio;
        }
        else
            speed += deltaSpeed;

        if (speed < 0)
            speed = 0;
        position += speed * Time.fixedDeltaTime;
        position %= 360;

        if (inputTorque != 0)
            inputTorque = 0;
    }
    

    /*void UpdateInstrumentChannels()
    {
        foreach (var instrumentChannel in instrumentChannels)
        {
            instrumentChannel.UpdatePosition(position, speed);
        }
    }*/
}

/*public class InstrumentChannel
{
    public List<float> pinPositions = new List<float>();

    public float lastPosition = 0;
    public int nextPinIndex = 0;

    public void UpdatePosition(float position, float speed)
    {
        while (pinPositions[nextPinIndex] <= position && (nextPinIndex != 0 || position < 180))
        {
            nextPinIndex = nextPinIndex + 1;
        }

        if (nextPinIndex >= pinPositions.Count)
            nextPinIndex = 0;

        if (pinPositions[nextPinIndex] > position)
            TriggerAssembly.TriggerPosition = 1 - (position - pinPositions[nextPinIndex]);
        else
            TriggerAssembly.TriggerPosition = 1 - (position - pinPositions[nextPinIndex] + 360);
    }
}*/