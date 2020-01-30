// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using GlobalDefine;

public class AnimStateSystem : JobComponentSystem {
    private int[] _animNameHashes;
    protected override void OnCreate() {
        var animTypes = Enum.GetValues(typeof(AnimationType));
        _animNameHashes = new int[animTypes.Length];
        foreach (AnimationType type in animTypes) {
            _animNameHashes[(int) type] = type.ToString().Sum(Convert.ToInt32);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var animNameHashes = new NativeArray<int>(_animNameHashes, Allocator.TempJob);
        return Entities
            .WithName("AnimStateSystem")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref SpriteStateComponent state, in PlayerComponent player) => {
                state.hash = animNameHashes[(int) player.currentAnim];
            })
            .WithDeallocateOnJobCompletion(animNameHashes)
            .Schedule(inputDependencies);
    }
}
