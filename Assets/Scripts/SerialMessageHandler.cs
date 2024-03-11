using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;

/// <summary>
/// This class handles message construction and deconstruction between Unity and Arduino
/// </summary>
public class SerialMessageHandler : MonoBehaviour
{
    #region FIELDS

    /// <summary>
    /// Class which handles receiving and sending message
    /// </summary>
    public SerialController serialController;

    /// <summary>
    /// Update IoT state rate in seconds
    /// </summary>
    public float pollingRate = 10.0f;

    /// <summary>
    /// Message read from serial
    /// </summary>
    private string incomingMessage = null;

    /// <summary>
    /// Deconstructed message
    /// </summary>
    private string[] IoTMessage = null;

    /// <summary>
    /// Index of Source in message protocol
    /// </summary>
    private const int indexSource = 0;

    /// <summary>
    /// Index of Command in message protocol
    /// </summary>
    private const int indexCommand = 1;

    /// <summary>
    /// Index of Payload in message protocol
    /// </summary>
    private const int indexPayload = 2;

    /// <summary>
    /// Delay to give Arduino time to send message
    /// </summary>
    private const float delayReply = 2.0f;

    #endregion

    #region UNITY

    private void Awake()
    {
        // Get the component this script is on
        serialController = GetComponent<SerialController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Continuously update IoT states every x seconds
        InvokeRepeating("PollIoT", 0.0f, pollingRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region METHODS

    /// <summary>
    /// Request values from IoT
    /// </summary>
    private void PollIoT()
    {
        // Ask IoT for data
        Debug.Log("Start polling...");
        serialController.SendSerialMessage("GAMEON\n");

        // Add delay to give Arduino time to send message before reading
        Invoke("GetMessage", delayReply);
    }

    /// <summary>
    /// Read serial message and perform actions
    /// </summary>
    private void GetMessage()
    {
        // Read message in serial
        incomingMessage = serialController.ReadSerialMessage();
        if (incomingMessage == null)
        {
            Debug.Log("No incomingMessage...");
            return;
        }
        else if (ReferenceEquals(incomingMessage, "__Connected__"))
        {
            Debug.Log("Connected to Arduino");
            incomingMessage = null;
            return;
        }

        Debug.Log("incomingMessage: " + incomingMessage);

        DeconstructMessage();

        UpdateGameObjectStates();

        // Reset after use
        incomingMessage = null;
        IoTMessage = null;
    }

    /// <summary>
    /// Break message into parts for processing
    /// </summary>
    private void DeconstructMessage()
    {
        // Abort if there is no message to prevent error
        if (incomingMessage == null)
            return;

        // Split incoming message into actionable parts
        IoTMessage = incomingMessage.Split(',');
        Debug.Log("Message from IOT:");
        for (int i = 0; i <  IoTMessage.Length; i++)
        {
            Debug.Log("|-- " + IoTMessage[i]);
        }
    }

    /// <summary>
    /// Apply IoT states to game objects
    /// </summary>
    private void UpdateGameObjectStates()
    {
        // Abort if no deconstructed message
        // Abort if IoT does not send its value
        if (IoTMessage == null || IoTMessage[indexCommand] != "SEND") return;

        // Apply each sensor state to game object
        for (int i = indexPayload; i < IoTMessage.Length; i=i+2)
        {
            Debug.Log("Sensor Value: " + IoTMessage[i] + ": " + IoTMessage[i+1]);
        }
    }

    #endregion
}
