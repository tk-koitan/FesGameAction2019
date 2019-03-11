using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBirds : MonoBehaviour {

    private bool isExisted = true;
    private float acceY;
    public bool startExist;

    SpriteRenderer spriteRen;

	// Use this for initialization
	void Start () {
        spriteRen = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        acceY = GetJoyConValues.accel.y;

        // 無駄なコード多いから後で修正
        if (startExist)
        {
            if (!isExisted)
            {
                if (acceY > 1.0f - 1.0f)
                {
                    isExisted = true;
                    ChangeColor(isExisted);
                }
            }
            else
            {
                if (acceY < -1.0f + 1.0f)
                {
                    isExisted = false;
                    ChangeColor(isExisted);
                }
            }
        }
        else
        {
            if (isExisted)
            {
                if (acceY > 1.0f - 1.0f)
                {
                    isExisted = false;
                    ChangeColor(isExisted);
                }
            }
            else
            {
                if (acceY < -1.0f + 1.0f)
                {
                    isExisted = true;
                    ChangeColor(isExisted);
                }
            }
        }
	}

    private void ChangeColor(bool toExist)
    {
        if (toExist)
        {
            spriteRen.color =
                new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, 1.0f);
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            spriteRen.color =
    new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, 0.3f);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
