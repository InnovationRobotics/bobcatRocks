
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{

    private float LastTime = 0;
    public bool ShowFixUpdate=false;

    void Start()
    {

    }





    float deltaTime = 0.0f;
    float FixdeltaTime = 0.0f;
    private string text;
    private string Fixtext;

    void OnGUI()
    {


        //past
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        Rect Fixrect = new Rect(0, 20, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.blue;


        GUI.Label(rect, text, style);
        GUI.Label(Fixrect, Fixtext, style);
    }


    private void Update()
    {
        float msec = deltaTime * 1000f;
        float fps = 1 / deltaTime;
        text = string.Format("{0:0.0} ms ({1:0.}fps)", msec, fps);
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void FixedUpdate()
    {
        
        if (ShowFixUpdate)
        {

            if (Time.time > LastTime)
            {
                float msec = Time.time - LastTime;

                LastTime = Time.time;

                Fixtext = string.Format("\nPhysic Step: {0:0.0000} ms", msec);
            } 
        }


    }

}
