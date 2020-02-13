using Assets.MonogoScripts;
using UnityEngine;

public class DataBaseMongo : MonoBehaviour
{
    private Report report;

    // Start is called before the first frame update
    void Start()
    {
        report = new Report("Name", "Scenario", "Configuration");
        MongoDBHelper.SaveToCollection(report);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
