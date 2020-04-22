using System.Collections.Generic;
using UnityEngine;
using System;

using Newtonsoft.Json;



[Serializable]
public class VelodyneConfig
{
    [SerializeField] private string ip;
    [SerializeField] private string port;
    [SerializeField] private string returnMode;
    [SerializeField] private string dataSource;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float rotateFrequency;
    [SerializeField] private float angularResolution;
    [SerializeField] private float lowerAngle;
    [SerializeField] private float higherAngle;
    [SerializeField] private float rotationAngle;
    [SerializeField] private int channels;
    [SerializeField] private int superSample;
    [SerializeField] private float measurementRange;
    [SerializeField] private float minMeasurementRange;
    [SerializeField] private float measurementAccuracy;
    [SerializeField] private float cameraRotationAngle;
    [SerializeField] private int blocksOnPacket;
    [SerializeField] private bool sendDataOnICD;
    [SerializeField] private bool rotate;

    [JsonIgnore]

    public List<VelodyneFrame> FramesQueue = new List<VelodyneFrame>();


    public float RotateFrequency
    {
        get { return rotateFrequency; }
        set { rotateFrequency = value; }
    }

    public float AngularResolution
    {
        get { return angularResolution; }
        set { angularResolution = value; }
    }

    public float LowerAngle
    {
        get { return lowerAngle; }
        set { lowerAngle = value; }
    }

    public float HigherAngle
    {
        get { return higherAngle; }
        set { higherAngle = value; }
    }

    public float RotationAngle
    {
        get { return rotationAngle; }
        set { rotationAngle = value; }
    }

    public int Channels
    {
        get { return channels; }
        set { channels = value; }
    }

    public int SuperSample
    {
        get { return superSample; }
        set { superSample = value; }
    }

    public float MeasurementRange
    {
        get { return measurementRange; }
        set { measurementRange = value; }
    }

    public float MinMeasurementRange
    {
        get { return minMeasurementRange; }
        set { minMeasurementRange = value; }
    }

    public float MeasurementAccuracy
    {
        get { return measurementAccuracy; }
        set { measurementAccuracy = value; }
    }

    public float CameraRotationAngle
    {
        get { return cameraRotationAngle; }
        set { cameraRotationAngle = value; }
    }

    public int BlocksOnPacket
    {
        get { return blocksOnPacket; }
        set { blocksOnPacket = value; }
    }

    public bool SendDataOnIcd
    {
        get { return sendDataOnICD; }
        set { sendDataOnICD = value; }
    }

    public bool Rotate
    {
        get { return rotate; }
        set { rotate = value; }
    }

    public float MaxAngle
    {
        get { return maxAngle; }
        set { maxAngle = value; }
    }

    public float MinAngle
    {
        get { return minAngle; }
        set { minAngle = value; }
    }

    public string Ip
    {
        get { return ip; }
        set { ip = value; }
    }

    public string Port
    {
        get { return port; }
        set { port = value; }
    }

    public string ReturnMode
    {
        get { return returnMode; }
        set { returnMode = value; }
    }

    public string DataSource
    {
        get { return dataSource; }
        set { dataSource = value; }
    }
}



