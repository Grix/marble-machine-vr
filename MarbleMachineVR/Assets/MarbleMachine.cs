using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarbleMachine : MonoBehaviour
{
    public float Position { get { return position; } } // In degrees
    float position = 0;
    float inputTorque = 0;
    float speed = 12;
    float flywheelSpeed = 0;
    float frictionConstant = 0f;//0.2f;
    bool flywheelIsEngaged = true;
    float mutePosition = 0; // 0: fully unmuted, 1: fully muted

    float FlywheelInertiaRatio = 10;
    public int NumBars = 16;
    public int NumChannels = 38;

    //List<InstrumentChannel> instrumentChannels = new List<InstrumentChannel>();

    public List<List<float>> PinPositions { get; } = new List<List<float>>();

    public event EventHandler PinPositionsChanged;
    public event EventHandler MutePositionChanged;
    

    void Start()
    {
        for (int i = 0; i < NumChannels; i++)
            PinPositions.Add(new List<float>());

        Invoke("LoadTestPattern", 1);
        Invoke("LoadMidiFile", 2);
    }

    void LoadTestPattern()
    {
        for (int i = 0; i < NumChannels; i++)
            PinPositions.Add(new List<float>());
        /*for (int i = 0; i < 360; i += 8)*/
        for (int i = 0; i < NumChannels; i++)
            PinPositions[i].Add(10+i);

        TriggerPinPositionsChangedEvent();
    }

    void LoadMidiFile()
    {
        new MidiLoader(this).OpenMidiFileBrowser();

    }

    // Received input torque from crank or pedal etc.
    public void InputTorque(float deltaAngle)
    {
        //Debug.Log(deltaAngle.ToString());
        inputTorque = Mathf.Max(deltaAngle, 10*Time.deltaTime);
    }

    public void InputTorqueAsSpeed(float inputSpeed)
    {
        float multiplier = 0.1f;
        inputTorque = (inputSpeed - (speed/Time.deltaTime / 2)) * multiplier; // why is /Time.deltaTime/2 needed?

        HelperFunctions.Log(inputSpeed, (speed / Time.deltaTime / 2), inputTorque);
    }

    public void TriggerPinPositionsChangedEvent()
    {
        PinPositionsChanged?.Invoke(this, new EventArgs());
    }

    void FixedUpdate()
    {
        UpdateWheelPosition();
    }

    void UpdateWheelPosition()
    {
        var friction = frictionConstant;// * 1/Math.Log(speed+1.1);
        var deltaSpeed = (inputTorque - friction) * Time.fixedDeltaTime;
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

    public void LoadProgramming(List<List<float>> pinPositions)
    {
        PinPositions.Clear();
        for (int i = 0; i < pinPositions.Count || i < NumChannels; i++)
        {
            PinPositions.Add(pinPositions[i]);
        }
        // todo error checking etc, do the pin positions fit in the programming plates?
        TriggerPinPositionsChangedEvent();
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