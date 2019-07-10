using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RosSharp.RosBridgeClient
{
public class MoveRobil : MonoBehaviour {

	public TankDriver TankDriver;
	public Float64Subscriber Float64SubThrottle;
	public Float64Subscriber Float64SubSteering;
	// Use this for initialization
	void Start () 
	{
		//The attribute has been assigned in the scene so shouldn't be here
		//Float64SubscriberThrottle=GetComponent<Float64Subscriber>();
		//Float64SubscriberSteering=GetComponent<Float64Subscriber>();
	}
	
	// Update is called once per frame
	void Update () {
		

		if(!TankDriver.ManualInput)
		{
#if VERBOSE
			Debug.Log("Got ThrottleWhatever="+Float64SubscriberThrottle.whatever.ToString() + " and SteerWhatever=" + Float64SubscriberSteering.whatever.ToString());
			Debug.Log("Got ThrottleRate="+Float64SubscriberThrottle.rate.ToString() + " and SteerRate=" + Float64SubscriberSteering.rate.ToString());
#endif
			TankDriver.Apply((float)Float64SubThrottle.whatever,(float)Float64SubSteering.whatever);
		}
		
	}
}
}
