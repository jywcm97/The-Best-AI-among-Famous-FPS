using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

public class AIBase_Xonotic : ObjectBase_AIBase
{

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
            //����ģ ���� ������� �ʴ´�.
            shoot(mVisibleEnemy);

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
        else //���°� ������ �� ���� ã�´�
        {

            if (mClosestOccupy != -1)//�������̸�
            {
                moveTo(mClosestOccupy, false, 4);

            }
            else if(mClosestEnemy != -1)  //������ġ��
            {
                moveTo(mClosestEnemy, false, 5);
                /*
                if (mClosestAmmo != -1 || mClosestHeal != -1) // �������� ����
                {
                    if (mClosestAmmo != -1 && mClosestHeal == -1) //�Ѿ� �����۸� �����Ѵ�
                    {
                        moveTo(mClosestAmmo, false, lCommandID++); //�Ѿ˷� �̵�
                    }
                    else if (mClosestAmmo == -1 && mClosestHeal != -1) //ü�� �����۸� �����Ѵ�
                    {
                        moveTo(mClosestHeal, false, lCommandID++); //ü������ �̵�
                    }
                    else //�Ѵ� �����Ѵ�
                    {
                        if (Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestAmmo].transform.position)
                                       < Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestHeal].transform.position))
                        {
                            moveTo(mClosestAmmo, false, lCommandID++); // ���� ����� ������ �ϳ�
                        }
                        else
                        {
                            moveTo(mClosestHeal, false, lCommandID++);
                        }
                    }
                }
                */
            }
        }





    }


}

