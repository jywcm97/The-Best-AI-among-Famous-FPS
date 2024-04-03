using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemBase_Rifle : ObjectBase_ItemBase
{
    protected override void Awake()
    {
        base.Awake();
        mObjectType = GameData.ObjectType.Weapon;
    }
    public override void action(int pReachedAIsIDNumber)
    {
        base.action(pReachedAIsIDNumber);

        ObjectBase_AIBase mAIClass = GameManager.mAll_Of_Game_Objects[pReachedAIsIDNumber].GetComponent<ObjectBase_AIBase>();

        if (mAIClass == null) return;
        mAIClass.obtainWeapon(GameData.Weapon.Rifle); //���� AI�� obtainWeapon�� �� ������ ������ ���ڷ� �ְ� ȣ���Ѵ�

        this.gameObject.SetActive(false);

    }

}
