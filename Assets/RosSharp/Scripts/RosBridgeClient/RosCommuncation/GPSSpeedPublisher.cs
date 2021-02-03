/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;
using AGXUnity;


namespace RosSharp.RosBridgeClient
{
    public class GPSSpeedPublisher : Publisher<MessageTypes.Sensor.NavSatFix>
    {
        public string FrameId = "Unity";
        public RigidBody rb;
        public bool Outside_Time_Synchronization=false;

        private MessageTypes.Sensor.NavSatFix message;
        private float start_latitude, start_longitude, start_altitude;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            start_latitude = 0f;//31.2622f; //32.0017549051;
            start_longitude = 0f;// 34.803611f; //34.9083870312;           
            start_altitude = 0f;//2.0f;
     
        }

        private void FixedUpdate()
        {
          //  if (Time.deltaTime <1.0/pfreq) return;
            if (Outside_Time_Synchronization){
                return;
            }
            UpdateMessage();
        }

        public void InitializeMessage()
        {
            message = new MessageTypes.Sensor.NavSatFix
            {
                header = new MessageTypes.Std.Header()
                {
                    frame_id = FrameId
                },
                status = new MessageTypes.Sensor.NavSatStatus()
                {
                    status = (sbyte)MessageTypes.Sensor.NavSatStatus.STATUS_FIX,
                    service = (ushort)MessageTypes.Sensor.NavSatStatus.SERVICE_GPS
                },
                latitude = start_latitude,
                longitude = start_longitude,
                altitude = start_altitude          
            };
            message.position_covariance = new double[] {1,0,0,0,1,0,0,0,1};
            message.position_covariance_type=  0;
        }

        private void UpdateMessage()
        {
                message.header.Update();
         
                //Compute current coordinates
                message.longitude = rb.LinearVelocity.x;
                message.latitude = rb.LinearVelocity.z;
                message.altitude = rb.LinearVelocity.y;
                Debug.Log("velocity="+rb.LinearVelocity.ToString());
                Publish(message);
           
        }

        public void SendSynchronizedMessage(MessageTypes.Std.Time synchronized_time)
        {
                message.header.TimeSynchronization(synchronized_time);
                //Compute current coordinates
                message.longitude = rb.LinearVelocity.x;
                message.latitude = rb.LinearVelocity.z;
                message.altitude = rb.LinearVelocity.y;
                Debug.Log("SendSynchronizedMessage velocity="+rb.LinearVelocity.ToString());
                Publish(message);
           
        }
    }
}