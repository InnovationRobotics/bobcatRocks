using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using RosSharp.RosBridgeClient;

[BurstCompile]
public struct RayToHeightJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<RaycastHit> _RayCastHits;
    public NativeArray<float> _TerrainHeights;


    public void Execute(int index)
    {

        var hit = _RayCastHits[index];
        var dis = hit.point.y;
        _TerrainHeights[index] = dis;




    }
}


[BurstCompile]
public struct ShotRayCastJob : IJobParallelFor
{

    // public NativeArray<Vector3> _PositionCell;
    public NativeArray<RaycastHit> _RayCastResults;
    public NativeArray<RaycastCommand> _Commands;


    public Vector3 Min;
    public float _Width;
    public float _Height;
    public float _Result;
    public Vector3 _TransformPostion;
    public Quaternion _Quaternion;
    public Vector3 _TransformScale;
    public int _LayerMask;


    Vector3 LocalToWorld(Vector3 position, Vector3 myTransformPostion, Quaternion myTransformRotation, Vector3 myTransformScale)
    {
        return myTransformPostion + myTransformRotation * (Vector3.Scale(position, myTransformScale));
    }

    public void Execute(int index)
    {
        int i = index / (int)_Width;
        int j = index % (int)_Width;
       
        var z = i*0.2f;
        var x = j * 0.2f;

        Min = new Vector3(Min.x, 0, Min.z);
        var Result = LocalToWorld(Min + new Vector3(x, 100, z), _TransformPostion, _Quaternion, _TransformScale);
        _Commands[index] = new RaycastCommand(Result, Vector3.down, 500, _LayerMask);
        

    }
}

public class MapHightManager : Publisher<RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray>
{
  

  

    public Terrain Terrain;
    public Transform Sphere;
    public float Width;
    public float Height;
    public NativeArray<Vector3> cells;
    public NativeArray<float> TerrainHeights;
    public NativeArray<RaycastHit> RayCastResults;
    public NativeArray<RaycastCommand> Commands;
    public float size;

    private ShotRayCastJob RayCastJob;
    private RayToHeightJob RayCastToHeightJob;
    private RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray message;

    public float[] TerrainHeightResultArry;
    float time;
    bool paint = false;

    // Start is called before the first frame update
    void Awake  ()
    {

        
        Width /= 0.2f;
        Height /= 0.2f;
        size = Width * Height;
        TerrainHeightResultArry = new float[(int)size];
        Terrain = Terrain.activeTerrain;

        cells = new NativeArray<Vector3>((int)size, Allocator.Persistent); 
        TerrainHeights = new NativeArray<float>((int)size, Allocator.Persistent); 
        Commands = new NativeArray<RaycastCommand>((int)size, Allocator.Persistent); 
        RayCastResults = new NativeArray<RaycastHit>((int)size, Allocator.Persistent);



        RayCastJob = new ShotRayCastJob()
        {
            // _PositionCell = cells,
            _Width = Width,
            _Height = Height,
            _Commands = Commands,
            _RayCastResults = RayCastResults,
            _LayerMask = 1 << 10,
            
        };


        RayCastToHeightJob = new RayToHeightJob
        {
            _RayCastHits = RayCastResults,
            _TerrainHeights = TerrainHeights,
           

        };

       

    }
    protected override void Start()
    {
        base.Start();
        InitializeMessage();
    }
    private void InitializeMessage()
    {
        message = new RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray();
        message.data = new float[(int)size];
    }

    // Update is called once per frame
    void Update()
    {
        

        RayCastJob.Min = Sphere.localPosition;
        RayCastJob._TransformPostion = transform.position;
        RayCastJob._Quaternion = transform.rotation;
        RayCastJob._TransformScale = transform.localScale;      




        JobHandle handle = RayCastJob.Schedule(Commands.Length,1);
        handle.Complete();

  
        //Schedule the batch of raycasts
        JobHandle RayCastHandle = RaycastCommand.ScheduleBatch(Commands, RayCastResults, 8);                   
        RayCastHandle.Complete();


        JobHandle TerrainHeightHandle = RayCastToHeightJob.Schedule(RayCastResults.Length, 1);
        TerrainHeightHandle.Complete();

        NativeArray<float>.Copy(TerrainHeights, message.data);

       time += Time.deltaTime;
       if (time-Time.deltaTime>0.5f)
        {
            Publish(message);
            time = Time.deltaTime;
        }
     




    }
    
    private void LateUpdate()
    {
      
         if (Input.GetKeyDown(KeyCode.I))
            paint = !paint;

        if (paint)
        {
            for (int index = 0; index < RayCastResults.Length; index++)
            {



                int i = index / (int)Width;
                int j = index % (int)Width;
                if (j % 2 == 0 && i % 2 == 0)
                {
                    float z = (float)j / 10;
                    float x = (float)i / 10;



                    var result = LocalToWorld(Sphere.localPosition + new Vector3(x, 0, z), transform);
                    // Terrain.activeTerrain.SampleHeight(result);
                    Debug.DrawLine(new Vector3(result.x, 100, result.z), RayCastResults[index].point, Color.red);
                    var hit = RayCastResults[index];
                    var dis = hit.point.y;



                   // Debug.Log("Hit" + dis);


                }
            }
        }
    }

   

    private void OnDestroy()
    {
        cells.Dispose();
        TerrainHeights.Dispose();
        RayCastResults.Dispose();
        Commands.Dispose();
    }

  

    //LOCAL TO WORLD
    Vector3 LocalToWorld(Vector3 position, Transform myTransform)
    {
        return myTransform.position + myTransform.rotation * (Vector3.Scale(position, myTransform.localScale));
    }

    //WORLD TO LOCAL
    Vector3 WorldToLocal(Vector3 position, Transform myTransform)
    {
        Vector3 scaleInvert = myTransform.localScale;
        scaleInvert = new Vector3(1f / scaleInvert.x, 1f / scaleInvert.y, 1f / scaleInvert.z);
        return scaleInvert;


    }
}
