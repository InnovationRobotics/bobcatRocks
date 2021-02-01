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

using AGXUnity.Model;

namespace RosSharp.RosBridgeClient
{
    public class EngineThrottlePublisher : Publisher<MessageTypes.Std.Float64>
    {
        public int ValueToPublish = 0;
        private WheelLoader wheelLoader;
        //public float Rate;

        private MessageTypes.Std.Float64 message;

        protected override void Start()
        {
            base.Start();
            wheelLoader = GetComponent<WheelLoader>();
            InitializeMessage();
        }

        private void Update()
        {
            UpdateMessage();
        }



        private void InitializeMessage()
        {
            message = new MessageTypes.Standard.Float64();
            message.data = wheelLoader.Engine.getThrottle(); 
        }

        private void UpdateMessage()
        {
            message.data = wheelLoader.Engine.getThrottle();
            Publish(message);
        }
    }
}
