
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    public int mID = -1;
    public GameData.ObjectType mObjectType = GameData.ObjectType.None;

    protected virtual void Awake() //�ڽ� Ŭ�������� override �� �� �ݵ�� base.Awake ���� �߰��� ��
    {
        GameManager.mAll_Of_Game_Objects[GameManager.mIDIndex] = this.gameObject; //Ǯ��
        mID = GameManager.mIDIndex++; //���Ǹ� ���� ����Ʈ �ε����� ID�� �����Ѵ�
    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

}

