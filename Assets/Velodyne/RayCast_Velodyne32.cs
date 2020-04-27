using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


[System.Serializable]
public class RayCast_Velodyne32 : MonoBehaviour
{
    //Todo for multuiplatform ifdef 
    [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    private static extern void GetSystemTimePreciseAsFileTime(out long filetime);
    long filetime;
    Rigidbody rb;
    public Transform SensorRotator, emitter;
    VelodyneWrapper vel32ICDinterface;
    public string ICD_ConfigFile;
    private string m_JsonString;
    public string VelodyneDataFilePath;
    private string m_jsonstring;
    public VelodyneConfig ConfigRef;

    public float horizontalFOV, verticalFOV;
    public bool DrawLidar = false; // for digug
    public bool DrawLidarDebug = false; // for digug
                                        //Private Vars
    float horCurrentAngle;
    int m_ResWidth, m_ResHeight;
    public int m_ColumnsPerPhysStep;
    float m_VerticalAngularResolution;
    private int m_BlocksCounter = 0;
    private NativeArray<RaycastHit> results;
    private NativeArray<RaycastCommand> commands;
    double ts;
    private float[] Azimuts;

    private float _rotationAngle;
    private float _minAngle;
    private float _maxAngle;
    private bool _sendDataOnIcd;
    private int _channels;
    private float _lowerAngle;
    private float _measurementRange;
    private float _angularResolution;
    private int _blocksOnPacket;
    bool FirstRun = true, start = true;
    float[] CurChannelHits;
    float[] NextChannelHits;
    //public List<VelodyineRuntimePreview> PreviewTransformList = new List<VelodyineRuntimePreview>();
    //VelodyineRuntimePreview PreviewTransform = new VelodyineRuntimePreview();
    float StartTime, curTime;
    DateTime st;

    void Awake()
    {
        var GoInstance = GameManager.Instance;
        VelodyneDataFilePath = FileFinder.Find(Application.streamingAssetsPath, "Velodyne" + ".json");  //todo Set In Outside File
                                                                                                          //   ICD_ConfigFile = GoInstance.PathsAndConfigRef.PathToLoadVelodyneConfig;  //todo Set In Outside File
                                                                                                          //json parser ***all data commeing from json file 
        m_JsonString = File.ReadAllText(VelodyneDataFilePath);
        VelodyneConfig velor = JsonConvert.DeserializeObject<VelodyneConfig>(m_JsonString);
        ConfigRef = velor;
    }


    void Start()
    {

        CurChannelHits = new float[ConfigRef.Channels];
        NextChannelHits = new float[ConfigRef.Channels];
        // Calculation of FOV 
        verticalFOV = ConfigRef.HigherAngle - ConfigRef.LowerAngle;
        m_VerticalAngularResolution = verticalFOV / (ConfigRef.Channels - 1f);
        //horizontalFOV = Time.fixedDeltaTime * ConfigRef.RotationAngle * ConfigRef.RotateFrequency / ConfigRef.SuperSample; 
        horizontalFOV = Time.fixedDeltaTime * (ConfigRef.MaxAngle - ConfigRef.MinAngle) * ConfigRef.RotateFrequency / ConfigRef.SuperSample;
        // target Texture size calculation 
        m_ColumnsPerPhysStep = Mathf.RoundToInt(horizontalFOV / ConfigRef.AngularResolution); //From Mor Code
        Debug.Log("m_ColumnsPerPhysStep " + m_ColumnsPerPhysStep + "horizontalFOV " + horizontalFOV + " m_VerticalAngularResolution " + m_VerticalAngularResolution);                                                                                     // m_ColumnsPerPhysStep = Mathf.RoundToInt(Time.fixedDeltaTime * ConfigRef.RotationAngle * ConfigRef.RotateFrequency / ConfigRef.AngularResolution) / ConfigRef.SuperSample;
        m_ResWidth = m_ColumnsPerPhysStep;
        //  Debug.Log("m_ResWidth: "+ m_ResWidth);
        m_ResHeight = ConfigRef.Channels;
        ConfigRef.CameraRotationAngle = horizontalFOV / 2.0f;
        results = new NativeArray<RaycastHit>(m_ResWidth * ConfigRef.Channels, Allocator.TempJob);
        commands = new NativeArray<RaycastCommand>(m_ResWidth * ConfigRef.Channels, Allocator.TempJob);
    }

    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {

        #region Load ConfigRef Data
        _rotationAngle = ConfigRef.RotationAngle;
        _minAngle = (ConfigRef.MinAngle < 0) ? 360 + ConfigRef.MinAngle : ConfigRef.MinAngle;
        _maxAngle = (ConfigRef.MaxAngle < 0) ? 360 + ConfigRef.MaxAngle : ConfigRef.MaxAngle; ;
        _sendDataOnIcd = ConfigRef.SendDataOnIcd;
        _channels = ConfigRef.Channels;
        _lowerAngle = ConfigRef.LowerAngle;
        _measurementRange = ConfigRef.MeasurementRange;
        _angularResolution = ConfigRef.AngularResolution;
        _blocksOnPacket = ConfigRef.BlocksOnPacket;
        #endregion

        horCurrentAngle = _minAngle;

        // activtion of the ICD interface    
        if (_sendDataOnIcd)
        {
            Debug.Log(ConfigRef.Ip);
            vel32ICDinterface = new VelodyneWrapper(ConfigRef.Ip, ConfigRef.Port, ConfigRef.ReturnMode, ConfigRef.DataSource, true);
            //vel32ICDinterface = new VelodyneWrapper(ICD_ConfigFile, false);
        }
    }

    private void OnDisable()
    {
        vel32ICDinterface.Dispose();
    }

    void FixedUpdate()
    {
        curTime = Time.realtimeSinceStartup;
        StartTime = Time.realtimeSinceStartup;
        VelodyneCalcAndSendDataMultiFull360();
    }

    private void OnApplicationQuit()
    {
        vel32ICDinterface.Dispose();
    }



    private void VelodyneCalcAndSendDataMultiFull360()
    {

        Azimuts = new float[m_ResWidth];

        //if (horCurrentAngle >= _rotationAngle)
        if (horCurrentAngle >= _maxAngle && horCurrentAngle < _minAngle)
        { //completed horizontal scan
            horCurrentAngle = _minAngle;
            SensorRotator.localEulerAngles = new Vector3(0, _minAngle, 0);
        }
        if (horCurrentAngle >= 360)
        { //completed horizontal scan
            horCurrentAngle = 0;
            SensorRotator.localEulerAngles = new Vector3(0, 0, 0);
        }

        int RayCastNum = 0; //    Vector3 ScannerVel = myref.InverseTransformVector(rb.velocity);
        Vector3 scanerPos = SensorRotator.position;//  Vector3 ScanerLinearStep = ScannerVel * Time.fixedTime;

        for (int i = 0; i < m_ResWidth; i++)
        {
            //Debug.Log("horCurrentAngle " + horCurrentAngle + " max " + _maxAngle + " min " + _minAngle);
            if (horCurrentAngle >= _maxAngle && horCurrentAngle < _minAngle)
            {
                horCurrentAngle = _minAngle;
                SensorRotator.localEulerAngles = new Vector3(0, _minAngle, 0);
            }
            if (horCurrentAngle >= 360)
            {
                horCurrentAngle = 0;
                SensorRotator.localEulerAngles = new Vector3(0, 0, 0);
            }
            //Debug.Log("horCurrentAngle " + horCurrentAngle);
            for (int j = 0; j < _channels; j++)
            { //the lazer column 

                float verCurentAngle = (_lowerAngle + j * m_VerticalAngularResolution);

                /* emitter.localEulerAngles = new Vector3(-verCurentAngle, 0, 0);*///rotation around x for the layers


                var dir = SensorRotator.rotation * Quaternion.Euler(-verCurentAngle, 0, 0) * Vector3.forward;//return the z axis of the emitter related to the world because this is the direction of the ray

                commands[RayCastNum] = new RaycastCommand(scanerPos, dir, _measurementRange);

                if (DrawLidarDebug)
                    Debug.DrawRay(scanerPos, dir * _measurementRange, Color.green);

                RayCastNum++;

            }

            Azimuts[i] = horCurrentAngle;

            horCurrentAngle = horCurrentAngle + _angularResolution;
            SensorRotator.localEulerAngles = new Vector3(0, horCurrentAngle, 0);
        }


        // Schedule the batch of raycasts
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 8, default(JobHandle));// m_ResWidth * ConfigRef.Channels
        // Wait for the batch processing job to complete
        handle.Complete();

        // commands.Dispose();

        int azimuthLoc = 0;

        if (_sendDataOnIcd)
        {

            for (int loc = 0; loc < results.Length; loc++)
            {
                var raycastHit = results[loc];

                if (raycastHit.collider != null)
                    vel32ICDinterface.SetChannel(raycastHit.distance, 0);
                else
                {
                    vel32ICDinterface.SetChannel(0, 0);
                }

                if ((loc + 1) % _channels == 0)
                {
                    var horAngle = Azimuts[azimuthLoc];
                    azimuthLoc++;

                    vel32ICDinterface.SetAzimuth(horAngle);
                    GetSystemTimePreciseAsFileTime(out filetime);
                    ts = DateTime.FromFileTime(filetime).Minute * 60f;
                    var str = DateTime.FromFileTime(filetime).ToString("ss.ffffff");

                    Double.TryParse(str, out double tempSec);

                    ts += tempSec;

                    //test
                    //st = DateTime.FromFileTime(filetime);
                    //ts = (st - TankDriver.StartTime).Days * 3600f * 24f + (st - TankDriver.StartTime).Hours * 3600f + (st - TankDriver.StartTime).Minutes * 60f;
                    //var sec = (st - TankDriver.StartTime).ToString(@"ss\.ffffff");
                    //Double.TryParse(sec, out double temp);
                    //ts += temp;

                    vel32ICDinterface.SetTimeStamp(ts);
                    //Debug.Log(ts);
                    vel32ICDinterface.CloseBlock();

                    if (m_BlocksCounter == _blocksOnPacket)
                    {
                        //FOR TEST synchronization
                        Vector3 ang = gameObject.transform.eulerAngles;
                        vel32ICDinterface.TestOrientation(ang.y, angShift(ang.x), angShift(ang.z));
                        //
                        vel32ICDinterface.SendData();
                        m_BlocksCounter = 0;
                    }

                    m_BlocksCounter++;
                }

            }


        }

        Azimuts = null;



    }


    private void VelodyneCalcAndSendDataWithInterpulation()
    {
        float NextRes = 0;
        RaycastHit hit;
        Vector3 scanerPos = transform.position;
        #region DrawLidar

        //if (DrawLidar && PreviewTransformList.Count > 0)
        //{
        //    for (int i = PreviewTransformList.Count - 1; i >= 0; i--)
        //    {
        //        if (Time.unscaledTime - PreviewTransformList[i].Timestamp > drawTime)
        //        {
        //            PreviewTransformList.RemoveAt(i);
        //        }
        //        else
        //        {
        //            DrawLiderPreviewSegment(PreviewTransformList[i].transformList);
        //        }
        //    }
        //}

        #endregion
        //we only raycast the even horizontal angle rays, and set the odd angle rays, to the mid of the prev and next horizontal angle rays. 
        m_ResWidth = Mathf.RoundToInt(horizontalFOV / ConfigRef.AngularResolution / 2);

        //at the first time of the function calculate the rays array of the first horizontal angle 
        if (FirstRun)
        {
            for (int j = 0; j <= ConfigRef.Channels - 1; j++)
            { //the lazer column 
                float verCurentAngle = (ConfigRef.LowerAngle + j * m_VerticalAngularResolution);
                NextRes = 0;
                if (DrawLidarDebug)
                {
                    //we will only draw lines of the even horizontal angles
                    //Debug.DrawLine(CurChannelHits[j].point, CurChannelHits[j].point + 0.1f * Vector3.up, Color.red, drawTime, true);
                }
                //rotation around x for the layers
                emitter.localEulerAngles = new Vector3(-verCurentAngle, 0, 0);
                if (Physics.Raycast(scanerPos, emitter.TransformDirection(Vector3.forward), out hit, ConfigRef.MeasurementRange))
                {
                    NextRes = hit.distance;
                }
                NextChannelHits[j] = NextRes;
            }
            FirstRun = false;
        }


        //if (DrawLidar)
        //{
        //    PreviewTransform = new VelodyineRuntimePreview();
        //    PreviewTransform.Timestamp = Time.unscaledTime;
        //    PreviewTransform.transformList = new List<Matrix4x4>();
        //}

        if (horCurrentAngle > ConfigRef.RotationAngle)
        { //completed horizontal scan
            horCurrentAngle = 0;
            SensorRotator.localEulerAngles = new Vector3(0, 0, 0);
        }
        //Vector3 ScannerVel = myref.InverseTransformVector(rb.velocity);
        //  Vector3 ScanerLinearStep = ScannerVel * Time.fixedTime;

        // multiple horizontal scans in 1 physics step in order to achieve the full range in the desired rate
        for (int i = 0; i < m_ResWidth; i++)
        {
            //code for stopping the function when fixed time is over
            curTime = Time.realtimeSinceStartup;
            if (curTime - StartTime >= 0.005)
            {
                //Debug.Log("number of rays " + i + " time passed " + (curTime - StartTime));
                break;
            }
            //Debug.Log("horCurrentAngle " + horCurrentAngle + " m_ResWidth" + m_ResWidth);

            //if (InterpolateLocation)// check this***************
            //{
            //    scanerPos = scanerPos + ScanerLinearStep * i / m_ResWidth;
            //}

            if (horCurrentAngle > ConfigRef.RotationAngle)
            {
                horCurrentAngle = 0;
                SensorRotator.localEulerAngles = new Vector3(0, 0, 0);
            }
            /*
                for each even horizontal angle, we send the vertical rays (calculated in the prev step), we calculate and save the next even
                horizontal angle vertical rays, and calculate the next odd horizontal angle vertical rays with interpulation of the even 
                horizontal angle vertical rays.
            */
            for (int j = 0; j <= ConfigRef.Channels - 1; j++)
            { //the lazer column 
                float verCurentAngle = (ConfigRef.LowerAngle + j * m_VerticalAngularResolution);

                NextRes = 0;
                //set channel with the currunt ray information
                vel32ICDinterface.SetChannel(CurChannelHits[j], 0);
                if (DrawLidarDebug)
                {
                    //we will only draw lines of the even horizontal angles
                    //Debug.DrawLine(CurChannelHits[j].point, CurChannelHits[j].point + 0.1f * Vector3.up, Color.red, drawTime, true);
                }
                //rotation around x for the layers and calculate and save next even angle ray
                emitter.localEulerAngles = new Vector3(-verCurentAngle, 2 * ConfigRef.AngularResolution, 0);
                if (Physics.Raycast(scanerPos, emitter.TransformDirection(Vector3.forward), out hit, ConfigRef.MeasurementRange))
                {
                    NextRes = hit.distance;
                }
                NextChannelHits[j] = NextRes;
                if (verCurentAngle < 0 && CurChannelHits[j] != 0)
                {
                    float distance = CurChannelHits[j] - 3.2f / Mathf.Sin((-verCurentAngle) * Mathf.PI / 180.0f);
                    float angleDif = -verCurentAngle - Mathf.Asin(3.2f / CurChannelHits[j]) * 180 / Mathf.PI;
                    //Debug.Log("vertical angle " + verCurentAngle + " horizontal angle " + horCurrentAngle + " velodyne distance " + CurChannelHits[j] + " distance diff " + distance + " angle diff " + angleDif + " d9 x rotation " + transform.eulerAngles.x + " laser sensor rotation " + SensorRotator.eulerAngles.x + " laser sensor local rotation " + SensorRotator.localEulerAngles.x);

                }
                else
                {
                    Debug.Log(NextRes);
                }
            }
            //send current angle channel data
            VelodyneCalcAndSendChannelData();
            //calculate next odd horizontal angle vertical rays
            horCurrentAngle = horCurrentAngle + ConfigRef.AngularResolution;
            if (horCurrentAngle > ConfigRef.RotationAngle)
            { //completed horizontal scan
                horCurrentAngle = 0;
                SensorRotator.localEulerAngles = new Vector3(0, 0, 0);
                for (int j = 0; j <= ConfigRef.Channels - 1; j++)
                {
                    CurChannelHits[j] = NextChannelHits[j];
                }
            }
            else
            {
                for (int j = 0; j <= ConfigRef.Channels - 1; j++)
                { //the lazer column
                    vel32ICDinterface.SetChannel((CurChannelHits[j] + NextChannelHits[j]) / 2, 0);
                    CurChannelHits[j] = NextChannelHits[j];
                }
                //send current angle channel data
                VelodyneCalcAndSendChannelData();
                horCurrentAngle = horCurrentAngle + ConfigRef.AngularResolution;
                SensorRotator.localEulerAngles = new Vector3(0, horCurrentAngle, 0);
            }

        }
    }

    private void VelodyneCalcAndSendChannelData()
    {
        vel32ICDinterface.SetAzimuth(horCurrentAngle);
        GetSystemTimePreciseAsFileTime(out filetime);
        ts = DateTime.FromFileTime(filetime).Minute * 60f;
        var str = DateTime.FromFileTime(filetime).ToString("ss.ffffff");

        Double.TryParse(str, out double tempSec);

        ts += tempSec;
        vel32ICDinterface.SetTimeStamp(ts);
        vel32ICDinterface.CloseBlock();
        m_BlocksCounter++;
        if (m_BlocksCounter == ConfigRef.BlocksOnPacket)
        {
            vel32ICDinterface.SendData();
            m_BlocksCounter = 0;
        }
    }

    float angShift(float ang)   // invert the direction of ang, ad shift it from [0 , 360] to [-180 180]
    {
        if (ang > 180)
            return (360 - ang);
        else
            return (-ang);
    }

}



