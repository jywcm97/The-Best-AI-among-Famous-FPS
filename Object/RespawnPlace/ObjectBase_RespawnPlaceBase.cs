using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectBase_RespawnPlaceBase : ObjectBase //���𰡿� ����� �� �̺�Ʈ�� ó���ؾ� �ϴ� ���� ���������� ����.
{
    public ObjectBase mPlaceOwner;

    protected override void Awake()
    {
        base.Awake();
        mObjectType = GameData.ObjectType.RespawnPlace;
    }

}
