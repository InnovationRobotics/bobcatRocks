using UnityEngine;
using UnityEngine.UI;

public class MeasureTool : MonoBehaviour
{
    [Header("Arrows")]
    public GameObject arrowL;
    public GameObject arrowR;
    [Range(-0.15f, 0.15f)]
    public float arrowScale = 0.15f;
    [Range(0, 90)]
    public float arrowAngle = 0;
    public Color arrowColor;

    [Header("Text")]
    public Text TextField;
    [Range(0, 0.1f)]
    public float TextScale = 0.02f;
    public Color textColor;

    public GameObject Canvas;

    private float distance;


    void OnDrawGizmos()
    {
        MesureStuff();
    }

    void OnValidate()
    {
        MesureStuff();
    }

    void Update()
    {
        MesureStuff();
    }
    void MesureStuff()
    {

        distance = Vector3.Distance(arrowL.transform.position, arrowR.transform.position);
        TextField.text = distance.ToString("N2") + "cm";
        Canvas.transform.position = LerpByDistance(arrowL.transform.position, arrowR.transform.position, 0.5f);
        if (arrowL != null)
        {
            arrowL.GetComponent<SpriteRenderer>().color = arrowColor;
            arrowL.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
            arrowL.transform.localRotation = Quaternion.Euler(arrowAngle, 0, 0);
        }
        if (arrowR != null)
        {
            arrowR.GetComponent<SpriteRenderer>().color = arrowColor;
            arrowR.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
            arrowR.transform.localRotation = Quaternion.Euler(arrowAngle, 0, 0);
        }

        if (TextField != null)
        {
            TextField.color = textColor;
            TextField.transform.localScale = new Vector3(TextScale, TextScale, TextScale);
        }
    }

    Vector3 LerpByDistance(Vector3 A, Vector3 B, float X)
    {
        Vector3 p = A + X * (B - A);
        return p;
    }
    // Start is called before the first frame update

}
