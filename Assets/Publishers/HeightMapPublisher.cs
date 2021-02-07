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
    public class HeightMapPublisher : Publisher<MessageTypes.Std.Float64MultiArray>
    {
        public int ValueToPublish = 0;
        private MapHightManager mapHightManager;
        //public float Rate;

        private MessageTypes.Std.Float64MultiArray message;

        protected override void Start()
        {
            base.Start();

            mapHightManager = GetComponent<MapHightManager>();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            UpdateMessage();
        }



        private void InitializeMessage()
        {
            message = new MessageTypes.Std.Float64MultiArray();
            message.data = new double[(int)mapHightManager.size];
        }

        private void UpdateMessage()
        {
            mapHightManager.TerrainHeights.CopyTo(message.data);
            Publish(message);
        }
    }
}
