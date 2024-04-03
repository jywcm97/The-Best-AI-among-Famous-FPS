using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraManager
{
    public UIBase_CameraNameUI mUIBase_CameraNameUI = null;


    private List<Camera> cameras = new List<Camera>();
    private List<int> cameraOwnerIndex = new List<int>();
    private int currentCameraIndex = -1;

    public void Awake()
    {
        mUIBase_CameraNameUI = GameObject.FindFirstObjectByType<UIBase_CameraNameUI>(); //ù��° GamaManager ��ũ��Ʈ�� ���� ������Ʈ�� ã�´�. //GameManager�� �ϳ��� �����Ѵ�

    }


    public void Start()
    {
        foreach (var obj in GameManager.mAll_Of_Game_Objects)
        {

            if (obj.Value.GetComponent<ObjectBase>().mObjectType == GameData.ObjectType.AI)
            {
                AddCameraToCharacter(obj.Value, obj.Value.GetComponent<ObjectBase>().mID); //AI�鿡�� ī�޶� �����Ѵ�.

            }
        }
    }
    public void Update()
    {
        if (mUIBase_CameraNameUI != null) uiUpdate();



        // �����̽��ٸ� ���� ������ ī�޶� ��ȯ (ī�޶� ����)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���� ī�޶� ��Ȱ��ȭ
            if (currentCameraIndex != -1)
            {
                cameras[currentCameraIndex].enabled = false;
            }

            // ���� ī�޶�� �̵�
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;

            // �� ī�޶� Ȱ��ȭ
            cameras[currentCameraIndex].enabled = true;
        }
    }

    public void uiUpdate()
    {

        //�߾� �ϴ� ���� ī�޶� ĳ���� �̸�
        if (currentCameraIndex != -1)
        {
            mUIBase_CameraNameUI.GetComponent<Text>().text = "";
            mUIBase_CameraNameUI.GetComponent<Text>().text += GameManager.mAll_Of_Game_Objects[cameraOwnerIndex[currentCameraIndex]].name;
        }




    }


    public void AddCameraToCharacter(GameObject character, int pID)
    {
        // ĳ���Ϳ� ī�޶� ���� �� ����
        GameObject cameraGameObject = new GameObject("CharacterCamera");
        Camera cam = cameraGameObject.AddComponent<Camera>();
        cam.transform.SetParent(character.transform);
        cam.transform.localPosition = new Vector3(0, 2.5f, 0); // �Ӹ� ���� ��ġ
        cam.transform.rotation *= Quaternion.Euler(20, 0, 0);

        // ī�޶� ����Ʈ�� �߰�
        cameras.Add(cam);
        cameraOwnerIndex.Add(pID);
        // ī�޶� �ʱ� ��Ȱ��ȭ
        cam.enabled = false;
    }


}
