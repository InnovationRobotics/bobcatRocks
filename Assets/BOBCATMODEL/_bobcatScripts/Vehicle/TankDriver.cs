//Written by Yossi Cohen <yossicohen2000@gmail.com>


using RosSharp.RosBridgeClient;

using UnityEngine;

public class TankDriver : Subscriber<RosSharp.RosBridgeClient.Messages.Sensor.Joy>
{
    public bool ManualInput = true;

    public Rigidbody[] LeftWheels, RightWheels;
    HingeJoint[] rightHinges, leftHinges;
    Transform myref;
    public float MaxTorque = 400,
            MaxSpeed = 400,
          MaxBreakingTorque = 0,
          MaxSteeringSpeed = 1,
          WheelFriction = 1,
          VelocityDamping = 0.05f,
          VehicleWidth = 2,
          VehicleLength = 3,
          Torque,
          HelperForce = 0,
          BreakPedal,
          ForwardVel = 0;
    float throttle, tempWheelFriction, angvel, appliedTrq, angular;
    float throttleRos, angularRos;
    public float rightSpeed;
    public float leftSpeed;
    Rigidbody rb;



    //ARM PARAMS

    public ConfigurableJoint Arm;
    public HingeJoint loader, brackets;
    public float armpos = 0.05f, loaderpos = -15;
    public float bracketspos = -15;
    public TankDriver tankDriver;
    private float armRos, loaderRos;
    Controllers control;

    // Use this for initialization
    void Awake()
    {
        control = new Controllers();
        control.GamePlay.Arm.performed += X => MoveArm(0);
    }
    void OnEnable()
    {
        control.GamePlay.Enable();
    }
    void OnDisable()
    {
        control.GamePlay.Disable();
    }
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        myref = transform;

        leftHinges = new HingeJoint[LeftWheels.Length];
        rightHinges = new HingeJoint[RightWheels.Length];
        for (int i = 0; i < LeftWheels.Length; i++)
        {
            leftHinges[i] = LeftWheels[i].GetComponent<HingeJoint>();
            rightHinges[i] = RightWheels[i].GetComponent<HingeJoint>();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ForwardVel = myref.InverseTransformDirection(rb.velocity).z;
        if (ManualInput)//For KeyBoard Use

        {
            angular = -(Input.GetAxisRaw("Horizontal"));
            angular = Mathf.Clamp(angular, -1, 1);
            throttle = (Input.GetAxis("Vertical"));
            Apply(throttle, angular);

            MoveArm(Input.GetAxis("Arm"));
            MoveLoader(Input.GetAxis("Loader"));
            MoveBracket(Input.GetAxis("bracket"));


        }
        else //From Ros 
        {
            Apply(throttleRos, angularRos);
            MoveArm(armRos);
            MoveLoader(loaderRos);
        }

    }
    public void Apply(float Throttle, float Steer)
    {

        //  Debug.Log("Apply has been called with Throttle=" + Throttle.ToString() + " and Steer=" + Steer.ToString());
        Torque = MaxTorque * Throttle;
        rightSpeed = Throttle * MaxSpeed + Steer * MaxSteeringSpeed;
        leftSpeed = Throttle * MaxSpeed - Steer * MaxSteeringSpeed;
        rb.AddRelativeForce(0, 0, Throttle * HelperForce);

        for (int i = 0; i < rightHinges.Length; i++)
        {
            angvel = rightHinges[i].velocity;
            // Wheels[i].angularDrag = Break * MaxBreakingTorque;
            ForwardVel = Throttle != 0 ? ForwardVel : 0;
            appliedTrq = Torque - Mathf.Clamp((VelocityDamping + tempWheelFriction) * ForwardVel, -MaxBreakingTorque, MaxTorque);

            var tempmotor = rightHinges[i].motor;
            tempmotor.targetVelocity = rightSpeed;
            tempmotor.force = MaxTorque;
            rightHinges[i].motor = tempmotor;
            tempmotor = leftHinges[i].motor;
            tempmotor.targetVelocity = leftSpeed;
            tempmotor.force = MaxTorque;
            leftHinges[i].motor = tempmotor;
            // Wheels[i].AddRelativeTorque(new Vector3(appliedTrq, 0, 0), ForceMode.Force);
        }


    }


    public void MoveArm(float parameter)
    {
        armpos = Mathf.Clamp(armpos, -0.10f, 0.52f);
        armpos += 0.1f * Time.fixedDeltaTime * parameter;
        if (Arm)
        {
            Arm.targetPosition = new Vector3(armpos, armpos, armpos);
        }
    }

    public void MoveLoader(float parameter)
    {
        loaderpos += 15f * Time.fixedDeltaTime * parameter;
        loaderpos = Mathf.Clamp(loaderpos, -110, 10);
        var temp = loader.spring;
        temp.targetPosition = loaderpos;
        loader.spring = temp;

    }
    public void MoveBracket(float parameter)
    {
        bracketspos += 15f * Time.fixedDeltaTime * parameter;
        bracketspos = Mathf.Clamp(bracketspos, -110, 110);
        var tempbrackets = brackets.spring;
        tempbrackets.targetPosition = bracketspos;
        brackets.spring = tempbrackets;

    }

    protected override void ReceiveMessage(RosSharp.RosBridgeClient.Messages.Sensor.Joy message)
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

        //RightTrigger   [5] Defult is -1 When Press 1
        //LeftTrigger    [2] Defult is -1 When Press 1



     //   Debug.Log("Joystic " + message);
        Debug.Log("Received JOY values=" + message.axes[0].ToString() + "," + message.axes[2].ToString() + "," + message.axes[3].ToString() + "," + message.axes[4].ToString() + "," + message.axes[5].ToString());

        if (message.axes[5] < 0 && message.axes[2] > 0)
        {
            throttleRos = message.axes[5];
            throttleRos = Mathf.Abs(throttleRos);
        }
        else if (message.axes[5] > 0)
        {
            throttleRos = 0;
        }
        if (message.axes[2] < 0 && message.axes[5] > 0)
        {
            throttleRos = message.axes[2];
        }




        //throttleRos = message.axes[5] < 0 && message.axes[2] > 0 ? Mathf.Abs(throttleRos) : 0;//throttle trigger preesed from 1 to -1 (-1 is press state) 


        //throttleRos = message.axes[2] < 0 && message.axes[5] > 0 ? throttleRos : 0;//Reverse

        //message.axes[2] = message.axes[2] == -1 ? throttleRos = 0 : message.axes[2];//If Brackes Trigger Pressed throttle =0;
        angularRos = message.axes[0];//Sterring
        armRos = message.axes[4]; //Arm Up/down
        loaderRos = message.axes[3]; //Loader Up/Down


    }


}

