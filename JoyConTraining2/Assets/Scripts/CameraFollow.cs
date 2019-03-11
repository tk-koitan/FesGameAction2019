﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERATARGET // -----------カメラのターゲットタイプ----------------
{
    PLAYER,             // プレイヤー座標
    PLAYER_MARGIN,      // プレイヤー座標（前方視界を確保するマージン）
    PLAYER_GROUND,      // 過去にプレイヤーが世知下地面の座標
                        // (前方視界を確保するマージン）
}

public enum CAMERAHOMING
{ // ----------カメラのホーミングタイプ---------------
    DIRECT,             // ダイレクトにカメラ座標にターゲット座標を設定する
    LERP,               // カメラとターゲット座標を線形補強する
    SlERP,              // カメラとターゲット座標を曲線補強する
    STOP,               // カメラを止める
}
public class CameraFollow : MonoBehaviour
{
    // ==== 外部パラメータ(Inspector表示)===================
    [System.Serializable]
    public class Param
    {
        public CAMERATARGET targetType = CAMERATARGET.PLAYER_GROUND;
        public CAMERAHOMING homingType = CAMERAHOMING.LERP;
        public Vector2 margin = new Vector2(2.0f, 2.0f);
        public Vector2 homing = new Vector2(0.1f, 0.2f);
        public bool borderCheck = false;
        public GameObject borderLeftTop;
        public GameObject borderRightBottom;
        public bool viewAreaCheck = true;
        public Vector2 viewAreaMinMargin = new Vector2(0.0f, 0.0f);
        public Vector2 viewAreaMaxMargin = new Vector2(0.0f, 2.0f);

        public bool orthographicEnabled = true;
        public float screenOGSize = 5.0f;
        public float screenOGSizeHoming = 0.1f;
        public float screenPSSize = 50.0f;
        public float screenPSSizeHoming = 0.1f;
    }
    public Param param;

    // ====== キャッシュ ===============================
    GameObject player;
    Transform playerTrfm;
    PlayerController playerCtrl;

    float screenOGSizeAdd = 0.0f;
    float screenPSSizeAdd = 0.0f;

    bool verticalEnabled = true; // 横方向にかめらが　移動するか
    bool horizontalEnabled = true;

    // ====== コード(Monobehaviour基本機能の実装) =========
    void Awake()
    {
        player = PlayerController.GetGameObject();
        playerTrfm = player.transform;
        playerCtrl = player.GetComponent<PlayerController>();
    }

    void LateUpdate()
    {
        float targetX = playerTrfm.position.x;
        float targetY = playerTrfm.position.y;
        float pX = transform.position.x;
        float pY = transform.position.y;
        float screenOGSize = GetComponent<Camera>().orthographicSize; // なぜかエラー
        float screenPSSize = GetComponent<Camera>().fieldOfView;

        // ターゲットの設定
        switch (param.targetType)
        {
            case CAMERATARGET.PLAYER:
                targetX = playerTrfm.position.x;
                targetY = playerTrfm.position.y;
                break;
            case CAMERATARGET.PLAYER_MARGIN:
                targetX = playerTrfm.position.x +
                    param.margin.x;// * playerCtrl.dir;
                targetY = playerTrfm.position.y + param.margin.y;
                break;
            case CAMERATARGET.PLAYER_GROUND:
               // targetX = playerTrfm.position.x +
                //    param.margin.x * playerCtrl.dir;
                targetY = playerCtrl.groundY + param.margin.y;
                break;
        }

        // カメラの移動限界境界線チェック
        verticalEnabled = true;
        horizontalEnabled = true;

        if (param.borderCheck)
        {
            float cX = playerTrfm.transform.position.x;
            float cY = playerTrfm.transform.position.y;

            if (cX < param.borderLeftTop.transform.position.x ||
                    cX > param.borderRightBottom.transform.position.x)
            {
                horizontalEnabled = false;
            }
            if (cY > param.borderLeftTop.transform.position.y ||
                    cY < param.borderRightBottom.transform.position.y)
            {
                verticalEnabled = false;
            }
        }

        // プレイヤーのカメラ内チェック
        if (param.viewAreaCheck)
        {
            float z = playerTrfm.position.z - transform.position.z;
            Vector3 minMargin = param.viewAreaMinMargin;
            Vector3 maxMargin = param.viewAreaMaxMargin;
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, z)) - minMargin;
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, z)) - maxMargin;
            if (playerTrfm.position.x < min.x || playerTrfm.position.x > max.x)
            {
                targetX = playerTrfm.position.x;
            }
            if (playerTrfm.position.y < min.y || playerTrfm.position.y > max.y)
            {
                // ここで死亡処理

                targetY = playerTrfm.position.y + 3.0f;
                playerCtrl.groundY = playerTrfm.position.y;
            }
        }

        // カメラ移動（ホーミング）
        switch (param.homingType)
        {
            case CAMERAHOMING.DIRECT:
                pX = 0;pX = targetX;
                pY = targetY;
                screenOGSize = param.screenOGSize;
                screenPSSize = param.screenPSSize;
                break;

            case CAMERAHOMING.LERP: // これしか使ってないからここだけ修正
                if (horizontalEnabled)
                    pX = UnityEngine.Mathf.Lerp(transform.position.x, targetX, param.homing.x);
                if (verticalEnabled)
                    pY = UnityEngine.Mathf.Lerp(transform.position.y, targetY, param.homing.y);
                screenOGSize = UnityEngine.Mathf.Lerp(screenOGSize, param.screenOGSize,
                            param.screenOGSizeHoming);
                screenPSSize = UnityEngine.Mathf.Lerp(screenPSSize, param.screenPSSize,
                            param.screenPSSize);
                break;

            case CAMERAHOMING.SlERP:
                pX = UnityEngine.Mathf.SmoothStep(transform.position.x, targetX, param.homing.x);
                pY = UnityEngine.Mathf.SmoothStep(transform.position.y, targetY, param.homing.y);
                screenOGSize = UnityEngine.Mathf.SmoothStep(screenOGSize,
                            param.screenOGSize, param.screenOGSizeHoming);
                screenPSSize = UnityEngine.Mathf.SmoothStep(screenPSSize,
                            param.screenPSSize, param.screenPSSizeHoming);
                break;

            case CAMERAHOMING.STOP:
                break;
        }

        transform.position = new Vector3(pX, pY, transform.position.z);
        GetComponent<Camera>().orthographic = param.orthographicEnabled;
        GetComponent<Camera>().orthographicSize = screenOGSize + screenOGSizeAdd;
        GetComponent<Camera>().fieldOfView = screenPSSize + screenPSSizeAdd;
        GetComponent<Camera>().orthographicSize = UnityEngine.Mathf.Clamp(GetComponent<Camera>().orthographicSize, 2.5f, 10.0f);
        GetComponent<Camera>().fieldOfView = UnityEngine.Mathf.Clamp(GetComponent<Camera>().fieldOfView, 30.0f, 100.0f);

        // カメラの特殊ズーム効果計算
        screenOGSizeAdd *= 0.9f;
        screenPSSizeAdd *= 0.9f;
    }

    // ======= コード（その他）================================
    public void SetCamera(Param cameraPara)
    {
        param = cameraPara;
    }

    public void AddCameraSize(float ogAdd, float psAdd)
    {
        screenOGSizeAdd = ogAdd;
        screenPSSizeAdd = psAdd;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

