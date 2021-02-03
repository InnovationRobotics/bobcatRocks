/*
This message class is generated automatically with 'SimpleMessageGenerator' of ROS#
*/ 

using Newtonsoft.Json;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Nav;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;

namespace RosSharp.RosBridgeClient.MessageTypes
{
public class FibonacciActionResult : Message
{
[JsonIgnore]
public const string RosMessageName = "actionlib_tutorials/FibonacciActionResult";

public Header header;
public GoalStatus status;
public FibonacciResult result;

public FibonacciActionResult()
{
header = new Header();
status = new GoalStatus();
result = new FibonacciResult();
}
}
}

