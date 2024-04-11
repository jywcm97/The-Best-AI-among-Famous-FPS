using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

public class AIBase_AssaultCube : ObjectBase_AIBase
{

    int mLastEnemy = -1;

    public int mClosestEnemy = -1;
    public int mVisibleEnemy = -1;
    public int mClosestHeal = -1;
    public int mClosestAmmo = -1;
    public int mClosestOccupy = -1;

    public int mSafestHeal = -1;
    public int mSafestAmmo = -1;

    override public void think()
    {
        base.think();

        mSafestHeal = searchItemNumber(mID, GameData.SearchType.Safe, GameData.ObjectType.Heal);
        mSafestAmmo = searchItemNumber(mID, GameData.SearchType.Safe, GameData.ObjectType.Ammo);
        mClosestEnemy = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.AI, GameData.TeamType.Enemy);
        mVisibleEnemy = searchItemNumber(mID, GameData.SearchType.Visible, GameData.ObjectType.AI, GameData.TeamType.Enemy);
        mClosestHeal = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.Heal);
        mClosestAmmo = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.Ammo);
        mClosestOccupy = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.OccupyPlace);


        //����
        if (mVisibleEnemy != -1) // ���� ���̸�
        {
            mLastEnemy = mVisibleEnemy; // ����ģ ������ ���
            shoot(mLastEnemy);

        }

        //������
        if (getHpPercentage() < 0.5 || getAmmoPercentage() < 0.5) //���°� ���� �ʴ� "�ʿ��� ������ Ž��"
        {
            if (getHpPercentage() <= getAmmoPercentage() && mClosestHeal != -1) //ü���� �� ����
            {
                moveTo(mClosestHeal, false, 0); // ����� ü�� ���������� �̵�

            }
            else if (getHpPercentage() > getAmmoPercentage() && mClosestAmmo != -1) //�Ѿ��� �� ����
            {
                moveTo(mClosestAmmo, false, 1); // ����� �Ѿ� ���������� �̵�
            }
            else //���°� ���� ������ �������� �������� ���� ��
            {
                //Stop
            }
        }
        else //���°� ������ ��
        {


            if (mClosestOccupy != -1)//�������̸�
            {
                moveTo(mClosestOccupy, false, 2);

            }
            else //������ġ��
            {
                if (mLastEnemy != -1) //���������� ���� �������� �̵�
                {
                    moveTo(mLastEnemy, false, 3);
                }
                else if (mClosestEnemy != -1)
                {
                    moveTo(mClosestEnemy, false, 4);

                }
            }



        }




        if (mLastEnemy != -1 && !GameManager.mAll_Of_Game_Objects[mLastEnemy].activeSelf) mLastEnemy = -1; //����� �׾��ٸ� �ʱ�ȭ
    }


    public override void respawn()
    {
        base.respawn(); // ���� respawn����
        mLastEnemy = -1; // ���� �׾����� ��� �ʱ�ȭ

    }

    public override void killed(int pDeadAIsID)
    {
        base.killed(pDeadAIsID);
        if(pDeadAIsID == mLastEnemy) mLastEnemy = -1; //�Ѵ� ��븦 �׿����� ��� �ʱ�ȭ
    }

}

