// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct AvatarStatusComponent : IComponentData {
    public int health;
    public float madness;
    public float maxMadness;
    public float agility;
    public float physical;
    public float mentality;
    public float search;
    public float luck;
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
        physical = rhs.physical;
        mentality = rhs.mentality;
        search = rhs.search;
        luck = rhs.luck;
        _moveSpeed = 1.0f;
        _bInPanic = false;
    }
}
