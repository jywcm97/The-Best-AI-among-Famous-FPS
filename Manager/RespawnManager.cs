using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RespawnManager
{
    private Dictionary<int, float> itemRespawnTimers = new Dictionary<int, float>();
    private Dictionary<int, float> aiRespawnTimers = new Dictionary<int, float>();


    public void Awake()
    {

    }
    public void Start()
    {

    }
    public void Update()
    {
        var currentTime = Time.time;

        // Handle item respawning
        foreach (var obj in GameManager.mAll_Of_Game_Objects)
        {
            if (obj.Value.GetComponent<ObjectBase_ItemBase>() == null) continue;

            if (!obj.Value.activeSelf && !obj.Value.GetComponent<ObjectBase_ItemBase>().mIsRespawning) //�ð� ���
            {
                obj.Value.GetComponent<ObjectBase_ItemBase>().mIsRespawning = true;
                itemRespawnTimers[obj.Value.GetComponent<ObjectBase>().mID] = currentTime + obj.Value.GetComponent<ObjectBase_ItemBase>().mRespawnCount;
            }
            else if (obj.Value.GetComponent<ObjectBase_ItemBase>().mIsRespawning
                && itemRespawnTimers.ContainsKey(obj.Value.GetComponent<ObjectBase>().mID)
                && currentTime >= itemRespawnTimers[obj.Value.GetComponent<ObjectBase>().mID]) //�ð� �Ϸ� ������
            {
                obj.Value.GetComponent<ObjectBase_ItemBase>().mIsRespawning = false;
                obj.Value.SetActive(true);
                itemRespawnTimers.Remove(obj.Value.GetComponent<ObjectBase>().mID);
            }
        }

        // Handle AI respawning
        foreach (var obj in GameManager.mAll_Of_Game_Objects)
        {
            if (obj.Value.GetComponent<ObjectBase_AIBase>() == null) continue;

            if (!obj.Value.activeSelf && !obj.Value.GetComponent<ObjectBase_AIBase>().mIsRespawning)
            {
                obj.Value.GetComponent<ObjectBase_AIBase>().mIsRespawning = true;
                aiRespawnTimers[obj.Value.GetComponent<ObjectBase>().mID] = currentTime + 10;
            }
            else if (obj.Value.GetComponent<ObjectBase_AIBase>().mIsRespawning
                && aiRespawnTimers.ContainsKey(obj.Value.GetComponent<ObjectBase>().mID)
                && currentTime >= aiRespawnTimers[obj.Value.GetComponent<ObjectBase>().mID])
            {
                obj.Value.GetComponent<ObjectBase_AIBase>().mIsRespawning = false;
                obj.Value.GetComponent<ObjectBase_AIBase>().respawn();
                aiRespawnTimers.Remove(obj.Value.GetComponent<ObjectBase>().mID);
            }
        }
    }
}
