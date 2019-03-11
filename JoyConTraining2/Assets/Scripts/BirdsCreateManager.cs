using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdsCreateManager : MonoBehaviour {

    public Text degree;
    private string[] degreeText = new string[7] {"ミジンコ級","アオミドロ級","ミドリムシ級", "オオミドリムシ級",
    "アメーバ級","Zakky級","koitan級"};

    // 鳥を作る 鳥の出現確率を表す
    // 固定鳥が0、移動鳥が1、点滅鳥が2、自動点滅鳥が3
    private GameObject[] birds = new GameObject[4];
    private int[] birdsPattern = new int[3] { 0, 0, 1 };
    private int randomBird = 0; // 0,1,2間での乱数
    private Sprite[] birdsSprite = new Sprite[6];
    private int randomSprite = 0; // 0~5での乱数

    // 現在生成されている鳥
    private GameObject[] existBirds = new GameObject[5];
    private int[] birdType = new int[5];
    private int nextBirdIndex = 0;

    // 突破段階によって鳥の出現パターン(birdPattern)をいじる
    private bool[] clearStage = new bool[7] { false, false, false, false, false, false, false };
    public float[] clearSign = new float[7] { 15, 30, 45, 60, 75, 90, 105 };

    // 鳥の出現位置の乱数
    private float randomPosY = 0;
    private float randomPosX = 0;

    public float createScope = 10.0f;
    public float destroyScope = 5.0f;

    private float randomSpeed = 5f; // 移動鳥の速度
    private int randomSwitch = 0; // 点滅鳥の最初の表裏

    GameObject player;
    PlayerController playerCtrl;

    // Use this for initialization
    void Start () {
        //degree = GetComponent<Text>();

        // resourcesからプレハブを読み込む
        birds[0] = (GameObject)Resources.Load("SetBird"); // 固定鳥
        birds[1] = (GameObject)Resources.Load("MoveBird"); // 移動鳥
        birds[2] = (GameObject)Resources.Load("SwitchBird"); // 点滅鳥
        birds[3] = (GameObject)Resources.Load("AutoSwitchBird"); // 自動点滅鳥
        // スプライトを読み込む
        
        birdsSprite[0] = Resources.Load<Sprite>("BirdN");
        birdsSprite[1] = Resources.Load<Sprite>("Bird1");
        birdsSprite[2] = Resources.Load<Sprite>("Bird2");
        birdsSprite[3] = Resources.Load<Sprite>("Bird3");
        birdsSprite[4] = Resources.Load<Sprite>("Bird4");
        birdsSprite[5] = Resources.Load<Sprite>("Bird5");
        
        player = PlayerController.GetGameObject();
        playerCtrl = player.GetComponent<PlayerController>();

        // 鳥を最初に作成
        for(int i = 0; i < existBirds.Length; i++)
        {
            randomPosX = Random.Range(-8.0f, 2.0f);
            randomPosY = Random.Range(2.0f, 2.5f);
            randomBird = Random.Range(0, 3); // 整数0~2
            randomSprite = Random.Range(0, 6);
            if(i != 0)
            {
                if(birdType[i-1] == 1) // 移動鳥ならもっと上に
                {
                    randomPosY = Random.Range(3.0f, 3.5f);
                }
                randomPosY += existBirds[i - 1].transform.position.y;
            }
            Transform birdPos = this.transform; // なんか初期化しなきゃいけなかったっぽいんで
            birdPos.position = new Vector3(randomPosX, randomPosY, 0);
            birdType[i] = birdsPattern[randomBird];
            existBirds[i] = Instantiate(birds[birdType[i]]); // 生成
            existBirds[i].transform.position = birdPos.position;
            this.transform.position = new Vector3(0, 0, 0); // なんか移動しちゃうんで

            //Debug.LogFormat("randomPosX:{0},randomPosY:{1}", randomPosX, randomPosY);
            //Debug.LogFormat("birdPos:{0}, bird:{1}", existBirds[i].transform.position, birdsPattern[randomBird]);

            if(birdType[i] == 1) // 移動鳥ならスピードを決める
            {
                randomSpeed = Random.Range(3.0f, 7.0f);
                existBirds[i].GetComponent<MoveByGyro>().speed = randomSpeed;
            }

            existBirds[i].GetComponent<SpriteRenderer>().sprite = birdsSprite[randomSprite]; // スプライトをランダムに
        }
        nextBirdIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {

        for(int i = 0; i < clearStage.Length - 1; i++)
        {
            if(!clearStage[i] && playerCtrl.groundY > clearSign[i])
            {
                clearStage[i] = true;
                // 1,2,0の順に++していく
                birdsPattern[(i + 1) % 3]++;
                degree.text = degreeText[i];
            }
        }
        // レベル7だけ特別
        if(!clearStage[6] && playerCtrl.groundY > clearSign[6])
        {
            clearStage[6] = true;
            birdsPattern[0]--;
            // 自動点滅鳥の点滅のペースを速める
            AutoSwitchBird.switchInterval /= 2;
            degree.text = degreeText[6];
        }

        // 以下鳥作成
        // PlayerCtrl.groundYよりx上の鳥を作っていく
        // またy下の鳥は削除していく 削除したときに作成で良いかも
        // 一つの状態で最大どれだけの鳥をおけるか
        // 5個としよう　GameObject[] existBirds = new GameObject[5]　でいける
        // 現在のカウントを用意し、4つ前（一番古い）鳥をDestroyしていこう
        // いける！！！！！
        // Startで5つ先に作っておこう

        // nextBirdIndex+existBirds.Length-1)%existBirds.Length

        int preBirdIndex = (nextBirdIndex + existBirds.Length - 1) % existBirds.Length;

        // 最下部の鳥が範囲外にいったら消して最上部のを作る
        //Debug.LogFormat("{0},{1}", playerCtrl.groundY, existBirds[nextBirdIndex].transform.position.y + destroyScope);

        if (playerCtrl.groundY > existBirds[nextBirdIndex].transform.position.y + destroyScope)
        {
            Debug.Log("destroy");
            Destroy(existBirds[nextBirdIndex]);
            randomPosX = Random.Range(-8.0f, 2.0f);
            randomPosY = Random.Range(2.0f, 2.5f);
            randomBird = Random.Range(0, 3); // 整数0~2
            randomSprite = Random.Range(0, 6);

                if (birdType[preBirdIndex] == 1) // 移動鳥ならもっと上に
                {
                    randomPosY = Random.Range(3.0f, 3.5f);
                }
                randomPosY += existBirds[preBirdIndex].transform.position.y;
            
            Transform birdPos = this.transform; // なんか初期化しなきゃいけなかったっぽいんで
            birdPos.position = new Vector3(randomPosX, randomPosY, 0);
            birdType[nextBirdIndex] = birdsPattern[randomBird];
            existBirds[nextBirdIndex] = Instantiate(birds[birdType[nextBirdIndex]]); // 生成
            existBirds[nextBirdIndex].transform.position = birdPos.position;
            this.transform.position = new Vector3(0, 0, 0); // なんか移動しちゃうんで

            //Debug.LogFormat("randomPosX:{0},randomPosY:{1}", randomPosX, randomPosY);
            //Debug.LogFormat("birdPos:{0}, bird:{1}", existBirds[i].transform.position, birdsPattern[randomBird]);

            if (birdType[nextBirdIndex] == 1) // 移動鳥ならスピードを決める
            {
                randomSpeed = Random.Range(2.0f, 9.0f);
                existBirds[nextBirdIndex].GetComponent<MoveByGyro>().speed = randomSpeed;
            }
            if(birdType[nextBirdIndex] == 2) // 点滅鳥なら最初の表裏を決める
            {
                bool newSwitch;
                randomSwitch = Random.Range(0, 2);
                if(randomSwitch == 0)
                {
                    // 裏
                    newSwitch = false;
                }
                else
                {
                    //表
                    newSwitch = true;
                }
                existBirds[nextBirdIndex].GetComponent<SwitchBirds>().startExist = newSwitch;

            }

            existBirds[nextBirdIndex].GetComponent<SpriteRenderer>().sprite = birdsSprite[randomSprite]; // スプライトをランダムに
            nextBirdIndex = (nextBirdIndex+1)% existBirds.Length;
        }
	}
}
