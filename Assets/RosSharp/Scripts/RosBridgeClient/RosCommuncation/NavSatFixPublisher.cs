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
    public class NavSatFixPublisher : Publisher<Messages.Sensor.NavSatFix>
    {
const double  CENTER_X_NS = 31.2622f;		// GPS coordinates
const double  CENTER_Y_EW = 34.803611f;	// of lab 320
const float  DEGREE_TO_M = 111000; 		//1 degree has appprox. 111km
const double PI  =3.141592653589793238463;

        public Rigidbody rb;

        public string FrameId = "Unity";
        public int pfreq = 20;
        public bool Outside_Time_Synchronization=false;
        Vector3 _init_pos = Vector3.zero;
        private Messages.Sensor.NavSatFix message;
        private float start_latitude, start_longitude, start_altitude;
        private double stLatRad, stLonRad, LatRad, LonRad;
        private Vector3 tmpPos = Vector3.zero;
        private double other_dist, dist, brng, R;
        private sbyte test;


        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            start_latitude =32.0017549051f;// (float)CENTER_X_NS;//31.2622f; //32.0017549051;
            start_longitude = 34.9083870312f; //(float)CENTER_Y_EW; //34.803611f; //34.9083870312;           
            start_altitude = 2.0f;
            _init_pos = rb.position;
        }

        private void FixedUpdate()
        {
          //  if (Time.deltaTime <1.0/pfreq) return;

            UpdateMessage();
        }

        public void InitializeMessage()
        {
            message = new Messages.Sensor.NavSatFix
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
            message.position_covariance = new float[] {1,0,0,0,1,0,0,0,1};
            message.position_covariance_type=  0;
        }

        private void UpdateMessage()
        {
             if (!Outside_Time_Synchronization){
                        message.header.Update();
         
                        //Compute current coordinates
                        tmpPos = rb.position;
                        other_dist = (tmpPos - _init_pos).magnitude;
                        dist = Math.Sqrt((tmpPos.x-_init_pos.x)*(tmpPos.x-_init_pos.x)+(tmpPos.y-_init_pos.y)*(tmpPos.y-_init_pos.y));
                        // if ((tmpPos.magnitude*_init_pos.magnitude)==0.0f) brng = Math.Atan2(tmpPos.x,tmpPos.y);
                        // else 
                        brng = Math.Atan2(tmpPos.y - _init_pos.y, tmpPos.x - _init_pos.x);
                        //acos(pos.Dot(_init_pos)/(pos.GetLength()*_init_pos.GetLength()));
                        brng *= 1; //not clear what this statement does!
                            
                        R = 6378.1*1000;
                        message.altitude = tmpPos.z;

                        stLatRad = start_latitude*PI/180; 
                        LatRad = Math.Asin(Math.Sin(stLatRad)*Math.Cos(dist/R)+Math.Cos(stLatRad)*Math.Sin(dist/R)*Math.Cos(brng));
                        message.latitude = (float)(LatRad*180/PI); //Degrees
		
                        LonRad = Math.Atan2(Math.Sin(brng)*Math.Sin(dist/R)*Math.Cos(stLatRad),Math.Cos(dist/R)-Math.Sin(stLatRad)*Math.Sin(LatRad*PI/180));
                        message.longitude = start_longitude + (float) (LonRad*180/PI); //Degrees
                        Publish(message);
             }
        }

        public void SendSynchronizedMessage(Messages.Standard.Time synchronized_time)
        {
            Debug.Log("GPS:Send Sync Messages..."); //+message.header.stamp);

            message.header.TimeSynchronization(synchronized_time);
            //Compute current coordinates
             tmpPos = rb.position;
             other_dist = (tmpPos - _init_pos).magnitude;
             dist = Math.Sqrt((tmpPos.x-_init_pos.x)*(tmpPos.x-_init_pos.x)+(tmpPos.y-_init_pos.y)*(tmpPos.y-_init_pos.y));
             // if ((tmpPos.magnitude*_init_pos.magnitude)==0.0f) brng = Math.Atan2(tmpPos.x,tmpPos.y);
             // else 
             brng = Math.Atan2(tmpPos.y - _init_pos.y, tmpPos.x - _init_pos.x);
             //acos(pos.Dot(_init_pos)/(pos.GetLength()*_init_pos.GetLength()));
             brng *= 1; //not clear what this statement does!
                 
             R = 6378.1*1000;
             message.altitude = tmpPos.z;

             stLatRad = start_latitude*PI/180; 
             LatRad = Math.Asin(Math.Sin(stLatRad)*Math.Cos(dist/R)+Math.Cos(stLatRad)*Math.Sin(dist/R)*Math.Cos(brng));
             message.latitude = (float)(LatRad*180/PI); //Degrees
		
             LonRad = Math.Atan2(Math.Sin(brng)*Math.Sin(dist/R)*Math.Cos(stLatRad),Math.Cos(dist/R)-Math.Sin(stLatRad)*Math.Sin(LatRad*PI/180));
             message.longitude = start_longitude + (float) (LonRad*180/PI); //Degrees
             Publish(message);
        }


    }
}