using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rigid;

    public bool isLoop = true;

    private Transform groundCheck_L;
    private Transform groundCheck_C;
    private Transform groundCheck_R;

    [System.NonSerialized] public bool grounded = false;
    [System.NonSerialized] public float groundY = 0.0f;
    [System.NonSerialized] public int score = 0;

    private float nGrondedTime = 0.0f;
    private float jumpStartTime = 0.0f;
    private bool jumped = false;

    public float initSpeed = 5.0f;
    private float speedVx = 0.0f;
    private float speedVy = 0.0f;

    // ======= コード（サポート関数）================================-
    public static GameObject GetGameObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }


    // Use this for initialization
    void Awake () {
        rigid = GetComponent<Rigidbody2D>();
        groundCheck_L = transform.Find("GroundCheck_L"); // 接地判定のためのオブジェクトを読み込む
        groundCheck_C = transform.Find("GroundCheck_C");
        groundCheck_R = transform.Find("GroundCheck_R");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // 地面チェック
        grounded = false;

        Collider2D[][] GroundCheckCollider = new Collider2D[3][];
        GroundCheckCollider[0] = Physics2D.OverlapPointAll(groundCheck_L.position);
        GroundCheckCollider[1] = Physics2D.OverlapPointAll(groundCheck_C.position);
        GroundCheckCollider[2] = Physics2D.OverlapPointAll(groundCheck_R.position);

        foreach (Collider2D[] GroundCheckList in GroundCheckCollider) // 接地しているかチェック
        {
            foreach (Collider2D GroundCheck in GroundCheckList)
            {
                if (GroundCheck != null)
                {
                    if (GroundCheck.tag == "Bird")
                    {
                        grounded = true;
                        if (!jumped && groundY < transform.position.y)
                        {
                            groundY = transform.position.y;
                            score = (int)(groundY * 10);
                            //Debug.Log(score);
                        }
                    }
                    else if(GroundCheck.tag == "Load")
                    {
                        grounded = true;
                        groundY = transform.position.y;
                            //Debug.Log(score);
                    }
                }
            }
        }

        if (jumped)
        {
            if (grounded && Time.fixedTime > jumpStartTime + 0.3f)
            {
                jumped = false;
            }
        }
        else
        {
            if (grounded)
            {
                speedVy = -0.2f;
            }
        }
        if (!grounded)
        {
            speedVy -= 0.8f;
        }

        // 画面端ループ
        if(isLoop)
            if(!grounded)
                FieldLoopMethod.fieldLoop(this.transform);

        //Debug.LogFormat("jumped:{0}, grounded:{1}", jumped, grounded);
        rigid.velocity = new Vector2(speedVx, speedVy);
	}

    public void ActionMove(float nx)
    {
        if (nx != 0)
            speedVx = nx * initSpeed;
        else
            speedVx = 0;
    }

    public void ActionJump()
    {
        jumpStartTime = Time.fixedTime;
        jumped = true;
        speedVy = 18.0f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bird")
        {
            //Debug.Log("Do it");
            transform.SetParent(col.transform);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bird")
        {
            transform.SetParent(null);
        }
    }
}
