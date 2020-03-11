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
    public class RobilShiffonFusion  : MonoBehaviour //Publisher<Messages.Sensor.Imu>
    {
        Transform myref;
        public string FrameId = "Unity";
        public RigidBody rb;
        Vector3 lastPos = Vector3.zero;
        private Messages.Sensor.Imu imu_msg;
        private Messages.Sensor.NavSatFix gps_speed_msg;
        private Messages.Sensor.NavSatFix gps_msg;

        private ImuPublisher imu_pub;
        private GPSSpeedPublisher gps_speed_pub;
        private float start_latitude, start_longitude, start_altitude;

        private NavSatFixPublisher gps_pub;

        private GameObject me;
        private bool firstCall;
        protected void Start()
        { 
            //myref = transform;
            //me = gameObject;//.GetComponent<RosConnector>();
        //    imu_pub = new ImuPublisher();
            imu_pub = gameObject.AddComponent<ImuPublisher>() as ImuPublisher;//new ImuPublisher();
            imu_pub.Topic = "/SENSORS/INS";
            imu_pub.rb = rb;
            imu_pub.enabled = true;
            imu_pub.Outside_Time_Synchronization = true;
            imu_pub.InitializeMessage();
            gps_speed_pub = gameObject.AddComponent<GPSSpeedPublisher>() as GPSSpeedPublisher;
            gps_speed_pub.Topic = "/SENSORS/GPS/Speed";
            gps_speed_pub.rb = rb;
            gps_speed_pub.enabled = true;
            gps_speed_pub.Outside_Time_Synchronization = true;
            gps_speed_pub.InitializeMessage();
            gps_pub = gameObject.AddComponent<NavSatFixPublisher>() as NavSatFixPublisher;
            gps_pub.Topic = "/SENSORS/GPS";
            gps_pub.rb = rb;
            gps_pub.pfreq = 20;
            gps_pub.enabled = true;
            gps_pub.Outside_Time_Synchronization = true;
            gps_pub.InitializeMessage();
            firstCall = true;

        
            //rb = GetComponent<Rigidbody>();


        }

        private void FixedUpdate()
        {
            if (firstCall == true){
                firstCall = false;
            } else {
                UpdateMessages();
            }
        }

         private void UpdateMessages()
        {
            Messages.Standard.Time synced = new Messages.Standard.Time();
            float time = Time.realtimeSinceStartup;
            synced.secs = (uint)time;
            synced.nsecs = (uint)(1e9 * (time - synced.secs));
            gps_pub.SendSynchronizedMessage(synced);
            gps_speed_pub.SendSynchronizedMessage(synced);
            imu_pub.SendSynchronizedMessage(synced);

            
        }

    }
}
