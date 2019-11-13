// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct AvatarStatusComponent : IComponentData {
    public int health;
    public int madness;
    public int maxMadness;
    public float agility;
    public float eyeSight;
    private float _moveSpeed;
    private bool _bInPanic;

    public bool InPanic {
        get => _bInPanic;
        set => _bInPanic = value;
    }

    public float MoveSpeed {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    public AvatarStatusComponent(ref AvatarStatusComponent rhs) {
        health = rhs.health;
        madness = rhs.madness;
        maxMadness = rhs.maxMadness;
        agility = rhs.agility;
        eyeSight = rhs.eyeSight;
        _moveSpeed = 1.0f; //rhs.moveSpeed;
        _bInPanic = false;
    }
}
