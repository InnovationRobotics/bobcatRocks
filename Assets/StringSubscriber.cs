using RosSharp.RosBridgeClient.Messages.Standard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class StringSubscriber : Subscriber<Messages.Standard.String>
    {
        private bool isMessageReceived;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(String message)
        {
            Debug.Log("Message received");
            isMessageReceived = true;
            //throw new System.NotImplementedException();
        }

        

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
