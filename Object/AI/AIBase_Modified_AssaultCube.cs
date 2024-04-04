using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

public class AIBase_Modified_AssaultCube : ObjectBase_AIBase
{

    int mLastEnemy = -1;

    public int mClosestEnemy = -1;
    public int mVisibleEnemy = -1;
    public int mClosestHeal = -1;
    public int mClosestAmmo = -1;
    public int mClosestOccupy = -1;
    public int mClosestTeammate = -1;

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
        mClosestTeammate = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.AI, GameData.TeamType.Teammate);


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
                if (mVisibleEnemy != -1) moveTo(mSafestHeal, true, 0); //���� ���� �� �ݵ�� ������ ��������
                else moveTo(mClosestHeal, false, 1); // ü�� ���������� �̵�

            }
            else if (getHpPercentage() > getAmmoPercentage() && mClosestAmmo != -1) //�Ѿ��� �� ����
            {
                if (mVisibleEnemy != -1) moveTo(mSafestAmmo, true, 2); //���� ���� �� �ݵ�� ������ �Ѿ˷�
                else moveTo(mClosestAmmo, false, 3); // �Ѿ� ���������� �̵�
            }
            else //���°� ���� ������ �������� �������� ���� ��
            {
                //Stop
            }
        }
        else //���°� ������ ��
        {

            if (mClosestTeammate != -1)  //������ �����ϸ�
            {
                if (Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestTeammate].transform.position) > 10) //�ָ� �������� �̵�
                {
                    moveTo(mClosestTeammate, false, 4);
                }
                else //������
                {
                    if (mClosestOccupy != -1)//�������̸�
                    {
                        moveTo(mClosestOccupy, false, 5);

                    }
                    else //������ġ��
                    {
                        if (mLastEnemy != -1) //���������� ���� �������� �̵�
                        {
                            moveTo(mLastEnemy, false, 6);
                        }
                        else if (mClosestEnemy != -1)
                        {
                            moveTo(mClosestEnemy, false, 7);

                        }
                    }
                }
            }
            else //������ �������� ������
            {
                if (mClosestOccupy != -1)//�������̸�
                {
                    moveTo(mClosestOccupy, false, 8);

                }
                else //������ġ��
                {
                    if (mLastEnemy != -1) //���������� ���� �������� �̵�
                    {
                        moveTo(mLastEnemy, false, 9);
                    }
                    else if (mClosestEnemy != -1)
                    {
                        moveTo(mClosestEnemy, false, 10);

                    }
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

