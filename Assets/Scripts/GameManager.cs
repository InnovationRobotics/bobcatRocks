﻿using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Subscriber<RosSharp.RosBridgeClient.MessageTypes.Sensor.Joy>
{


    public static GameManager Instance;

    private TankDriver tankDriver;
    public DLDriver DlDriver;
    public DLDriveJoy DlDriveJoy;

    public bool ManualInput;
    public bool DriverInput;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetScene();
        }
    }

    public void ToggeleManualInput(bool state)
    {

        ManualInput = !ManualInput;
    }

    protected override void ReceiveMessage(Joy message)
    {
        throw new System.NotImplementedException();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }


    public void ToggleCOntrollerInput()
    {
        DriverInput = !DriverInput;
        DlDriveJoy.enabled = DriverInput;
        DlDriver.enabled = !DriverInput;
    }



}
