﻿// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct PlayerComponent : IComponentData {
    public float playerDirection;        // -1 or 1
    public int currentBehaviors;		 // GlobalDefine.BehaviorState 참고
    public AnimationType currentAnim;

    public PlayerComponent(float initialDir) {
        playerDirection = initialDir;
        currentBehaviors = 0;
        currentAnim = AnimationType.Idle;
    }
}

[Serializable]
public struct PlayerStatusComponent : IComponentData {
    public int health;          // HP
    public float madness;       // 광기
    public float maxMadness;    // max 광기
    public float agility;       // 행동에 속도를 결정하는 수치. 이동속도 등.
    public float physical;      // 힘으로 하는 행동에 영향을 미치는 수치. 올드원 밀어내기.
    public float mentality;     // Madness 수치가 차 오르는 속도를 낮춘다.
    public float search;        // 아이템을 찾아내는 능력
    public float luck;          // 시나리오 선택 하고 진입시에 몇몇 랜덤 리스폰 영향
    private float _moveSpeed;
    private float _madnessWeight;
    private float _searchingWeight;

    public float MoveSpeed {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    public float MadnessWeight {
        get => _madnessWeight;
        set => _madnessWeight = value;
    }

    public float SearchingWeight {
        get => _searchingWeight;
        set => _searchingWeight = value;
    }

    public PlayerStatusComponent(ref PlayerStatusComponent rhs) {
        health = rhs.health;
        madness = rhs.madness;
        maxMadness = rhs.maxMadness;
        agility = rhs.agility;
        physical = rhs.physical;
        mentality = rhs.mentality;
        search = rhs.search;
        luck = rhs.luck;
        _moveSpeed = 1.0f;
        _madnessWeight = 1.0f;
        _searchingWeight = 1.0f;
    }
}
