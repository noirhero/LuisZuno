// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(MovementSystem))]
public class AutoMovementSystem : JobComponentSystem {
    private EntityQuery _group;

    protected override void OnCreate() {
        var query = new EntityQueryDesc() {
            All = new ComponentType[] {
                typeof(MovementComponent),
                typeof(Translation)
            },
            Options = EntityQueryOptions.FilterWriteGroup
        };
        _group = GetEntityQuery(query);
    }

    [BurstCompile]
    struct AutoMovementSystemJob : IJobChunk {
        public float deltaTime;
        public ArchetypeChunkComponentType<MovementComponent> movementType;
        public ArchetypeChunkComponentType<Translation> translationType;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex) {
            var movements = chunk.GetNativeArray(movementType);
            var translations = chunk.GetNativeArray(translationType);

            for (var i = 0; i < chunk.Count; ++i) {
                var moveComp = movements[i];
                var currentPos = translations[i].Value;

                if (moveComp.xValue == 0.0f) {
                    moveComp.xValue = -1.0f;
                }

                for (var j = 0; j < chunk.Count; ++j) {
                    if (i == j) {
                        continue;
                    }

                    var at = translations[j].Value.x - currentPos.x;
                    bool isHeadingForward = (moveComp.xValue < 0.0f && at < 0.0f) || (moveComp.xValue > 0.0f && at > 0.0f);
                    if (!isHeadingForward)
                        continue;

                    if (0.5f >= math.abs(at)) {
                        moveComp.value = 0.0f;
                        movements[i] = moveComp;
                        break;
                    }

                    if (moveComp.xValue < 0.0f) {
                        moveComp.value.x = -1.0f;
                    }
                    else {
                        moveComp.value.x = 1.0f;
                    }

                    currentPos.x += moveComp.value.x * deltaTime;

                    movements[i] = moveComp;
                    translations[i] = new Translation() {
                        Value = currentPos
                    };
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new AutoMovementSystemJob() {
            deltaTime = Time.deltaTime,
            movementType = GetArchetypeChunkComponentType<MovementComponent>(),
            translationType = GetArchetypeChunkComponentType<Translation>()
        };
        return job.Schedule(_group, inputDependencies);
    }
}
