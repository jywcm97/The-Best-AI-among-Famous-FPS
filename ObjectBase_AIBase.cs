using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class ObjectBase_AIBase : ObjectBase
{


    public int mDestinationItemNumber = -1; //�̵� ��ǥ ������
    public Vector3 mDestinationPositionThen = new Vector3(); //�̵��Ϸ��� ����� �̵� ��ǥ �������� ��ġ (��������� Destination -1)

    bool mIsFixed = false;
    //bool mIsIter = true;
    int mCommandID = -1;

    private int mBeAttacked_By_Enemy = -1; //���� ���� ������ ��


    public bool mIsRespawning = false;
    public int mCurrentHP = 0;

    public GameData.Weapon mUsingWeapon = GameData.Weapon.None; //�����ִ� �� ����
    public Dictionary<GameData.Weapon, int> mCurrentAmmo = new Dictionary<GameData.Weapon, int>(); // �� ������ ���� �Ѿ� ��� ��ųʸ�

    private float mShootTimer;
    private bool mShootisPossible = false;


    protected override void Awake()
    {
        base.Awake();
        mObjectType = GameData.ObjectType.AI;
    }



    protected override void Start()
    {
        base.Start();
        respawn(); //������ �����ϸ� ������ ��Ų�� **************


    }


    protected override void Update()
    {
        base.Update();
        if (this.gameObject.activeSelf == false) return;
        checkReachedItem();
        if (Vector3.Distance(mDestinationPositionThen, this.gameObject.transform.position) <= 0.5f)  reachedDestination();
        checkShootDelay();
        updateMotion();
        think();

    }
    public void checkReachedItem() //�̵��ϸ� �� ���� �����۵�
    {
        foreach(var pair in GameManager.mAll_Of_Game_Objects)
        {
            ObjectBase_ItemBase lItem = pair.Value.GetComponent<ObjectBase_ItemBase>();
            if (lItem == null) continue;
            if (Vector3.Distance(pair.Value.transform.position, this.gameObject.transform.position) >= 0.5f) continue;
            if (!pair.Value.activeSelf) continue;

            lItem.action(mID);
        }
    }
    public void reachedDestination() //�̵� ��ǥ���� ����� ��
    {
        
        mDestinationItemNumber = -1;
        //mIsForced = false;
        //mIsIter = true;
        mCommandID = -1;
        mIsFixed = false;
    }


    void checkShootDelay()
    {
        mShootTimer += Time.deltaTime;
        if (!mShootisPossible && GameData.mWeaponDataDictionary[mUsingWeapon].mShootDelay < mShootTimer)
        {
            mShootisPossible = true;
            mShootTimer = 0.0f;
        }
    }

    public void updateMotion()
    {
        if (this.gameObject.GetComponent<NavMeshAgent>().velocity.magnitude < 1)
        {

            this.gameObject.GetComponent<Animator>().SetBool("isMoving", false);
            this.gameObject.GetComponent<Animator>().SetBool("isHolding", true);

        }
        else
        {
            this.gameObject.GetComponent<Animator>().SetBool("isMoving", true);
            this.gameObject.GetComponent<Animator>().SetBool("isHolding", false);
        }
    }


    virtual public void think()
    {

    }


    #region Ž��
    public virtual bool isVisible(int pItemNumber)
    {
        Collider lMyCollider = this.gameObject.GetComponent<Collider>();
        Collider lTargetCollider = GameManager.mAll_Of_Game_Objects[pItemNumber].GetComponent<Collider>();

        Vector3 lDirectionToEnemy = (lTargetCollider.bounds.center - lMyCollider.bounds.center).normalized; //pItemNumber ��������

        if (Vector3.Angle(transform.forward, lDirectionToEnemy) < 90) // �þ߰� 180������ ���̸鼭
        {
            // ����ĳ��Ʈ�� �þ� ���� ���� ��ֹ� ���� Ȯ��
            RaycastHit hit;

            if (Physics.Raycast(lMyCollider.bounds.center, lDirectionToEnemy, out hit, 100)) // pItemNumber �������� ����� �� �¾Ҵµ�
            {
                if (hit.collider.gameObject == GameManager.mAll_Of_Game_Objects[pItemNumber]) //������ pItemNumber��
                {
                    return true;
                    
                }
            }
        }

        return false;
    }

    public virtual int searchItemNumber(int pID, GameData.SearchType pSearchType, GameData.ObjectType pObjectType, GameData.TeamType pTeamType = GameData.TeamType.All)
    {

        // �˻����ɿ��� ����
        var lIsSearchable = new List<ObjectBase>();

        foreach (var pair in GameManager.mAll_Of_Game_Objects)
        {


            //������ �ֵ��� ����, Ÿ�Կ� �´� �͸� true

            if (!pair.Value.activeSelf) continue; // ���� �ִ� ã�� �ʴ´�
            if (pair.Value.GetComponent<ObjectBase>() == null) continue; // ObjectBase ������Ʈ�� ���� ��� �Ѿ��.
            if (pair.Value.GetComponent<ObjectBase>().mID == pID) continue;// �ڱ� �ڽ��� ã�Ҵٸ� �Ѿ��.
            if (pair.Value.GetComponent<ObjectBase>().mObjectType != pObjectType) continue;// ã���� �ϴ� ������Ʈ Ÿ���� �ƴϸ� �Ѿ��.


            // ���ϴ� ������Ʈ Ÿ���� �ƴϸ� �Ѿ��.

            if (pObjectType == GameData.ObjectType.AI) //AI�� �� ������ �ʿ䰡 ����
            {

                if (pTeamType == GameData.TeamType.Teammate)
                {
                    if (pair.Value.GetComponent(this.GetType()) != null)
                    {
                        lIsSearchable.Add(pair.Value.GetComponent<ObjectBase>());
                    }
                }
                else if (pTeamType == GameData.TeamType.Enemy)
                {
                    if (pair.Value.GetComponent(this.GetType()) == null)
                    {
                        lIsSearchable.Add(pair.Value.GetComponent<ObjectBase>());

                    }
                }
                else if(pTeamType == GameData.TeamType.All)
                {
                        lIsSearchable.Add(pair.Value.GetComponent<ObjectBase>());

                }
            }

            else if (pObjectType == GameData.ObjectType.RespawnPlace) //������ ������ �Ʊ��� �������� ���� �������� ������ �ʿ䰡 ����
            {


                //All�� ��쿣 �ƹ��͵� ��Ƽ�� ���� �ʴ´�.

                if (pTeamType == GameData.TeamType.Teammate)
                {
                    //null�̸� �ڵ����� ��Ƽ�� �ȴ�. 
                    if (pair.Value.GetComponent<ObjectBase_RespawnPlaceBase>().mPlaceOwner.GetType() == this.GetType())
                    {
                        lIsSearchable.Add(pair.Value.GetComponent<ObjectBase>());

                    }
                }
                else if (pTeamType == GameData.TeamType.Enemy)
                {
                    if (pair.Value.GetComponent<ObjectBase_RespawnPlaceBase>().mPlaceOwner.GetType() != this.GetType())
                    {
                        lIsSearchable.Add(pair.Value.GetComponent<ObjectBase>());

                    }
                }
                else if(pTeamType == GameData.TeamType.All)
                {
                    lIsSearchable.Add(pair.Value.GetComponent<ObjectBase>());

                }



            }
            else //���� �����۵�
            {
                lIsSearchable.Add(pair.Value.GetComponent<ObjectBase>());

            }



        }




        ////////////�˻�


        int lSearchItemNumber = -1;

        switch (pSearchType)
        {
            case GameData.SearchType.Visible: // Ÿ�Կ� �´� ���̴� ObjectBase�� �ڽ����� ���� GameObject Ž��
                Collider lMyCollider = GameManager.mAll_Of_Game_Objects[pID].GetComponent<Collider>();
                RaycastHit hit;

                foreach (var obj in lIsSearchable) //TODO JYW ���̴� �� �߿��� �׷��� ���� ����� ���� �����ϵ��� �ٲ���Ѵ�. Visible�� ���̴°� + ����� ������ ������.
                {
                    ObjectBase lObjectBase = obj;

                    Vector3 lDirectionToEnemy = (lObjectBase.gameObject.GetComponent<Collider>().bounds.center - lMyCollider.bounds.center).normalized;

                    if (Vector3.Angle(transform.forward, lDirectionToEnemy) > 90) continue; // �þ߰� 180�� �Ѿ�� ��Ƽ��
                    if (!Physics.Raycast(lMyCollider.bounds.center, lDirectionToEnemy, out hit, 200)) continue;// �ƹ��͵� �ȸ¾����� ��Ƽ��
                    if (hit.collider.gameObject != GameManager.mAll_Of_Game_Objects[lObjectBase.mID]) continue; //������ ��ǥ���� �ƴϸ� ��Ƽ��

                    lSearchItemNumber = hit.collider.GetComponent<ObjectBase>().mID;
                }
                break;
            case GameData.SearchType.Closest: // Ÿ�Կ� �´� ����� ObjectBase�� �ڽ����� ���� GameObject Ž��

                float lClosestDistance = float.MaxValue;

                foreach (var obj in lIsSearchable)
                {

                    float distance = Vector3.Distance(GameManager.mAll_Of_Game_Objects[pID].transform.position, obj.gameObject.transform.position);

                    if (distance < lClosestDistance)
                    {
                        lSearchItemNumber = obj.mID;
                        lClosestDistance = distance;
                    }
                }

                break;

            case GameData.SearchType.Farthest: //Ÿ�Կ� �´� �ָ��ִ� ObjectBase�� �ڽ����� ���� GameObject Ž��

                float lFarthestDistance = float.MinValue;
                foreach (var obj in lIsSearchable)
                {

                    float distance = Vector3.Distance(GameManager.mAll_Of_Game_Objects[pID].transform.position, obj.gameObject.transform.position);

                    if (distance > lFarthestDistance)
                    {
                        lSearchItemNumber = obj.mID;
                        lFarthestDistance = distance;
                    }
                }
                break;

            case GameData.SearchType.Safe: // Ÿ�Կ� �´� ���� �ָ� �������� ���� ����� ObjectBase�� �ڽ����� ���� GameObject Ž��
                int lVisibleEnemy = searchItemNumber(pID, GameData.SearchType.Visible, GameData.ObjectType.AI, GameData.TeamType.Enemy);
                if (lVisibleEnemy == -1) return -1; //���� �� ���ִ� ���� �־�� �Ѵ�.

                float lSafestDistence = float.MaxValue; //���� �����鼭 ������ �������� ã�ƾ��Ѵ�. ���ʰ��� ����

                foreach (var obj in lIsSearchable)
                {
                                                                                             // ���ϴ� ������Ʈ Ÿ���� �ƴϸ� �Ѿ��.

                    float lMeToEnemyDistance = Vector3.Distance(GameManager.mAll_Of_Game_Objects[pID].transform.position, GameManager.mAll_Of_Game_Objects[lVisibleEnemy].transform.position);
                    float lEnemyToItemDistance = Vector3.Distance(GameManager.mAll_Of_Game_Objects[lVisibleEnemy].transform.position, obj.gameObject.transform.position);
                    float lMeToItemDistance = Vector3.Distance(GameManager.mAll_Of_Game_Objects[pID].transform.position, obj.gameObject.transform.position);

                    if (lEnemyToItemDistance > lMeToEnemyDistance && lMeToItemDistance < lSafestDistence) //������ �Ÿ����� ���� �������� �Ÿ��� �� �ָ� ���� ���� && �� �� ����� ������
                    {
                        lSearchItemNumber = obj.mID;
                        lSafestDistence = lMeToItemDistance;
                    }
                }

                break;
            case GameData.SearchType.Random: // Ÿ�Կ� �´� ���� ObjectBase�� �ڽ����� ���� GameObject Ž��


                if (lIsSearchable.Count == 0) break;
                lSearchItemNumber = lIsSearchable[UnityEngine.Random.Range(0, lIsSearchable.Count)].mID; //���� �ε��� + ���� ��ũ

                break;
        }
        return lSearchItemNumber; // ã�� �������� -1�� ��ȯ
    }

    /*

    public virtual int searchClosestObject(GameData.ObjectType pObjectType, GameData.TeamType pTeamType = GameData.TeamType.None)
    {
        float closestDistance = float.MaxValue;
        int itemNumber = -1;

        for (int i = 0; i < GameManager.mAll_Of_Game_Objects.Count; i++)
        {
            if (!GameManager.mAll_Of_Game_Objects[i].activeSelf
                || GameManager.mAll_Of_Game_Objects[i] == this.gameObject
                || GameManager.mAll_Of_Game_Objects[i].GetComponent<ObjectBase>() == null
                || GameManager.mAll_Of_Game_Objects[i].GetComponent<ObjectBase>().mObjectType != pObjectType) continue; // ���� �ִ� ã�� �ʴ´�.// �ڱ� �ڽ��� ã�Ҵٸ� �Ѿ��. // ObjectBase ������Ʈ�� ���� ��쵵 ó��
            // ���ϴ� ������Ʈ Ÿ���� �ƴϸ� �Ѿ��.

            float distance = Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[i].transform.position);

            if (distance < closestDistance)
            {
                itemNumber = i;
                closestDistance = distance;
            }
        }

        return itemNumber; // �� ã������ -1 ��ȯ
    }


    public virtual int searchSafeObject(GameData.ObjectType pObjectType, GameData.TeamType pTeamType = GameData.TeamType.None) // �������κ��� ���� �ְ� ���κ��� ���� ����� ������ �������� ã�´�.
    {
        if (mBeAttacked_By_Enemy == -1) return -1; //���� ������ ����� �����ؾ� �Ѵ�.

        float lFarthestDistance = float.MinValue;
        int itemNumber = -1;

        for (int i = 0; i < GameManager.mAll_Of_Game_Objects.Count; i++)
        {
            if (!GameManager.mAll_Of_Game_Objects[i].activeSelf
                || GameManager.mAll_Of_Game_Objects[i] == this.gameObject
                || GameManager.mAll_Of_Game_Objects[i].GetComponent<ObjectBase>() == null
                || GameManager.mAll_Of_Game_Objects[i].GetComponent<ObjectBase>().mObjectType != pObjectType) continue; // ���� �ִ� ã�� �ʴ´�.// �ڱ� �ڽ��� ã�Ҵٸ� �Ѿ��. // ObjectBase ������Ʈ�� ���� ��쵵 ó��
            // ���ϴ� ������Ʈ Ÿ���� �ƴϸ� �Ѿ��.

            float lEnemyToItemDistance = Vector3.Distance(GameManager.mAll_Of_Game_Objects[mBeAttacked_By_Enemy].transform.position, GameManager.mAll_Of_Game_Objects[i].transform.position);
            float lMeToItemDistance = Vector3.Distance(this.gameObject.transform.position, GameManager.mAll_Of_Game_Objects[i].transform.position);

            if (lEnemyToItemDistance - lMeToItemDistance > lFarthestDistance)
            {
                itemNumber = i;
                lFarthestDistance = lEnemyToItemDistance - lMeToItemDistance;
            }
        }

        return itemNumber; // �� ã������ -1 ��ȯ
    }


    public virtual int searchRandomObject(GameData.ObjectType pObjectType, GameData.TeamType pTeamType = GameData.TeamType.None)
    {
        List<int> correctObjects = new List<int>();

        for (int i = 0; i < GameManager.mAll_Of_Game_Objects.Count; i++)
        {
            ObjectBase objectBase = GameManager.mAll_Of_Game_Objects[i].GetComponent<ObjectBase>();

            if (objectBase != null && objectBase.mObjectType == pObjectType)
            {
                correctObjects.Add(i);
            }
        }

        if (correctObjects.Count == 0)
        {
            // No matching objects found, return a default value or handle it as needed
            return -1; // Default value, change it if needed
        }

        int randomIndex = UnityEngine.Random.Range(0, correctObjects.Count);
        int randomObjectNumber = correctObjects[randomIndex];

        return randomObjectNumber + mRandomMark;
    }


    public int searchVisibleObject(GameData.ObjectType pObjectType, GameData.TeamType pTeamType = GameData.TeamType.None)
    {
        // �Ϲ� �þ�
        ObjectBase[] allObjectBases = GameObject.FindObjectsOfType<ObjectBase>();
        Collider lMyCollider = GetComponent<Collider>();

        foreach (var objObjectBase in allObjectBases)
        {
            if (!GameManager.mAll_Of_Game_Objects[objObjectBase.mID].activeSelf
                || GameManager.mAll_Of_Game_Objects[objObjectBase.mID] == this.gameObject
                || objObjectBase == null
                || objObjectBase.mObjectType != pObjectType) continue; // ���� �ִ� ã�� �ʴ´�.// �ڱ� �ڽ��� ã�Ҵٸ� �Ѿ��. // ObjectBase ������Ʈ�� ���� ��쵵 ó��
                                                                       // ���ϴ� ������Ʈ Ÿ���� �ƴϸ� �Ѿ��.

            Collider lTargetCollider = objObjectBase.gameObject.GetComponent<Collider>();

            Vector3 directionToEnemy = (lTargetCollider.bounds.center - lMyCollider.bounds.center).normalized;

            if (Vector3.Angle(transform.forward, directionToEnemy) < 90) // �þ߰� 180������ ���̸鼭
            {
                // ����ĳ��Ʈ�� �þ� ���� ���� ��ֹ� ���� Ȯ��
                RaycastHit hit;

                if (Physics.Raycast(lMyCollider.bounds.center, directionToEnemy, out hit, 100)) // ����� �� �¾Ҵµ�
                {
                    if (hit.collider.gameObject == GameManager.mAll_Of_Game_Objects[objObjectBase.mID]) //������ ��ǥ���̸�
                    {
                        return hit.collider.GetComponent<ObjectBase>().mID; // �ش� AI�� ������ �ѹ��� ��ȯ�Ѵ�
                    }
                }
            }
        }

        return -1; // ã�� �������� -1�� ��ȯ
    }

    */


    #endregion


    #region ����
    public void obtainWeapon(GameData.Weapon pWeapon)
    {


            mUsingWeapon = pWeapon; //���� ���⸦ ��� �Ѵ�.

        if (mCurrentAmmo.ContainsKey(pWeapon)) //������ �ִ� ����� ���� �Ѿ� ���Ѵ�
        {
            mCurrentAmmo[pWeapon] += GameData.mWeaponDataDictionary[pWeapon].mInitBullets;

        }
        else  //������ ���� ���� ����� ���� �Ѿ� �����Ѵ�.
        {
            mCurrentAmmo[pWeapon] = GameData.mWeaponDataDictionary[pWeapon].mInitBullets;
        }

    }
    public bool changeWeapon(GameData.Weapon pWeapon)
    {
        if (mCurrentAmmo.ContainsKey(pWeapon)) //������ �ִ� �����
        {
            mUsingWeapon = pWeapon; //���� ����
            return true;//�� ����Ǿ��� �� true
        }
        else
        {
            return false;
        }



    }
    public void resetWeapon()
    {
        mUsingWeapon = GameData.Weapon.None;

        mCurrentAmmo.Clear();

    }
    #endregion


    #region ����

    public float getHpPercentage()
    {
        return (float)mCurrentHP / (float)GameData.mMaxHP;
    }

    public float getAmmoPercentage()
    {
        return (float)mCurrentAmmo[mUsingWeapon] / (float)GameData.mWeaponDataDictionary[mUsingWeapon].mMaxBullets;

    }

    public void attacked(int pBeShotID) //���� pID�� �������� ��
    {
        reachedDestination(); //�����ϸ� ����������
    }

    public void beAttackedBy(int pShooterID)
    {
        reachedDestination(); //���� ���ϸ� ����������

        ObjectBase_AIBase lShooterAI = GameManager.mAll_Of_Game_Objects[pShooterID].GetComponent<ObjectBase_AIBase>();

        this.gameObject.transform.LookAt(lShooterAI.gameObject.transform); //���� ������ �ָ� �ٶ󺻴�.
        mBeAttacked_By_Enemy = pShooterID; //���� ������ �ִ� pShooterID
        mCurrentHP -= GameData.mWeaponDataDictionary[lShooterAI.mUsingWeapon].mDamage; // �� ���� ��������ŭ ü�� ��´�

        if (mCurrentHP <= 0) //�����׾�����
        {
            lShooterAI.killed(mID);
            bekilledBy(pShooterID);
        }
    }
    public virtual void killed(int pDeadID) //���� �������� �׿��� ��
    {
        reachedDestination();

        GameDataSaver.SaveKillDeathResultsToCSV(this.gameObject.name, GameManager.mAll_Of_Game_Objects[pDeadID].name);
        //refreshDestination();// ���� ���󰡴ٰ� ���� ������ �ѹ� �����ߴٰ� ���ΰ�ħ ����� �ٸ� �Ǵ��� �Ѵ�.
        //���������� �̵��ϴٰ� ���� �׾ �Ǵ��Ͽ� ��� ���������� �� ��
    }
    public virtual void bekilledBy(int pKillerID) //���� �׾��� ��
    {
        reachedDestination();

        this.gameObject.SetActive(false);
    }


    public virtual void respawn()
    {

        if(searchItemNumber(mID, GameData.SearchType.Random, GameData.ObjectType.RespawnPlace, GameData.TeamType.Teammate) != -1) // �� ���� ������ ������ �ִ°�?
        {
            this.gameObject.transform.position = GameManager.mAll_Of_Game_Objects[searchItemNumber(mID, GameData.SearchType.Random, GameData.ObjectType.RespawnPlace, GameData.TeamType.Teammate)].transform.position;
        }
        else
        {
            this.gameObject.transform.position = GameManager.mAll_Of_Game_Objects[searchItemNumber(mID, GameData.SearchType.Random, GameData.ObjectType.RespawnPlace, GameData.TeamType.All)].transform.position;
        }



        this.gameObject.SetActive(true);


        mCurrentHP = GameData.mMaxHP;
        resetWeapon();
        obtainWeapon(GameData.Weapon.Pistol);







    }


    #endregion

    public void shoot(int pID)
    {
        if (pID < 0) return; //�ε���
        transform.LookAt(GameManager.mAll_Of_Game_Objects[pID].transform); //�ٶ󺸰� �Ѵ�

        if(!mShootisPossible) return; //��Ÿ��
        mShootisPossible = false;

        if (mCurrentAmmo[mUsingWeapon] <= 0) return; //�Ѿ�
        mCurrentAmmo[mUsingWeapon]--;

        Collider lMyCollider = GetComponent<Collider>();
        Collider lTargetCollider = GameManager.mAll_Of_Game_Objects[pID].GetComponent<Collider>();

        Vector3 shotDirection = lTargetCollider.bounds.center - lMyCollider.bounds.center; //��Ȯ�� ����

        float angleRange;
        //if (this.gameObject.GetComponent<NavMeshAgent>().velocity == Vector3.zero) angleRange = 0f; // ���� �� �� ��Ȯ�ϴ�
        //else angleRange = 0.5f;

        angleRange = 0.0f;

        Quaternion yawRotation = Quaternion.AngleAxis(Random.Range(-angleRange, angleRange), Vector3.up); //���� ���� ����
        Quaternion pitchRotation = Quaternion.AngleAxis(Random.Range(-angleRange, angleRange), Vector3.right); //�¿� ���� ����
        Vector3 imprecision = (yawRotation * pitchRotation) * shotDirection.normalized; // ���� ��� ����

        Ray ray = new Ray(lMyCollider.bounds.center, shotDirection.normalized + imprecision); //���� + ���� ��� ����

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 200)) return; //�ƹ��͵� �ȸ¾����� ����
        Debug.DrawLine(lMyCollider.bounds.center, hit.point, Color.red, 1f); // ���� ������ ������ ������ ǥ��

        if (hit.collider.gameObject.GetComponent<ObjectBase_AIBase>() == null) return;//AI�� ������ �ƴϸ� ����
        attacked(hit.collider.gameObject.GetComponent<ObjectBase_AIBase>().mID); //hit.ID �� �����ߴ�
        hit.collider.gameObject.GetComponent<ObjectBase_AIBase>().beAttackedBy(mID); //�� ID�κ��� ���ݴ��ߴ�

        // �Ѿ� �Ҹ�
    }


    public float calculateNavMeshPathDistance(Vector3 sourcePosition, Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(sourcePosition, targetPosition, NavMesh.AllAreas, path))
        {
            // ����� �� �Ÿ��� �ʱ�ȭ�մϴ�.
            float totalDistance = 0.0f;

            // ��� ���̰� ����� ���� Ȯ���մϴ�.
            if (path.corners.Length > 1)
            {
                // ����� ��� �ڳʸ� ��ȸ�ϸ� �Ÿ��� ����մϴ�.
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    // ���ӵ� �� �ڳ� ������ �Ÿ��� ����մϴ�.
                    Vector3 segment = path.corners[i + 1] - path.corners[i];
                    totalDistance += segment.magnitude;
                }
            }

            return totalDistance;
        }
        else
        {
            // ��θ� ã�� �� ���� ���, -1�� ��ȯ�Ͽ� ��� ������ ��Ÿ���ϴ�.
            return 1.0f;
        }
    }





    public void changeSpeed(float pAngularSpeed, float pSpeed, float pAcceleration, float pStoppingDistance) //������ ����
    {
        this.gameObject.GetComponent<NavMeshAgent>().angularSpeed = pAngularSpeed; // ȸ�� �ӵ�
        this.gameObject.GetComponent<NavMeshAgent>().speed = pSpeed; // �ִ� �ӵ�
        this.gameObject.GetComponent<NavMeshAgent>().acceleration = pAcceleration; // ���ӵ�
        this.gameObject.GetComponent<NavMeshAgent>().stoppingDistance = pStoppingDistance; // ���ߴ� �Ÿ�
    }





    public void moveStop()
    {
        this.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

    }

    public void moveLeftRight()
    {

    }

    public void moveTo(int pID, bool pIsFixed, int pCommandID) //�� �Լ��� ���� ���� ����ϰ� �ؾ���
    {


        //TODO JYW 1. ������ų ���ΰ� 2. �ݺ� �����Ѱ�, 3. ID //�ݺ���ų ���ΰ��� ������ �� ������ �Դٰ�����
        //������ų���ΰ��� �޾�����
        if (mIsFixed) return; //���� �������� �������� �����Ǿ��ٸ� � ����̵� �����ϱ� ������ �����Ѵ�.(��: ������ ������ �̵��� �� ���� �Ⱥ��̰� �ǵ� �ϴ� ������ ������ �����Ѵ�) // Ư���� ��Ȳ��
        if (pCommandID == mCommandID) return; //���� ���ID�� ������ ����

        if (pID == -1) return; // -1�� ��ã�� ����̹Ƿ� ������� ����

        mIsFixed = pIsFixed;
        mCommandID = pCommandID;
        mDestinationItemNumber = pID; // pID�� ���� ��ǥ, ���������̰ų� �����ϰų� ���ݹްų� �װų� ���̸� -1�� ����ȴ�.
        mDestinationPositionThen = GameManager.mAll_Of_Game_Objects[mDestinationItemNumber].transform.position;
        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(mDestinationPositionThen);
    }


}

