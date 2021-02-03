/*
© Siemens AG, 2017-2019
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

using System.Collections.Generic;
using UnityEngine;
namespace RosSharp.RosBridgeClient
{
    public class JointStateSubscriberNoURDF : Subscriber<MessageTypes.Sensor.JointState>
    {
        public List<string> JointNames;
        //public List<JointStateWriter> JointStateWriters;

        public BobcatArm bobcatArm;

        protected override void ReceiveMessage(MessageTypes.Sensor.JointState message)
        {
            int index;
            for (int i = 0; i < message.name.Length; i++)
            {
                Debug.Log("Dealing with="+message.name[i]);
                index = JointNames.IndexOf(message.name[i]);
                
                if (index != -1) {
                   // JointStateWriters[index].Write((float) message.position[i]);
                   if (message.name[i] == "arm") 
                            bobcatArm.MoveArm((float)message.position[i]);
                   if (message.name[i] == "loader")
                            bobcatArm.MoveLoader((float)message.position[i]);
                   if (message.name[i] == "bracket")
                            bobcatArm.MoveBracket((float)message.position[i]);
                } 
            }
        }
    }
}

