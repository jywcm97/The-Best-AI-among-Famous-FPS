using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectBase_ItemBase : ObjectBase //���𰡿� ����� �� �̺�Ʈ�� ó���ؾ� �ϴ� ���� ���������� ����.
{
    public bool mIsRespawning = false;
    public int mRespawnCount = 9999;

    protected override void Awake()
    {
        base.Awake();

    }

    public virtual void action(int pReachedAIsIDNumber) //��� �������� �����ϸ� ������ AI���� ���� �̺�Ʈ, �������� ���� override�Ͽ� ����
    {
    }

}
