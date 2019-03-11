using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class GetJoyConValues : MonoBehaviour
{

    private static readonly Joycon.Button[] m_buttons =
    Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    PlayerController playerCtrl;

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    static public Vector3 gyro;
    static public Vector3 accel;
    static public Quaternion orientation;
    private float[] stick;

    // Use this for initialization
    void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        playerCtrl = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;


        if (m_joycons.Any(c => c.isLeft)) {
            if (playerCtrl.grounded)
            {
                if (m_joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN) ||
                m_joyconL.GetButtonDown(Joycon.Button.DPAD_RIGHT) ||
                m_joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT) ||
                m_joyconL.GetButtonDown(Joycon.Button.DPAD_UP))
                {
                    playerCtrl.ActionJump();
                }
            }

            //stick = m_joyconL.GetStick();
            //playerCtrl.ActionMove(stick[0]);

        }

        if (m_joycons.Any(c => !c.isLeft)){
            if (playerCtrl.grounded)
            {
                if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_DOWN) ||
                m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT) ||
                m_joyconR.GetButtonDown(Joycon.Button.DPAD_LEFT) ||
                m_joyconR.GetButtonDown(Joycon.Button.DPAD_UP))
                {
                    playerCtrl.ActionJump();
                }
            }

            if (m_joyconR.GetButtonDown(Joycon.Button.SR))
            {
                SceneManager.LoadScene("TitleScene");   
            }
            stick = m_joyconR.GetStick();
            playerCtrl.ActionMove(stick[1]);
        }

        //foreach (var joycon in m_joycons)
        //{ 
            //var stick = joycon.GetStick();
            gyro = m_joyconL.GetGyro();
            accel = m_joyconL.GetAccel();
            orientation = m_joyconL.GetVector();
        //}
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
            GUILayout.Label("ジャイロ：" + gyro);
            GUILayout.Label("加速度：" + accel);
            GUILayout.Label("傾き：" + orientation);
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }
}
