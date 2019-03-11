using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSwitchBird : MonoBehaviour {

    private int cnt = 0;
    [System.NonSerialized] public static int switchInterval = 80;
    private bool isExisted;

    SpriteRenderer spriteRen;

    // Use this for initialization
    void Start () {
        spriteRen = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        cnt++;
        if(cnt >= switchInterval)
        {
            if (!isExisted) isExisted = true;
            else isExisted = false;

            ChangeColor(isExisted);
            cnt = 0;
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
