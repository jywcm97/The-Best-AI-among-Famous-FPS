using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OccupyScoreManager : MonoBehaviour
{
    public GameObject occupyPlace = null; // ���� ��� ������Ʈ
    private Dictionary<System.Type, float> teamScores; // ���� ����
    public UIBase_OccupyScoreUI mOccupyUI = null;
    float mDataSaveCount = 0.0f;

    public void Awake() {

        mOccupyUI = GameObject.FindFirstObjectByType<UIBase_OccupyScoreUI>(); //ù��° GamaManager ��ũ��Ʈ�� ���� ������Ʈ�� ã�´�. //GameManager�� �ϳ��� �����Ѵ�

    }

    public void Start()
    {
        foreach(var obj in GameManager.mAll_Of_Game_Objects)
        {
            if (obj.Value.GetComponent<ObjectBase_OccupyPlaceBase>() != null)
            {
                occupyPlace = obj.Value;
            }
        }

        InitializeTeamScores();
    }

    void InitializeTeamScores()
    {
        teamScores = new Dictionary<System.Type, float>();

        // AI_Base�� ��ӹ޴� ��� Ÿ���� �˻�
        var aiTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ObjectBase_AIBase)));

        // �� Ÿ�Կ� ���� �� ������ 0���� �ʱ�ȭ
        foreach (var aiType in aiTypes)
        {
            if (!teamScores.ContainsKey(aiType))
            {
                teamScores[aiType] = 0;
            }
        }
    }

    public void Update()
    {
        if (occupyPlace == null) return;

        // ��� AI�� ��ȸ�ϸ鼭 ���� ���� �ȿ� �ִ��� Ȯ��
        foreach (var obj in GameManager.mAll_Of_Game_Objects)
        {
            ObjectBase_AIBase aiBase = obj.Value.GetComponent<ObjectBase_AIBase>();
            if (aiBase != null
                && aiBase.gameObject.activeSelf
                && Vector3.Distance(obj.Value.transform.position, occupyPlace.transform.position) <= occupyPlace.GetComponent<ObjectBase_OccupyPlaceBase>().mOccupyRange)
            {
                // �ش� AI Ÿ���� ������ ���� (�ߺ� ���� �� ���� ���)
                System.Type aiType = aiBase.GetType();
                if (teamScores.ContainsKey(aiType))
                {
                    teamScores[aiType]+= Time.deltaTime;
                }
            }
        }



        ///////////30�ʸ��� ��� ����

        mDataSaveCount += Time.deltaTime;
        if(mDataSaveCount > 20)
        {
            mDataSaveCount = 0.0f;
            GameDataSaver.SaveOccupyResultsToCSV(("CS", (int)teamScores[typeof(AIBase_CS)]),
                ("OW", (int)teamScores[typeof(AIBase_OW)]),
                ("AssaultCube", (int)teamScores[typeof(AIBase_AssaultCube)]),
                ("Xonotic", (int)teamScores[typeof(AIBase_Xonotic)]),
                ("Modified_AssaultCube", (int)teamScores[typeof(AIBase_Modified_AssaultCube)]),
                ("Modified_Xonotic", (int)teamScores[typeof(AIBase_Modified_Xonotic)])
                );
        }


        //////////////////UI
        ///
        mOccupyUI.GetComponent<Text>().text = "";

        foreach (var score in teamScores)
        {
            mOccupyUI.GetComponent<Text>().text += score.Key.Name + " Team Score: " + (int)score.Value + "\n";
        }
    }

}
