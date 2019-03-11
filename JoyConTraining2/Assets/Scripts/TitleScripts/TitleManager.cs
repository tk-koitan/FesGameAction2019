using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

    private static readonly Joycon.Button[] m_buttons =
    Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    private float[] stick;

    private Vector3 gyroTitle = new Vector3(0,0,0);
    private Vector3 accelTitle = new Vector3(0,0,0);
    private Quaternion orientationTitle = new Quaternion(0,0,0,0);

    private int cnt = 0;
    private float startCnt = 0f;

    public Transform selectHummer;

    private enum GAMEMODE
    {
        SINGLE,
        DOUBLE,
    }

    private GAMEMODE gameMode = GAMEMODE.SINGLE;

    // Use this for initialization
    void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        if (gameMode == GAMEMODE.SINGLE) selectHummer.position = new Vector3(-4f, 0.2f, 0f);
        else selectHummer.position = new Vector3(-4f, -1.8f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;


        if (m_joycons.Any(c => c.isLeft))
        {
            gyroTitle = m_joyconL.GetGyro();
            accelTitle = m_joyconL.GetAccel();
            orientationTitle = m_joyconL.GetVector();
            //stick = m_joyconL.GetStick();
            //playerCtrl.ActionMove(stick[0]);


            if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_DOWN) ||
                 m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT) ||
                 m_joyconR.GetButtonDown(Joycon.Button.DPAD_LEFT) ||
                 m_joyconR.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                GameStart();
            }
                // ジョイコンを縦に半回転させるとモード切替え
            if (gameMode == GAMEMODE.SINGLE)
            {
                    if (accelTitle.y < -1.0f + 0.5f)
                {
                    gameMode = GAMEMODE.DOUBLE;
                    selectHummer.position = new Vector3(-4f, -1.8f, 0f);
                }
            }
            else
            {
                if (accelTitle.y > 1.0f - 0.5f)
                {
                    gameMode = GAMEMODE.SINGLE;
                    selectHummer.position = new Vector3(-4.0f, 0.2f, 0f);
                }
            }

            /*
            // 1秒間横に振り続けると進む
            startCnt += Mathf.Abs(accelTitle.x);
            cnt++;
            if(cnt >= 60)
            {
                if(startCnt >= 80)
                {
                    GameStart();
                }
                startCnt = 0f;
                cnt = 0;
            }*/
        }

        if (m_joycons.Any(c => !c.isLeft))
        {

        }

    }

    private void GameStart()
    {
        if (gameMode == GAMEMODE.DOUBLE)
        {
            SceneManager.LoadScene("MainGame");
        }
        else
        {
            Debug.Log("一人用モードが始まります");
            // 一人用モードなんてないよ！！！
        }
    }

    private void OnGUI()
    {
        var style = GUI.skin.GetStyle("label");
        style.fontSize = 24;

        if (m_joycons == null || m_joycons.Count <= 0)
        {
            GUILayout.Label("Joy-Con が接続されていません");
            return;
        }

        if (!m_joycons.Any(c => c.isLeft))
        {
            GUILayout.Label("Joy-Con (L) が接続されていません");
            return;
        }

        GUILayout.BeginHorizontal(GUILayout.Width(960));

        foreach (var joycon in m_joycons)
        {
            GUILayout.BeginVertical(GUILayout.Width(480));
            GUILayout.Label(name);
            GUILayout.Label("ジャイロ：" + gyroTitle);
            GUILayout.Label("加速度：" + accelTitle);
            GUILayout.Label("傾き：" + orientationTitle);
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }
}
