using AGXUnity.Model;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Messages.Sensor;
using UnityEngine;

public class DLDriveJoy : Subscriber<RosSharp.RosBridgeClient.Messages.Sensor.Joy>
{
    public WheelLoaderInputController WheelLoaderInputController;

    public float Throttle = 0;
    public float Brake = 0;
    public float Steer = 0;

    public float Elevate = 0;
    public float Tilt = 0;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {


    }


    protected override void ReceiveMessage(Joy message)
    {
        //Right Stick LEFT/RIGHT    -1 <-[0]-> 1 

        //                                  1
        //Right Stick UP/DOWN       |
        //                                 [1]
        //                                  |
        //                                 -1


        //Left Stick LEFT/RIGHT    -1 <-[3]-> 1

        //                                  1
        // Left Stick UP/DOWN               |
        //                                 [4]
        //                                  |
        //                                 -1

        //RightTrigger   [5] Defult is 1 When Press -1
        //LeftTrigger    [2] Defult is 1 When Press -1



        //   Debug.Log("Joystic " + message);
        // Debug.Log("Received JOY values=" + message.axes[0].ToString() + "," + message.axes[2].ToString() + "," + message.axes[3].ToString() + "," + message.axes[4].ToString() + "," + message.axes[5].ToString());

        if (message.axes[5] < 0 && message.axes[2] > 0) //Right prees
        {
            Throttle = message.axes[5];
            WheelLoaderInputController.throttle = Mathf.Abs(Throttle);
            WheelLoaderInputController.brake = 0;

        }
        if (message.axes[2] > 0 && message.axes[5] > 0) //Left press
        {
            WheelLoaderInputController.brake = 0;
            WheelLoaderInputController.throttle = 0;
        }

        if (message.axes[2] < 0 && message.axes[5] > 0) //Left press
        {
            Brake = message.axes[2];

            WheelLoaderInputController.brake = Mathf.Abs(Brake);
            WheelLoaderInputController.throttle = 0;
        }





        WheelLoaderInputController.steer = message.axes[0];//Sterring
        WheelLoaderInputController.elevate = (message.axes[4]); //Arm Up/down
        WheelLoaderInputController.tilt = (message.axes[3]); //Loader Up/Down

    }

}
