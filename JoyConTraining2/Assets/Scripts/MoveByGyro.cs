using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByGyro : MonoBehaviour {

    private float gyroX;

    [System.NonSerialized] public float speed = 5f; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gyroX = GetJoyConValues.gyro.x;

        if(gyroX > 0)
        {
            transform.Translate(-gyroX * speed *  0.01f, 0, 0);
        }

        // JoyconLibを使うと通常のjoyConによる入力はできないっぽい
        /*
        float JoyMvx = Input.GetAxis("Horizontal1");
        float JoyMvy = Input.GetAxis("Vertical1");

        Debug.LogFormat("{0},{1}", JoyMvx, JoyMvy);
        transform.Translate(JoyMvx * 0.5f, 0, 0);
        */
        FieldLoopMethod.fieldLoop(this.transform);
	}


}
