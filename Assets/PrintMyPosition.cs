using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintMyPosition : MonoBehaviour
{
    public bool fixedUpdate;
    public bool update;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (update)
        {
            Debug.Log("UPDATE TIME::: " + Time.time);
            Debug.Log("Update ::: " + " Transform NAME: " + transform.name + "::  X: " + transform.position.x + ":: Y: " + transform.position.y + ":: Z:" + transform.position.z);
            if (GetComponent<Rigidbody>())
                Debug.Log("Rigidbody Update ::: " + " Transform NAME: " + transform.name + ":: X: " + GetComponent<Rigidbody>().position.x + " :: Y:" + GetComponent<Rigidbody>().position.y + ":: Z: " + GetComponent<Rigidbody>().position.z);

        }
        
    }
    private void FixedUpdate()
    {
        if (fixedUpdate)
        {
            // Debug.Log(" FIXEDUPDATE TIME::: " + Time.time); 
            Debug.Log(" Position: " + transform.name + "::  X: " + transform.position.x + ": :Y: " + transform.position.y + ":: Z:" + transform.position.z);
            Debug.Log(" Rotation: " + transform.name + "::  X: " + transform.eulerAngles.x + ": :Y: " + transform.eulerAngles.y + ":: Z:" + transform.eulerAngles.z);
            //if (GetComponent<Rigidbody>())
            //    Debug.Log("FixedUpdate Rigidbody Update ::: " + " Transform NAME: " + transform.name + ":: X: " + GetComponent<Rigidbody>().position.x + ":: Y:" + GetComponent<Rigidbody>().position.y + ":: Z: " + GetComponent<Rigidbody>().position.z);

        }


    }
}
