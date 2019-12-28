// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct PassiveMadnessComponent : IComponentData {
    public float value; // 일정 시간마다 차는 광기
    public float tickTime; // 광기가 차는 시간
    public float duration; // 지속 시간
    private float _elapsedDuration;
    private float _elapsedTickTime;


    public float ElapsedDuration {
        get => _elapsedDuration;
        set => _elapsedDuration = value;
    }

    public float ElapsedTickTime {
        get => _elapsedTickTime;
        set => _elapsedTickTime = value;
    }

    public PassiveMadnessComponent(PassiveMadnessComponent rhs) {
        value = rhs.value;
        tickTime = rhs.tickTime;
        duration = rhs.duration;
        _elapsedDuration = 0.0f;
        _elapsedTickTime = 0.0f;
    }
}
