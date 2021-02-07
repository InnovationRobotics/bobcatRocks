using RosSharp.RosBridgeClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class BladePichPublisher : Publisher<MessageTypes.Std.Float64>
    {
        public int ValueToPublish = 0;
        //public float Rate;

        private MessageTypes.Std.Float64 message;
        // private Transform Parent;
        

        protected override void Start()
        {
            base.Start();           
            InitializeMessage();
        }

        void FixedUpdate()
        {
            UpdateMessage();
        }



        private void InitializeMessage()
        {
            message = new MessageTypes.Std.Float64();

        }

        private void UpdateMessage()
        {
            if (message == null)
                return;
            //Down 213 -->276 
            //UP 31  -->80

            //we gonna use the Y Rotation to measure the hight
            float hight = transform.localEulerAngles.y;
           // Debug.Log("Pitch :" + hight);
           var data = CalcRange(hight);
            message.data = data;
             Publish(message);
        }

        private float CalcRange(float val)
        {

            val = 1.077f * val + 46.582f;
            Debug.Log("Pitch:" + val);
            return val;


        }
    }
}
