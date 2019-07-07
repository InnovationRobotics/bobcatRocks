using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RosSharp.RosBridgeClient
{
public class MoveRobil : MonoBehaviour {

	public TankDriver TankDriver;
	public Float64Subscriber Float64SubscriberThrottle;
	public Float64Subscriber Float64SubscriberSteering;
	// Use this for initialization
	void Start () 
	{
		
		Float64SubscriberThrottle=GetComponent<Float64Subscriber>();
		Float64SubscriberSteering=GetComponent<Float64Subscriber>();
	}
	
	// Update is called once per frame
	void Update () {
		

		if(!TankDriver.ManualInput)
		{
			TankDriver.Apply((float)Float64SubscriberThrottle.whatever,(float)Float64SubscriberSteering.whatever);
		}
		
	}
}
}
