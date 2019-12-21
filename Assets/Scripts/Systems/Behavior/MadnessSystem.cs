// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using GlobalDefine;

public class MadnessSystem : ComponentSystem {

    protected override void OnCreate() {
        Enabled = false;
    }

    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        Entities.ForEach((Entity entity, ref AvatarStatusComponent statusComp, ref MadnessComponent madnessComp, ref PlayerComponent playerComp) => {
            if (madnessComp.valueForInterrupted > 0.0f) {
                statusComp.madness = madnessComp.valueForInterrupted;
                madnessComp.valueForInterrupted = 0.0f;
            }

            // 이상한 걸 보고 직접적으로 광기에 영향받음
            if (madnessComp.value > 0.0f) {
                // 직접 영향은 서칭 중에 받지 않음
                if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
                    return;
                }

                if (madnessComp.transitionStartValue <= 0.0f) {
                    madnessComp.transitionStartValue = statusComp.madness;
                }

                madnessComp.elapsedTransitionTime += deltaTime;

                float dest = madnessComp.transitionStartValue + madnessComp.value;
                float speed = madnessComp.elapsedTransitionTime / madnessComp.duration;
                statusComp.madness = Mathf.Lerp(madnessComp.transitionStartValue, dest, speed);

                // finished
                if ((statusComp.madness >= dest) || (madnessComp.elapsedTransitionTime >= madnessComp.duration)) {
                    statusComp.madness = dest;
                    madnessComp.value = 0.0f;
                    madnessComp.duration = 0.0f;
                    madnessComp.transitionStartValue = 0.0f;
                    madnessComp.elapsedTransitionTime = 0.0f;

                    if (madnessComp.linearValue == 0.0f) {
                        EntityManager.RemoveComponent<MadnessComponent>(entity);
                    }
                }
            }

            // 이상한 환경에서 간접적으로 광기에 영향받음
            if (madnessComp.linearValue > 0.0f) {
                madnessComp.elapsedLinearTransitionTime += deltaTime;

                // finished
                if (madnessComp.elapsedLinearTransitionTime >= madnessComp.linearDuration) {
                    madnessComp.linearValue = 0.0f;
                    madnessComp.linearTickTime = 0.0f;
                    madnessComp.linearDuration = 0.0f;
                    madnessComp.elapsedLinearTransitionTime = 0.0f;

                    if (madnessComp.value == 0.0f) {
                        EntityManager.RemoveComponent<MadnessComponent>(entity);
                    }
                }

                if (madnessComp.elapsedLinearTransitionTime >= madnessComp.linearTickTime) {
                    madnessComp.linearDuration -= madnessComp.elapsedLinearTransitionTime;
                    madnessComp.elapsedLinearTransitionTime = 0.0f;
                    
                    statusComp.madness += madnessComp.linearValue;
                }
            }

            statusComp.madness = Mathf.Clamp(statusComp.madness, 0, statusComp.maxMadness);

            // update player's behaviour
            if (madnessComp.value == 0.0f || madnessComp.linearValue == 0.0f) {
                playerComp.currentBehaviors ^= BehaviorState.madness;
            }
        });
    }
}
