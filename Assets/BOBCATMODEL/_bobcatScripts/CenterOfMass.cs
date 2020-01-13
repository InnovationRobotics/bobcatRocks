using UnityEngine;
using System.Collections;

public class CenterOfMass : MonoBehaviour {
    public Transform com;
    public Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com.localPosition;
    }
}
