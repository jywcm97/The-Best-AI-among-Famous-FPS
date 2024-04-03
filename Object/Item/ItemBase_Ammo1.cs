using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemBase_Ammo1 : ObjectBase_ItemBase
{

    protected override void Awake()
    {
        base.Awake();
        mObjectType = GameData.ObjectType.Ammo;
        mRespawnCount = 10;
    }
    public override void action(int pReachedAIsIDNumber)
    {
        base.action(pReachedAIsIDNumber);
        ObjectBase_AIBase mAIClass = GameManager.mAll_Of_Game_Objects[pReachedAIsIDNumber].GetComponent<ObjectBase_AIBase>();

        mAIClass.mCurrentAmmo[mAIClass.mUsingWeapon] += GameData.mWeaponDataDictionary[mAIClass.mUsingWeapon].mInitBullets; //��� �ִ� ���� źâ�� ���� ������ Initbullet��ŭ ���Ѵ�
        if (mAIClass.mCurrentAmmo[mAIClass.mUsingWeapon]
            > GameData.mWeaponDataDictionary[mAIClass.mUsingWeapon].mMaxBullets) //����ִ� ���� ������Max ġ�� �Ѿ�� Max�� �����.
        {
            mAIClass.mCurrentAmmo[mAIClass.mUsingWeapon] = GameData.mWeaponDataDictionary[mAIClass.mUsingWeapon].mMaxBullets;
        }
        this.gameObject.SetActive(false); // Deactivates the object
    }


}
