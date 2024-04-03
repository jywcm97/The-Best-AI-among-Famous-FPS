using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RespawnPlaceBase_OWRespawnPlace : ObjectBase_RespawnPlaceBase
{
    protected override void Awake()
    {
        base.Awake();
        mPlaceOwner = new AIBase_OW();
    }
}
