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
                typeof(SpriteAnimComponent),
                typeof(MovementComponent),
                typeof(ReactiveComponent),
                typeof(AvatarStatusComponent),
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
        public ArchetypeChunkComponentType<MovementComponent> moveCompType;
        public ArchetypeChunkComponentType<ReactiveComponent> reactiveCompType;
        public ArchetypeChunkComponentType<AvatarStatusComponent> avatarStatusCompType;
        public int idleNameHash;
        public int walkNameHash;
        public int somethingDoItNameHash;
        public int nyonyoNameHash;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex) {
            var animations = chunk.GetNativeArray(animCompType);
            var movements = chunk.GetNativeArray(moveCompType);
            var reactives = chunk.GetNativeArray(reactiveCompType);
            var statuses = chunk.GetNativeArray(avatarStatusCompType);
            var nameHash = idleNameHash;

            for (var i = 0; i < chunk.Count; ++i) {
                var bMoving = math.FLT_MIN_NORMAL < math.lengthsq(movements[i].value);
                if (bMoving) {
                    nameHash = walkNameHash;
                }
                else {
                    if (statuses[i].InPanic) {
                        nameHash = nyonyoNameHash;
                    }
                    else if (reactives[i].ReactionElapsedTime > 0) {
                        nameHash = somethingDoItNameHash;
                    }
                    else {
                        nameHash = idleNameHash;
                    }
                }

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
            moveCompType = GetArchetypeChunkComponentType<MovementComponent>(),
            reactiveCompType = GetArchetypeChunkComponentType<ReactiveComponent>(),
            avatarStatusCompType = GetArchetypeChunkComponentType<AvatarStatusComponent>(),
            idleNameHash = _animNameHashes[(int)AnimationType.Idle],
            walkNameHash = _animNameHashes[(int)AnimationType.Walk],
            somethingDoItNameHash = _animNameHashes[(int)AnimationType.SomethingDoIt],
            nyonyoNameHash = _animNameHashes[(int)AnimationType.NyoNyo]
        };
        return job.Schedule(_group, inputDependencies);
    }
}
