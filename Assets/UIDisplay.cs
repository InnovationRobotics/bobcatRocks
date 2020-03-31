using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{

    public Text PositionText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
