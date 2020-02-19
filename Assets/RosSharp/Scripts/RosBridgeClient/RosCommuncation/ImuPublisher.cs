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
using System;

namespace RosSharp.RosBridgeClient
{
    public class ImuPublisher : Publisher<Messages.Sensor.Imu>
    {
        public string FrameId = "Unity";
        public Rigidbody rb;

        public bool Outside_Time_Synchronization=false;
        Vector3 lastPos = Vector3.zero;
        private Messages.Sensor.Imu message;
       
        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!Outside_Time_Synchronization){
                UpdateMessage();
            }
        }

        public void InitializeMessage()
        {
            message = new Messages.Sensor.Imu
            {
                header = new Messages.Standard.Header()
                {
                    frame_id = FrameId
                }
            };
            
        }

        private void UpdateMessage()
        {
            if (!Outside_Time_Synchronization){
                message.header.Update();
                message.orientation = GetGeometryQuaternion(rb.rotation.Unity2Ros());
                message.orientation_covariance = new float[] {1,0,0,0,1,0,0,0,1};
                message.angular_velocity=  GetGeometryVector3(rb.angularVelocity.Unity2Ros());
                message.angular_velocity_covariance = new float[] {1,0,0,0,1,0,0,0,1};
                Vector3 distancePerFrame = rb.position - lastPos;
                lastPos = rb.position;
                Vector3 speed = distancePerFrame * Time.deltaTime;
                message.linear_acceleration=  GetGeometryVector3(speed.Unity2Ros());
                message.linear_acceleration_covariance = new float[] {1,0,0,0,1,0,0,0,1};

                Publish(message);
            }
        }

        public void SendSynchronizedMessage(Messages.Standard.Time synchronized_time)
        {
                Debug.Log("IMU:Send Sync Messages..."+message.header.stamp); //+message.header.stamp);
                message.header.TimeSynchronization(synchronized_time);
                message.orientation = GetGeometryQuaternion(rb.rotation.Unity2Ros());
                message.orientation_covariance = new float[] {1,0,0,0,1,0,0,0,1};
                message.angular_velocity=  GetGeometryVector3(rb.angularVelocity.Unity2Ros());
                message.angular_velocity_covariance = new float[] {1,0,0,0,1,0,0,0,1};
                Vector3 distancePerFrame = rb.position - lastPos;
                lastPos = rb.position;
                Vector3 speed = distancePerFrame * Time.deltaTime;
                message.linear_acceleration=  GetGeometryVector3(speed.Unity2Ros());
                message.linear_acceleration_covariance = new float[] {1,0,0,0,1,0,0,0,1};

                Publish(message);
        }

        private Messages.Geometry.Vector3 GetGeometryVector3(Vector3 position)
        {
            Messages.Geometry.Vector3 geometryVector3 = new Messages.Geometry.Vector3();
            geometryVector3.x = position.x;
            geometryVector3.y = position.y;
            geometryVector3.z = position.z;
            return geometryVector3;
        }

        private Messages.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion)
        {
            Messages.Geometry.Quaternion geometryQuaternion = new Messages.Geometry.Quaternion();
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
            return geometryQuaternion;
        }

    }
}
