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

namespace RosSharp.RosBridgeClient
{
    public class RobilShiffonFusion  : MonoBehaviour //Publisher<Messages.Sensor.Imu>
    {
        Transform myref;
        public string FrameId = "Unity";
        public Rigidbody rb;
        Vector3 lastPos = Vector3.zero;
        private Messages.Sensor.Imu imu_msg;
        private Messages.Sensor.NavSatFix gps_speed_msg;
        private Messages.Sensor.NavSatFix gps_msg;

        public ImuPublisher imu_pub;
        public GPSSpeedPublisher gps_speed_pub;
        private float start_latitude, start_longitude, start_altitude;

        public NavSatFixPublisher gps_pub;

        private GameObject me;

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

        
            //rb = GetComponent<Rigidbody>();


        }

        private void FixedUpdate()
        {
            UpdateMessages();
        }

        private void InitializeMessages()
        {
#if SOMETHING_ELSE
            //Initialize Imu

            imu_msg = new Messages.Sensor.Imu
            {
                header = new Messages.Standard.Header()
                {
                    frame_id = FrameId
                }
            };

            //Initialize GPS Speed
            gps_speed_msg = new Messages.Sensor.NavSatFix
            {
                header = new Messages.Standard.Header()
                {
                    frame_id = FrameId
                },
                status = new Messages.Sensor.NavSatStatus()
                {
                    status = (sbyte)Messages.Sensor.NavSatStatus.StatusType.STATUS_FIX,
                    service = (short)Messages.Sensor.NavSatStatus.ServiceType.SERVICE_GPS
                },
                latitude = start_latitude,
                longitude = start_longitude,
                altitude = start_altitude          
            };
            gps_speed_msg.position_covariance = new float[] {1,0,0,0,1,0,0,0,1};
            gps_speed_msg.position_covariance_type=  0;
#endif
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
