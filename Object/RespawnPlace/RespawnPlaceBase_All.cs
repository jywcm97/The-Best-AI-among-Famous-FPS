using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RespawnPlaceBase_All : ObjectBase_RespawnPlaceBase
{
    protected override void Awake()
    {
        base.Awake();
        mPlaceOwner = new ObjectBase_AIBase(); //���� ������ ��Ҵ� ���� ������ null
    }
}
