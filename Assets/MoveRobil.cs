using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RosSharp.RosBridgeClient
{
public class MoveRobil : MonoBehaviour {

	public TankDriver TankDriver;
	public Float64Subscriber Float64SubThrottle;
	public Float64Subscriber Float64SubSteering;

		bool firstTime = true;
	// Use this for initialization
	void Start () 
	{
		//The attribute has been assigned in the scene so shouldn't be here
		//Float64SubscriberThrottle=GetComponent<Float64Subscriber>();
		//Float64SubscriberSteering=GetComponent<Float64Subscriber>();
		    Float64SubThrottle = gameObject.AddComponent<Float64Subscriber>() as Float64Subscriber;
            Float64SubThrottle.Topic = "/LLC/EFFORTS/Throttle";
            Float64SubThrottle.enabled = true;
			Float64SubThrottle.TimeStep = 0.1f;
			Float64SubThrottle.whatever= 0.0f;
			Float64SubThrottle.rate=0.0f;
            //Float64SubThrottle.Outside_Time_Synchronization = true;
            //Float64SubThrottle.InitializeMessage();
            Float64SubSteering = gameObject.AddComponent<Float64Subscriber>() as Float64Subscriber;
			Float64SubSteering.Topic = "/LLC/EFFORTS/Steering";
            Float64SubSteering.enabled = true;
			Float64SubSteering.TimeStep = 0.1f;
			Float64SubSteering.whatever= 0.0f;
			Float64SubSteering.rate=0.0f;
			firstTime = false;
	}
	
	// Update is called once per frame
	void Update () {
		

		if(!TankDriver.ManualInput)
		{
				//#if VERBOSE
				if (!firstTime)
				{
					Debug.Log("Got ThrottleWhatever=" + Float64SubThrottle.whatever.ToString() + " and SteerWhatever=" + Float64SubSteering.whatever.ToString());
					Debug.Log("Got ThrottleRate=" + Float64SubThrottle.rate.ToString() + " and SteerRate=" + Float64SubSteering.rate.ToString());
				}
//#endif
			TankDriver.Apply((float)Float64SubThrottle.whatever,(float)Float64SubSteering.whatever);
		}
		
	}
}
}
