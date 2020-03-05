using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Messages.Sensor;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Subscriber<RosSharp.RosBridgeClient.Messages.Sensor.Joy>
{




    private TankDriver tankDriver;
   
    

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
        tankDriver = FindObjectOfType<TankDriver>();
        tankDriver.ManualInput = !tankDriver.ManualInput;
    }

    protected override void ReceiveMessage(Joy message)
    {
        throw new System.NotImplementedException();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit()
    {
        GetComponent<RosConnector>().RosSocket.Close();
    }


}

