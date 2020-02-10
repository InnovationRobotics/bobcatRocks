/*
© CentraleSupelec, 2017
Author: Dr. Jeremy Fix (jeremy.fix@centralesupelec.fr)

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

// Adjustments to new Publication Timing and Execution Framework
// © Siemens AG, 2018, Dr. Martin Bischoff (martin.bischoff@siemens.com)

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class Float64Subscriber : Subscriber<Messages.Standard.Float64>
    {
        
 //       public TankDriver TankDriver;

        private float previousRealTime;
        public float whatever, rate;
        private bool isMessageReceived;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(Messages.Standard.Float64 message)
        {
            whatever = (float)message.data;
            Debug.Log("ZZZZZ Got ThrottleWhatever=");
            isMessageReceived = true;
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();        

        }
        private void ProcessMessage()
        {
            /**** Not used - Move Robil is used instead
            float deltaTime = Time.realtimeSinceStartup - previousRealTime;
            rate = whatever * deltaTime;
            previousRealTime = Time.realtimeSinceStartup;
            //Debug.Log("Rate="+rate.ToString());
            isMessageReceived = false;
            if (Topic == "/LLC/EFFORTS/Throttle"){
                      TankDriver.SetThrottle(rate); 
                      //Debug.Log("Rate="+rate.ToString());
            } 
            else TankDriver.SetSteer(rate);   
            */
        }
    }
}