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
        var playerAnimHandle = Entities
            .WithName("AnimStateSystem_Player")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref SpriteStateComponent state, in PlayerComponent player) => {
                state.hash = animNameHashes[(int)player.currentAnim];
            })
            .Schedule(inputDependencies);

        return Entities
            .WithName("AnimStateSystem_NPC")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref SpriteStateComponent state, in NPCComponent npc) => {
                state.hash = animNameHashes[(int)npc.currentAnim];
            })
            .WithDeallocateOnJobCompletion(animNameHashes)
            .Schedule(playerAnimHandle);
    }
}
