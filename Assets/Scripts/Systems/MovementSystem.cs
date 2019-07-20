// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MovementSystem : JobComponentSystem {
    struct MovementSystemJob : IJobForEach<MovementComponent, Translation> {
        public float deltaTime;

        public void Execute(ref MovementComponent moveComp, ref Translation pos) {
            var currentKeyboard = Keyboard.current;

            moveComp.value = float3.zero;
            if (true == currentKeyboard.wKey.isPressed) {
                moveComp.value.y += 1.0f;
            }
            if (true == currentKeyboard.sKey.isPressed) {
                moveComp.value.y -= 1.0f;
            }
            if (true == currentKeyboard.dKey.isPressed) {
                moveComp.value.x += 1.0f;
                moveComp.xValue = 1.0f;
            }
            if (true == currentKeyboard.aKey.isPressed) {
                moveComp.value.x -= 1.0f;
                moveComp.xValue = -1.0f;
            }

            float lengthSq = math.lengthsq(moveComp.value);
            if (math.FLT_MIN_NORMAL >= lengthSq) {
                return;
            }

            float length = math.sqrt(lengthSq);
            moveComp.value.x /= length;
            moveComp.value.y /= length;

            pos.Value += moveComp.value * deltaTime;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new MovementSystemJob();
        job.deltaTime = Time.deltaTime;

        return job.Schedule(this, inputDependencies);
    }
}