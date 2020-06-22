using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class ArmHightPublisher : Publisher<Messages.Standard.Int32>
    {
        public int ValueToPublish = 0;
        //public float Rate;

        private Messages.Standard.Int32 message;
        // private Transform Parent;
        public LayerMask LayerMask;
        public Transform origin;
        public Transform destination;

        protected override void Start()
        {
            base.Start();
            //  Parent = FindObjectOfType<TankDriver>().transform;
            InitializeMessage();
        }

        void FixedUpdate()
        {
            UpdateMessage();
        }



        private void InitializeMessage()
        {
            message = new Messages.Standard.Int32();

        }

        private void UpdateMessage()
        {

            //   down 43-->  145
            //   UP   59 --> 275

            // ReayCast(); //Dont Need raycast any more

            //we gonna use the Y Rotation to measure the hight
            float hight = ReayCast();
            // Debug.Log("HIGHT :" + hight);
            message.data = CalcRange(hight);
            Debug.Log("Hight:" + message.data);
            Publish(message);
        }

        private int CalcRange(float val)
        {

            // val = 8.1258f * val - 204.375f;
            val = 223.0993f * val -307.4455f;
          
            return (int)val;


        }


        private float ReayCast()
        {
            //RaycastHit hit;

          
            Debug.DrawLine(origin.position, destination.position, Color.blue);
          

            float ValueToPublish = Vector3.Distance(origin.position, destination.position);
          
            return ValueToPublish;
        }
    }
}