using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Messages.Sensor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Subscriber<RosSharp.RosBridgeClient.Messages.Sensor.Joy>
{


    public static GameManager Instance;

    private TankDriver tankDriver;

    public bool ManualInput;

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




}

