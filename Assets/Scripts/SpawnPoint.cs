using System.IO;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    public ObjList ObjectList = new ObjList();
    private string m_JsonString;
    public Terrain Terrain;
    private TerrainData terrainData;

    public float divRange;
    public float hm;

    public focusCamera Target;
    // Start is called before the first frame update
    void Awake()
    {


        Terrain = FindObjectOfType<Terrain>();
        terrainData = Terrain.terrainData;

        //todo Set In Outside File
        //json parser ***all data commeing from json file 
        var confFile = FileFinder.Find(Application.streamingAssetsPath, "InitialScene" + ".json");  //todo Set In Outside File
        Debug.Log("Found file:" + confFile);
        var m_JsonString = File.ReadAllText(confFile);
        ObjectList = JsonUtility.FromJson<ObjList>(m_JsonString);

        var objListLen = ObjectList.Objects.Count;
        Debug.Log("Found " + objListLen.ToString() + " objects");


        foreach (var obj in ObjectList.Objects)

        {
            RaycastHit hit;

            Vector3 ShootRayFrom = new Vector3(obj.Position.x, 1000, obj.Position.z);
            Ray ray = new Ray(ShootRayFrom, Vector3.down);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {


                GameObject go = Instantiate(Resources.Load(obj.Name) as GameObject);

                if (obj.Name == "BobCat")
                {
                    Target.ExaminedObjects = go.transform;
                    Target.Center();
                }

                go.name = obj.Id;

                go.transform.position = new Vector3(obj.Position.x, hit.point.y + 0.3f, obj.Position.z);
                go.transform.rotation = obj.Rotation;
                go.transform.localScale = obj.Scale;
                Debug.Log("The given " + obj.Name + " position point is here: "+ go.transform.position);

            }
            else
            {

                Debug.LogError("The given " + obj.Name + " postion point is out of boundary, please try diffrent position ");
            }


        }

        // GenerateTerrain(Terrain, hm);// RAndom Terrain
    }

    // Update is called once per frame

    public void GenerateTerrain(Terrain t, float tileSize)
    {
        //The lower the numbers in the number range, the higher the hills/mountains will be...

        //Heights For Our Hills/Mountains
        float[,] hts = new float[t.terrainData.heightmapResolution, t.terrainData.heightmapResolution];
        for (int i = 0; i < t.terrainData.heightmapResolution; i++)
        {
            for (int k = 0; k < t.terrainData.heightmapResolution; k++)
            {
                hts[i, k] = Mathf.PerlinNoise(((float)i / (float)t.terrainData.heightmapResolution) * tileSize, ((float)k / (float)t.terrainData.heightmapResolution) * tileSize) / divRange;
            }
        }
        Debug.LogWarning("DivRange: " + divRange + " , " + "HTiling: " + tileSize);
        t.terrainData.SetHeights(0, 0, hts);
    }





}