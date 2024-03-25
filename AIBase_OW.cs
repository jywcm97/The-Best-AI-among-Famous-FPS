using UnityEngine;
using UnityEngine.AI;

public class AIBase_OW : ObjectBase_AIBase
{
    public int mVisibleAI = -1;
    public int mClosestHeal = -1;
    public int mClosestAmmo = -1;
    public int mClosestTeammate = -1;
    public int mClosestOccupy = -1;
    public int mClosestEnemy = -1;

    override public void think()
    {

        mVisibleAI = searchItemNumber(mID, GameData.SearchType.Visible, GameData.ObjectType.AI, GameData.TeamType.Enemy);
        mClosestHeal = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.Heal);
        mClosestAmmo = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.Ammo);
        mClosestTeammate = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.AI, GameData.TeamType.Teammate);
        mClosestOccupy = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.OccupyPlace);
        mClosestEnemy = searchItemNumber(mID, GameData.SearchType.Closest, GameData.ObjectType.AI, GameData.TeamType.Enemy);



        //����
        if (mVisibleAI != -1) // ���� ���̸�
        {
            shoot(mVisibleAI); // ���� ����
        }

        //�̵�

        if (getHpPercentage() < 0.25 || getAmmoPercentage() < 0.25) //���°� ���� �ʴ� "�ʿ��� ������ Ž��" //0.25�� �� ������ �����ؼ� �پ�ٴϴ� ���� �� ���� AI�� �ٽ�
        {
            if (getHpPercentage() <= getAmmoPercentage() && mClosestHeal != -1) //ü���� �� ����
            {
                moveTo(mClosestHeal, false, 0); // ü�� ���������� �̵�

            }
            else if (getHpPercentage() > getAmmoPercentage() && mClosestAmmo != -1) //�Ѿ��� �� ����
            {
                moveTo(mClosestAmmo, false, 1); // �Ѿ� ���������� �̵�

            }
            else //���°� ���� ������ �������� �������� ���� ��
            {
                if (mClosestTeammate != -1 && Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestTeammate].transform.position) > 10) //������ �ִµ� �ָ�
                {
                    moveTo(mClosestTeammate, false, 2);
                }

            }
        }
        else //���°� ������ ��
        {
            if (mClosestTeammate != -1)  //������ �����ϸ�
            {
                if(Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestTeammate].transform.position) > 10) //�ָ� �������� �̵�
                {
                    moveTo(mClosestTeammate, false, 3);
                }
                else //������
                {
                    if (mClosestOccupy != -1) //�������̸�
                    {
                        moveTo(mClosestOccupy, false, 4); //�������� �̵�
                    }
                    else //�������� �ƴϸ�
                    {
                        if(mClosestEnemy != -1) moveTo(mClosestEnemy, false, 5); //����� ������ �̵�
                    }
                }

            }
            else //������ �������� ������
            {
                if (mClosestOccupy != -1) //�������̸�
                {
                    moveTo(mClosestOccupy, false, 6); //�������� �̵�
                }
                else //�������� �ƴϸ�
                {
                    if (mClosestEnemy != -1)  moveTo(mClosestEnemy, false, 7); //����� ������ �̵� //������ġ���� �������� �ƴ� ��밡 ��ǥ
                }
            }
            //������ġ���� ������ �ִ�ü�¿��� �Ծ����� �ʴ´�.
            
        }




    }

}

