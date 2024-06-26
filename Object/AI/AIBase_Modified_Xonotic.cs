using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

public class AIBase_Modified_Xonotic : ObjectBase_AIBase
{

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



        //공격
        if (mVisibleEnemy != -1) // 적이 보이면
        {
            //마주친 적을 등록하지 않는다.
            shoot(mVisibleEnemy);

        }


        //움직임
        if (getHpPercentage() < 0.5 || getAmmoPercentage() < 0.5) //상태가 좋지 않다 "필요한 아이템 탐색"
        {
            if (getHpPercentage() <= getAmmoPercentage() && mClosestHeal != -1) //체력이 더 부족
            {
                if (mVisibleEnemy != -1) moveTo(mSafestHeal, true, 0); //적이 보일 땐 반드시 안전한 힐팩으로
                else moveTo(mClosestHeal, false, 1); // 체력 아이템으로 이동

            }
            else if (getHpPercentage() > getAmmoPercentage() && mClosestAmmo != -1) //총알이 더 부족
            {
                if (mVisibleEnemy != -1) moveTo(mSafestAmmo, true, 2); //적이 보일 땐 반드시 안전한 총알로
                else moveTo(mClosestAmmo, false, 3); // 총알 아이템으로 이동
            }
            else //상태가 좋지 않지만 아이템이 존재하지 않을 때
            {
                //Stop
            }
        }
        else //상태가 괜찮을 때 적을 찾는다
        {
            if (mClosestTeammate != -1)  //팀원이 존재하면
            {
                if (Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestTeammate].transform.position) > 10) //멀면 팀원에게 이동
                {
                    moveTo(mClosestTeammate, false, 4);
                }
                else //모였으면
                {
                    if (mClosestOccupy != -1)//점령전이면
                    {
                        moveTo(mClosestOccupy, false, 5);

                    }
                    else if (mClosestEnemy != -1)  //데스매치면
                    {
                        moveTo(mClosestEnemy, false, 6);
                    }
                }
            }
            else //팀원이 존재하지 않으면
            {
                if (mClosestOccupy != -1)//점령전이면
                {
                    moveTo(mClosestOccupy, false, 7);

                }
                else if (mClosestEnemy != -1)  //데스매치면
                {
                    moveTo(mClosestEnemy, false, 8);
                }
            }

        }





    }


}

