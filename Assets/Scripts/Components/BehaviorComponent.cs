// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct BehaviorComponent : IComponentData {
}

[Serializable]
public struct HoldingComponent : IComponentData {
}

[Serializable]
public struct MadnessComponent : IComponentData {
    public float value;                     // 차야하는 광기
    public float duration;                  // 차는 시간
    public float transitionStartValue;      // 광기가 오르기 전 시작값
    public float elapsedTransitionTime;
    public float valueForInterrupted;       // 진행 중에 또 광기를 받아야할 경우 받아야 할 남은 광기값
}

[Serializable]
public struct PanicComponent : IComponentData {
    public float panicTime;
    public float elapsedPanicTime;
    public AnimationType panicAnim;

    // 패닉 시스템에서 매드니스 컴포넌트를 붙여주기 위한 전달값
    public float madness;
    public float madnessDuration;
}

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

[Serializable]
public struct PendingItemComponent : IComponentData {
    public Int64 pendingItemID;
}

[Serializable]
public struct SearchingComponent : IComponentData {
    public float searchingTime;
    public float elapsedSearchingTime;
    public AnimationType searchingAnim;
}

[Serializable]
public struct TurningComponent : IComponentData {
}
