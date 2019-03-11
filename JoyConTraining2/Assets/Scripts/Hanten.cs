using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanten : MonoBehaviour {

    Vector3 angles;

    bool isNormal = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        angles = GetJoyConValues.accel;

        if(angles.y > 0 && !isNormal)
        {
            GetComponent<SpriteRenderer>().color =
                new Color(1.0f, 1.0f, 1.0f, 1.0f);
            isNormal = true;
        }
        if(angles.y <= 0 && isNormal)
        {
            GetComponent<SpriteRenderer>().color =
                new Color(1.0f, 0.0f, 0.0f, 1.0f);
            isNormal = false;
        }
	}
}
