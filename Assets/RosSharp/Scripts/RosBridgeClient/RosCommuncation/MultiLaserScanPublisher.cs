/*
© Siemens AG, 2018
Author: Berkay Alp Cakal (berkay_alp.cakal.ct@siemens.com)

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
    public class MultiLaserScanPublisher : Publisher<Messages.Robil.MultiLaserScan>
    {
        public LaserScanReader laserScanReader_t1;
        public LaserScanReader laserScanReader_t2;
        public LaserScanReader laserScanReader_b1;
        public LaserScanReader laserScanReader_b2;

        public string FrameId = "Unity";
        
        private Messages.Robil.MultiLaserScan message;
        public int Samples;  
        public float Update_Rate = 500; //12.5 Hz
        private float ScanPeriod;
        private float previousScanTime = 0;
    
                
        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            //Debug.Log("FixedUpdate has been called  ");
            if (Time.realtimeSinceStartup >= previousScanTime + ScanPeriod)
            {
                UpdateMessage();
                previousScanTime = Time.realtimeSinceStartup;
            }
        }

        private void InitializeMessage()
        {
           // Debug.Log("InitializeMessage has been called ");

            ScanPeriod = Samples / Update_Rate;
            Debug.Log("InitializeMessage has been called , ScanPeriod = "+ScanPeriod.ToString());

            message = new Messages.Robil.MultiLaserScan
            {
                header = new Messages.Standard.Header { frame_id = FrameId },
                angle_min_t       = -0.77667f, //min_ang = -44.5deg
                angle_max_t       = 0.44505f,  //max_ang = 25.5deg
                angle_min_b       = -0.78534f, //=-45deg
                angle_max_b       = 0.43633f,  ////=25deg
                angle_increment = -0.0174533f, //<!-- 1.0deg=0.0174533rad    
                                                // resolution of single row is 1deg in combination we get 0.5deg -->
                angle_t1          = (float) -0.006981,  //row_t1_pitch_ang
                angle_t2          = (float) -0.020944,  //row_t2_pitch_ang
                angle_b1          = 0.006981f,   //row_b1_pitch_ang
                angle_b2          = 0.020944f,   //row_b2_pitch_ang
                //??time_increment  = laserScanReader.time_increment,
                range_min       = 0.3f, //=0.3m laserScanReader.range_min,
                range_max       = 30f, //=30m laserScanReader.range_max,
                ranges_t1          = laserScanReader_t1.ranges,
                ranges_t2          = laserScanReader_t2.ranges,
                ranges_b1          = laserScanReader_b1.ranges,
                ranges_b2          = laserScanReader_b2.ranges,      
                intensities     = new float[Samples]
            };
            laserScanReader_t1.angle_min = -0.77667f;
            laserScanReader_t2.angle_min = -0.77667f;
            laserScanReader_b1.angle_min = -0.78534f;
            laserScanReader_b2.angle_min = -0.78534f;

            laserScanReader_t1.angle_max = 0.44505f;
            laserScanReader_t2.angle_max = 0.44505f;
            laserScanReader_b1.angle_max = 0.43633f;
            laserScanReader_b2.angle_max = 0.43633f;

            laserScanReader_t1.angle_increment = laserScanReader_t2.angle_increment = -0.0174533f;
            laserScanReader_b2.angle_increment = laserScanReader_b2.angle_increment = -0.0174533f;
            laserScanReader_t1.range_min = laserScanReader_t2.range_min = 0.3f;
            laserScanReader_b1.range_min = laserScanReader_b2.range_min = 0.3f;
            laserScanReader_t1.range_max = laserScanReader_t2.range_max = 30f;
            laserScanReader_b1.range_max = laserScanReader_b2.range_max = 30f;
        }

        private void UpdateMessage()
        {
            Debug.Log("UpdateMessage has been called ");
            message.header.Update();
            message.ranges_t1 = laserScanReader_t1.Scan();
            message.ranges_t2 = laserScanReader_t2.Scan();
            message.ranges_b1 = laserScanReader_b1.Scan();
            message.ranges_b2 = laserScanReader_b2.Scan();
            message.time_increment = Time.deltaTime;
            message.scan_time = Time.deltaTime;
            Publish(message);
        }
    }
}
