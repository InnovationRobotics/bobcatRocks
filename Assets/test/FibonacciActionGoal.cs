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
public class FibonacciActionGoal : Message
{
[JsonIgnore]
public const string RosMessageName = "actionlib_tutorials/FibonacciActionGoal";

public Header header;
public GoalID goal_id;
public FibonacciGoal goal;

public FibonacciActionGoal()
{
header = new Header();
goal_id = new GoalID();
goal = new FibonacciGoal();
}
}
}

