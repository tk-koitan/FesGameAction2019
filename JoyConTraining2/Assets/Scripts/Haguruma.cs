using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haguruma : MonoBehaviour {

    Vector3 gyro;
    Vector3 angles;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gyro = GetJoyConValues.gyro;

        
            angles = transform.localEulerAngles;
            angles.z -= gyro.z;
            transform.localEulerAngles = angles;

    }
}
