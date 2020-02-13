using System.Collections;
using UnityEngine;


public class focusCamera : MonoBehaviour
{

    // The target we are following

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
        transform.position = ExaminedObjects.position;
        var wantedRotationAngle = Rotation;
        var wantedHeight = height;

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
            transform.position = ExaminedObjects.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);


            // Always look at the target
            transform.LookAt(ExaminedObjects);
            ElpsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();


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

        if (LookAt)
        {
            transform.LookAt(ExaminedObjects);
        }

    }

    public void IsLookAt()
    {
        LookAt = !LookAt;
    }
}



