using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class StatViewManager
{
    public UIBase_StatUI mScoreUI = null;

    public void Awake()
    {
        mScoreUI = GameObject.FindFirstObjectByType<UIBase_StatUI>(); //ù��° GamaManager ��ũ��Ʈ�� ���� ������Ʈ�� ã�´�. //GameManager�� �ϳ��� �����Ѵ�
    }

    public void Start()
    {
    }
    public void Update()
    {
        if (mScoreUI != null) uiUpdate();
    }

    public void uiUpdate()
    {

        //���� ��� ������
        mScoreUI.GetComponent<Text>().text = "";
        foreach(var obj in GameManager.mAll_Of_Game_Objects)
        {
            ObjectBase_AIBase lAI = obj.Value.GetComponent<ObjectBase_AIBase>();
            if (lAI == null) continue;

            mScoreUI.GetComponent<Text>().text +=
                lAI.name
                + "'s HP: "
                + lAI.mCurrentHP
                + "  /  Ammo:"
                + lAI.mCurrentAmmo[lAI.mUsingWeapon]
                + "\n";

        }


    }

}
