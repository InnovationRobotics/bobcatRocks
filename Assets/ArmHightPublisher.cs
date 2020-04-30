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
            float hight=transform.localEulerAngles.y;
           // Debug.Log("HIGHT :" + hight);
            message.data = CalcRange(hight);
            Publish(message);
        }

        private int CalcRange(float val)
        {

            val = 8.1258f * val - 204.375f;
            Debug.Log("Hight:" + (int)val);
            return (int)val;
            

        }


        private void ReayCast()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask))
            {
                Debug.DrawLine(transform.position, hit.point, Color.blue);
                // Debug.Log("distance" + hit.distance);
                var flDistance = hit.distance * 100;
                ValueToPublish = (int)flDistance;
                message.data = ValueToPublish;
                Publish(message);
            }
        }
    }
}
