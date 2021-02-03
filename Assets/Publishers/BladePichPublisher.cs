using RosSharp.RosBridgeClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class BladePichPublisher : Publisher<MessageTypes.Std.Int32>
    {
        public int ValueToPublish = 0;
        //public float Rate;

        private MessageTypes.Std.Int32 message;
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
            message = new MessageTypes.Std.Int32();

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
             message.data = CalcRange(hight);
             Publish(message);
        }

        private int CalcRange(float val)
        {

            val = 1.077f * val + 46.582f;
            Debug.Log("Pitch:" + (int)val);
            return (int)val;


        }
    }
}
