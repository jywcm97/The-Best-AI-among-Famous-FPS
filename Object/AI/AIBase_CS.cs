using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

public class AIBase_CS : ObjectBase_AIBase
{
    public int mVisibleEnemy = -1;
    public int mClosestAmmo = -1;
    public int mClosestTeammate = -1;
    public int mClosestOccupy = -1;
    public int mClosestEnemy = -1;
    override public void think()
    {

        base.think();

        mVisibleEnemy = searchItemNumber(mID, GameData.SearchType.Visible, GameData.ObjectType.AI, GameData.TeamType.Enemy);
        mClosestAmmo = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.Ammo);
        mClosestTeammate = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.AI, GameData.TeamType.Teammate);
        mClosestOccupy = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.OccupyPlace);
        mClosestEnemy = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.AI, GameData.TeamType.Enemy);



        //����
        if (mVisibleEnemy != -1) // ���� ���̸�
        {
            moveStop(); //�̵��� ���߰�
            shoot(mVisibleEnemy);

        }

        //�̵�

        if (getAmmoPercentage() < 0.25) //�Ѿ��� �����ϴ� //0.25�� �� ������ �����ؼ� �پ�ٴϴ� ���� �� ���� AI�� �ٽ�
        {
            if (mClosestAmmo != -1) //�Ѿ� ������ ����
            {
                moveTo(mClosestAmmo, false, 0); // �Ѿ� ���������� �̵�

            }
            else //���°� ���� ������ �������� �������� ���� ��
            {
                if (mClosestTeammate != -1 && Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestTeammate].transform.position) > 10) //������ �ִµ� �ָ�
                {
                    moveTo(mClosestTeammate, false, 1);
                }

            }
        }
        else //���°� ������ �� 
        {
            if (mClosestTeammate != -1)  //������ �����ϸ�
            {
                if (Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestTeammate].transform.position) > 10) //�ָ� �������� �̵�
                {
                    moveTo(mClosestTeammate, false, 2);
                }
                else //������
                {
                    if (mClosestOccupy != -1) //�������̸�
                    {
                        moveTo(mClosestOccupy, false, 3); //�������� �̵�
                    }
                    else //�������� �ƴϸ�
                    {
                        if (mClosestEnemy != -1) moveTo(mClosestEnemy, false, 4); //����� ������ �̵�
                    }
                }

            }
            else //������ �������� ������
            {
                if (mClosestOccupy != -1) //�������̸�
                {
                    moveTo(mClosestOccupy, false, 5); //�������� �̵�
                }
                else //�������� �ƴϸ�
                {
                    if (mClosestEnemy != -1) moveTo(mClosestEnemy, false, 6); //����� ������ �̵� //������ġ���� ���� �������� �ƴ� ��밡 ��ǥ
                }
            }

        }

    }

}

