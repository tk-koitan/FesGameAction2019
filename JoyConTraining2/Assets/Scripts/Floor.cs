using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public bool useAccel = true;
    public bool useOrientation = false;
    public bool useGyro = false;
    Vector3 angles;
    Vector3 joyConAngles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        joyConAngles = GetJoyConValues.accel;
        if (useAccel)
        {
            angles = transform.localEulerAngles; 
                                               // この下のなんかエラー起きる　けど全回転できるようになる
            angles.z = joyConAngles.x * -90.0f;// * (joyConAngles.y/Mathf.Abs(joyConAngles.y));
        }
        else if(useOrientation)
        {
            angles = transform.localEulerAngles;
            angles.z = GetJoyConValues.orientation.eulerAngles.y;
        }
        else if (useGyro)
        {
            angles = transform.localEulerAngles;
            angles.z -= GetJoyConValues.gyro.z;
        }
        transform.localEulerAngles = angles;
    }
}
