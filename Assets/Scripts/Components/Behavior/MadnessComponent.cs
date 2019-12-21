// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct MadnessComponent : IComponentData {
    public float value;                     // 차야하는 광기
    public float duration;                  // 차는 시간
    public float transitionStartValue;      // 광기가 오르기 전 시작값
    public float elapsedTransitionTime;
    public float valueForInterrupted;       // 진행 중에 또 광기를 받아야할 경우 이전의 목표값을 바로 적용

    public float linearValue;               // 일정 시간마다 선형으로 차야하는 광기
    public float linearTickTime;            // 일정 시간
    public float linearDuration;            // 지속 시간
    public float elapsedLinearTransitionTime;
}
