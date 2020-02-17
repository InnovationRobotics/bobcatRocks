using UnityEngine;

public class GameManager : MonoBehaviour
{




    private TankDriver tankDriver;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggeleManualInput(bool state)
    {
        tankDriver = FindObjectOfType<TankDriver>();
        tankDriver.ManualInput = !tankDriver.ManualInput;
    }
}

