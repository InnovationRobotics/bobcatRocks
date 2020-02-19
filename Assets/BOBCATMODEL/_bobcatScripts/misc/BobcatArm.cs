using UnityEngine;

public class BobcatArm : MonoBehaviour
{
    public ConfigurableJoint Arm;
    public HingeJoint loader, brackets;
    public float armpos = 0.05f, loaderpos = -15;
    public float bracketspos = -15;
    public TankDriver tankDriver;
    private float armRos, loaderRos;

    Controllers control;
    // Use this for initialization
    void Start()
    {
        tankDriver = GetComponent<TankDriver>();
    }

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
    // Update is called once per frame
    void FixedUpdate()
    {
        if (tankDriver.ManualInput)
        {
            MoveArm(Input.GetAxis("Arm"));
            MoveLoader(Input.GetAxis("Loader"));
            MoveBracket(Input.GetAxis("bracket"));
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


}
