using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarblePath : MonoBehaviour
{
    public BezierSpline Path;

    List<TransportedMarble> marblesUnderTransport = new List<TransportedMarble>();

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.StartsWith("Marble"))
        {
            TransportMarble(collision.gameObject);
        }
        else
            HelperFunctions.Log("wrong obj: " + collision.gameObject.name);
    }

    private void TransportMarble(GameObject gameObject)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        gameObject.transform.position = Path.GetPoint(0);
        marblesUnderTransport.Add(new TransportedMarble
        {
            Marble = gameObject,
            Speed = 1 // Todo get from rigidbody
        });
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var marble in marblesUnderTransport)
        {
            marble.Position += marble.Speed * Time.deltaTime;

            if (marble.Position >= 1)
                FinishMarbleTransport(marble);
            else
            {
                float lastYPosition = marble.Marble.transform.position.y;
                marble.Marble.transform.position = Path.GetPoint(marble.Position);
                marble.Speed += (-0.05f + lastYPosition - marble.Marble.transform.position.y) * Time.deltaTime;
            }
        }
    }

    private void FinishMarbleTransport(TransportedMarble marble)
    {
        marblesUnderTransport.Remove(marble);
        marble.Marble.transform.position = Path.GetPoint(1);
        marble.Marble.GetComponent<Rigidbody>().isKinematic = false;
        marble.Marble.GetComponent<Rigidbody>().detectCollisions = true;
    }
}

public class TransportedMarble
{
    public float Position = 0;
    public float Speed = 5;
    public GameObject Marble;
}