using RosSharp.RosBridgeClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDetect : MonoBehaviour
{
    // Start is called before the first frame update
    BoolPublisher boolPublisher;
    void Start()
    {
        boolPublisher = GetComponent<BoolPublisher>();

        boolPublisher.ValueToPublish = false;
        boolPublisher.Topic = "/stone/" + transform.name + "/IsLoaded";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
     if (other.gameObject.tag == "Loader")
        {
            boolPublisher.ValueToPublish = true;
            boolPublisher. Topic = "/stone/" + transform.name + "/IsLoaded";
            boolPublisher.Start();
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
            boolPublisher.ValueToPublish = false;
        
    }

    
   

}
