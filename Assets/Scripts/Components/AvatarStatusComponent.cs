// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct AvatarStatusComponent : IComponentData {
    public Int32 health;
    public Int32 madness;
    public float agility;
    public float eyeSight;
    private float moveSpeed;
    private bool bInPanic;

    public bool InPanic {
        get { return bInPanic; }
        set { bInPanic = value; }
    }

    public float MoveSpeed {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public AvatarStatusComponent(ref AvatarStatusComponent rhs) {
        health = rhs.health;
        madness = rhs.madness;
        agility = rhs.agility;
        eyeSight = rhs.eyeSight;
        moveSpeed = 1.0f; //rhs.moveSpeed;
        bInPanic = false;
    }
}
