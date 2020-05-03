using AGXUnity.Model;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Messages.Mavros;
using UnityEngine;

public class DLDriver : Subscriber<RosSharp.RosBridgeClient.Messages.Mavros.OverrideRCIn> /*Subscriber<RosSharp.RosBridgeClient.Messages.Sensor.Joy>*/
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




    //protected override void ReceiveMessage(Joy message)
    //{
    //    //Right Stick LEFT/RIGHT    -1 <-[0]-> 1 

    //    //                                  1
    //    //Right Stick UP/DOWN       |
    //    //                                 [1]
    //    //                                  |
    //    //                                 -1


    //    //Left Stick LEFT/RIGHT    -1 <-[3]-> 1

    //    //                                  1
    //    // Left Stick UP/DOWN               |
    //    //                                 [4]
    //    //                                  |
    //    //                                 -1

    //    //RightTrigger   [5] Defult is 1 When Press -1
    //    //LeftTrigger    [2] Defult is 1 When Press -1



    //    //   Debug.Log("Joystic " + message);
    //    // Debug.Log("Received JOY values=" + message.axes[0].ToString() + "," + message.axes[2].ToString() + "," + message.axes[3].ToString() + "," + message.axes[4].ToString() + "," + message.axes[5].ToString());

    //    if (message.axes[5] < 0 && message.axes[2] > 0) //Right prees
    //    {
    //        Throttle = message.axes[5];
    //        WheelLoaderInputController.throttle = Mathf.Abs(Throttle);
    //        WheelLoaderInputController.brake = 0;

    //    }
    //    if (message.axes[2] > 0 && message.axes[5] > 0) //Left press
    //    {
    //        WheelLoaderInputController.brake = 0;
    //        WheelLoaderInputController.throttle = 0;
    //    }

    //    if (message.axes[2] < 0 && message.axes[5] > 0) //Left press
    //    {
    //        Brake = message.axes[2];

    //        WheelLoaderInputController.brake = Mathf.Abs(Brake);
    //        WheelLoaderInputController.throttle = 0;
    //    }





    //    WheelLoaderInputController.steer = message.axes[0];//Sterring
    //    WheelLoaderInputController.elevate = (message.axes[4]); //Arm Up/down
    //    WheelLoaderInputController.tilt = (message.axes[3]); //Loader Up/Down

    //}





    protected override void ReceiveMessage(OverrideRCIn message)
    {
        //Throttle 
        //min 1100
        //max 1900


        //  Steer_Blade
        //min 1200-1000 Left   -1
        //max 1800-2000 Right  1             
        //1500 Stop

        //arm
        // 1500 Stop
        // Min 1900-2100  -1
        // Max 1100-900   1

        //2 Steer  
        //0 Throttle 
        //1 Arm   
        //3 Blade
        //5 oilpump
        //6 gear

        if (ReturnClampedNumberThrottle(message.channels[0]) > 0) //Right prees
        {
            Throttle = message.channels[0];

            WheelLoaderInputController.throttle = ReturnClampedNumberThrottle(Throttle);
            WheelLoaderInputController.brake = 0;

        }
        if (ReturnClampedNumberThrottle(message.channels[0]) == 0) //brack press
        {
            WheelLoaderInputController.brake = 0;
            WheelLoaderInputController.throttle = 0;
        }

        if (ReturnClampedNumberThrottle(message.channels[0]) < 0) //Left press
        {
            Brake = message.channels[0];

            WheelLoaderInputController.brake = Mathf.Abs(ReturnClampedNumberThrottle(Brake));
            WheelLoaderInputController.throttle = 0;
        }


        WheelLoaderInputController.steer = ReturnClampedNumberSteer_Blade(message.channels[2]);//Sterring
        WheelLoaderInputController.elevate = ReturnClampedNumberArm(message.channels[1]); //Arm Up/down
        WheelLoaderInputController.tilt = ReturnClampedNumberSteer_Blade(message.channels[3]); //Loader Up/Down
    }

    public float ReturnClampedNumberThrottle(float val)
    {
        val = (val / 400) - 3.75f;
        return val;
    }

    public float ReturnClampedNumberSteer_Blade(float val)
    {
        if (val == 1500)
            return 0;
        else
        {
            if (val >= 1800 && val <= 2000)
            {
                val = 0.00495f * val - 8.9f;

            }

            else if (val >= 1000 && val <= 1200)
            {
                val = 0.00495f * val - 5.95f;
            }
            return -val;
        }

    }

    public float ReturnClampedNumberArm(float val)
    {
        if (val == 1500)
            return 0;
        else
        {
            if (val >= 1900 && val <= 2100)
            {
                val = -0.00495f * val + 9.395f;

            }
            else if (val >= 900 && val <= 1100)
            {
                val = -0.00495f * val + 5.455f;
            }
            return -val;
        }

    }

}
