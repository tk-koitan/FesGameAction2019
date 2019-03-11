using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldLoopMethod : MonoBehaviour {

    static public void fieldLoop(Transform target)
    {
        if(target.position.x < -9.1f)
        {
            Vector3 warpPos = target.position;
            warpPos.x += 12f;
            target.position = warpPos;
        }
        if(target.position.x > 3.1f)
        {
            Vector3 warpPos = target.position;
            warpPos.x -= 12f;
            target.position = warpPos;
        }
    }

        
}
