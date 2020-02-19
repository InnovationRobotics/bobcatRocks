﻿using RosSharp.RosBridgeClient;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
     if (other.gameObject.tag == "Rock")
        {
            boolPublisher.ValueToPublish = true;
            boolPublisher. Topic = "/stone/" + other.gameObject.transform.parent.name + "/IsLoaded";
            boolPublisher.Start();
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
            boolPublisher.ValueToPublish = false;
        
    }

    
   

}
