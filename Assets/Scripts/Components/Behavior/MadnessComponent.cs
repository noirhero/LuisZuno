// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct MadnessComponent : IComponentData {
    public float value;                     // 차야하는 광기
    public float duration;                  // 차는 시간
    public float transitionStartValue;      // 광기가 오르기 전 시작값
    public float elapsedTransitionTime;
    public float valueForInterrupted;       // 진행 중에 또 광기를 받아야할 경우 받아야 할 남은 광기값
}
