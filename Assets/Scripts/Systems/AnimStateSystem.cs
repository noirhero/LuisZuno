// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using System.Text;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using GlobalDefine;

public class AnimStateSystem : JobComponentSystem {
    private EntityQuery _group;
    private int[] _animNameHashes;

    protected override void OnCreate() {
        var query = new EntityQueryDesc() {
            All = new ComponentType[] {
                typeof(SpriteAnimComponent)
            },
            Options = EntityQueryOptions.FilterWriteGroup
        };
        _group = GetEntityQuery(query);

        // caching anim information
        int totalAnimCount = Enum.GetNames(typeof(AnimationType)).Length;
        _animNameHashes = new int[totalAnimCount];

        foreach (AnimationType type in Enum.GetValues(typeof(AnimationType))) {
            int nameHash = 0;
            foreach (var b in Encoding.ASCII.GetBytes(type.ToString())) {
                nameHash += b;
            }
            _animNameHashes[(int)type] = nameHash;
        }
    }

    [BurstCompile]
    struct AnimStateSystemJob : IJobChunk {
        public ArchetypeChunkComponentType<SpriteAnimComponent> animCompType;
        public int idleNameHash;
        public int walkNameHash;
        public int somethingDoItNameHash;
        public int nyonyoNameHash;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex) {
            var animations = chunk.GetNativeArray(animCompType);
            var nameHash = idleNameHash;

            for (var i = 0; i < chunk.Count; ++i) {
                var animComp = animations[i];
                if (animComp.nameHash != nameHash) {
                    animComp.nameHash = nameHash;
                    animations[i] = animComp;
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new AnimStateSystemJob() {
            animCompType = GetArchetypeChunkComponentType<SpriteAnimComponent>(),
            idleNameHash = _animNameHashes[(int)AnimationType.Idle],
            walkNameHash = _animNameHashes[(int)AnimationType.Walk],
            somethingDoItNameHash = _animNameHashes[(int)AnimationType.SomethingDoIt],
            nyonyoNameHash = _animNameHashes[(int)AnimationType.NyoNyo]
        };
        return job.Schedule(_group, inputDependencies);
    }
}
