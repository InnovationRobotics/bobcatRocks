using System.Collections;
using UnityEngine;


    public class FoucsCameraToTarget : MonoBehaviour
    {

        // The target we are following
        [SerializeField]
        public Transform target;
        // The distance in the x-z plane to the target
        [SerializeField]
        private float distance = 10.0f;
        // the height we want the camera to be above the target
        [SerializeField]
        private float height = 5.0f;
    [SerializeField]
    private float Rotation = 5.0f;

    [SerializeField]
        private float Duration;
        [SerializeField]
  

    public bool LookAt;
    public Transform ExaminedObjects;





    private float lastClickTime = 0;

        float catchTime = .25f;


        public void Center()
        {
            StartCoroutine(center());
        }

        IEnumerator center()
        {
                    transform.position = target.position;
                    var wantedRotationAngle = target.eulerAngles.y + Rotation;
                    var wantedHeight = target.position.y + height;

                    var currentRotationAngle = transform.eulerAngles.y;
                    var currentHeight = transform.position.y;
                    float ElpsedTime = 0;
                    while (ElpsedTime < Duration)
                    {
                        // Damp the rotation around the y-axis
                        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, ElpsedTime / Duration);

                        // Damp the height
                        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, ElpsedTime / Duration);

                        // Convert the angle into a rotation
                        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

                        // Set the position of the camera on the x-z plane to:

                        // distance meters behind the target
                        transform.position = target.position;
                        transform.position -= currentRotation * Vector3.forward * distance;

                        // Set the height of the camera
                        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);


                        // Always look at the target
                        transform.LookAt(target);
                        ElpsedTime += Time.deltaTime;
                        yield return null;
                    }


                 


                
        }


    // Use this for initialization


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < catchTime)
            {
                //double click
                StartCoroutine(center());
            }

            lastClickTime = Time.time;
        }

       

    }
    float tempHight=0;
    private void LateUpdate()
    {
        if(ExaminedObjects==null)
        {
            return;
        }


         if (LookAt)
        {

            if (Input.GetAxis("Mouse ScrollWheel") > 0) distance--;
            if (Input.GetAxis("Mouse ScrollWheel") < 0) distance++;
            distance = Mathf.Clamp(distance, 2, 30);
            // Calculate the current rotation angles
            //var wantedRotationAngle = target.eulerAngles.y;
            //var wantedHeight = target.position.y + height;
            var CarHight = target.position.y;
            var currentRotationAngle = transform.eulerAngles.y;
            var CameraCurrentHight = transform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, currentRotationAngle, 1 * Time.deltaTime);

            // Damp the height
            
             CameraCurrentHight=Mathf.Lerp(CameraCurrentHight, CarHight+Vector3.Distance(transform.position,target.position)/2,Time.deltaTime);
             CameraCurrentHight=Mathf.Clamp(CameraCurrentHight,CarHight+Vector3.Distance(transform.position,target.position)/2,CameraCurrentHight);
           //  Debug.Log("HIGHT  "+CarHight );
           
            
           
             
                     

             // Height=Mathf.Clamp(Height,Height,currentHeight+10);
            
      

            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position= new Vector3(target.position.x, CameraCurrentHight , target.position.z);
            transform.position -= currentRotation * Vector3.forward * distance;

            // Set the height of the camera
           
                // CameraCurrentHight = Mathf.Lerp(CameraCurrentHight, CarHight+CameraCurrentHight, 1 * Time.deltaTime);
         
                 
            
                
         

            // Always look at the target
         //   transform.LookAt(target);
        }
    }

    public void IsLookAt()
        {
            LookAt = !LookAt;
        }
    }



