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

            if (mClosestOccupy != -1)//점령전이면
            {
                moveTo(mClosestOccupy, false, 4);

            }
            else if(mClosestEnemy != -1)  //데스매치면
            {
                moveTo(mClosestEnemy, false, 5);
                /*
                if (mClosestAmmo != -1 || mClosestHeal != -1) // 아이템이 존재
                {
                    if (mClosestAmmo != -1 && mClosestHeal == -1) //총알 아이템만 존재한다
                    {
                        moveTo(mClosestAmmo, false, lCommandID++); //총알로 이동
                    }
                    else if (mClosestAmmo == -1 && mClosestHeal != -1) //체력 아이템만 존재한다
                    {
                        moveTo(mClosestHeal, false, lCommandID++); //체력으로 이동
                    }
                    else //둘다 존재한다
                    {
                        if (Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestAmmo].transform.position)
                                       < Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[mClosestHeal].transform.position))
                        {
                            moveTo(mClosestAmmo, false, lCommandID++); // 가장 가까운 아이템 하나
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

