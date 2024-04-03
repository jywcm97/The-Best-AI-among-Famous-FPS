using System.Collections.Generic;

public class GameData
{

    public enum ObjectType : int
    {
        None, // �ʱⰪ, ���� ������ �ʴ´�.
        Ammo,
        Heal,
        AI,
        RespawnPlace,
        OccupyPlace,
        Weapon
    }
    public enum SearchType : int
    {
        None,
        Visible,
        Closest,
        Farthest,
        Safe,
        Random
    }

    public enum TeamType : int
    {
        All, // �ʱⰪ, searchITem���� Ammo�� ã�� �� All�� ��� ���� ã�´�, Ammo�� �������� ����.
        Teammate,
        Enemy,
    }
    

    public enum Weapon : int
    {
        None,
        Rifle,
        Pistol,
        Rocket,
        Cnt
    }

    public enum MotionType : int
    {
        Moving,
        Holding,
    }

    public struct WeaponData
    {
        public int mDamage;
        public int mMaxBullets;
        public int mInitBullets;
        public float mShootDelay;

    }

    public static Dictionary<Weapon, WeaponData> mWeaponDataDictionary = new Dictionary<Weapon, WeaponData>(); //���� ��ųʸ� //���� ������ Ű�� �޾� ���� ������ �������ش�.
    public const int mMaxHP = 100;

    // static ������
    static GameData() //������ ���� ��ġ
    {
        mWeaponDataDictionary[Weapon.Pistol] = new WeaponData
        {
            mDamage = 10,
            mMaxBullets = 30,
            mInitBullets = 15,
            mShootDelay = 0.2f
        };
        mWeaponDataDictionary[Weapon.Rifle] = new WeaponData
        {
            mDamage = 10,
            mMaxBullets = 60,
            mInitBullets = 30,
            mShootDelay = 0.1f
        };
        mWeaponDataDictionary[Weapon.Rocket] = new WeaponData
        {
            mDamage = 50,
            mMaxBullets = 10,
            mInitBullets = 10,
            mShootDelay = 0.5f
        };

    }
}
