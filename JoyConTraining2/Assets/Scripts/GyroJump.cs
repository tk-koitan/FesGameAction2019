using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroJump : MonoBehaviour {

    Vector3 gyro;
    float speedVy = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        gyro = GetJoyConValues.gyro;

        if(gyro.x > 0)
        {
        speedVy = gyro.x;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, GetComponent<Rigidbody2D>().velocity.y + speedVy);
        }
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.y);
	}
}
