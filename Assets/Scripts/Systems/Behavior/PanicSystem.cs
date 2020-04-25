// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

[UpdateAfter(typeof(GameSystem))]
public class PanicSystem : ComponentSystem {
    protected void GetMadness(Entity playerEntity, ref PanicComponent panicComp) {
        bool shouldGetMadness = (panicComp.madness > 0);
        if (shouldGetMadness) {
            // update
            if (EntityManager.HasComponent<MadnessComponent>(playerEntity)) {
                var madnessComp = EntityManager.GetComponentData<MadnessComponent>(playerEntity);
                var statusComp = EntityManager.GetComponentData<PlayerStatusComponent>(playerEntity);

                // 남은 매드니스 저장
                if (madnessComp.value > 0.0f) {
                    madnessComp.valueForInterrupted = (madnessComp.transitionStartValue + madnessComp.value) - statusComp.status.madness;
                }

                madnessComp.value = panicComp.madness;
                madnessComp.duration = panicComp.madnessDuration;

                EntityManager.SetComponentData<MadnessComponent>(playerEntity, madnessComp);
            }
            // create
            else {
                EntityManager.AddComponentData(playerEntity, new MadnessComponent() {
                    value = panicComp.madness,
                    duration = panicComp.madnessDuration
                });
            }
        }
    }

    protected override void OnUpdate() {
        Entities.ForEach((Entity playerEntity, ref PanicComponent panicComp, ref PlayerComponent playerComp) => {
            if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
                return;
            }

            // initialize animation
            if (panicComp.elapsedPanicTime == 0.0f) {
                playerComp.currentAnim = panicComp.panicAnim;

                GetMadness(playerEntity, ref panicComp);
            }

            panicComp.elapsedPanicTime += Time.DeltaTime;
            if (panicComp.elapsedPanicTime >= panicComp.panicTime) {
                EntityManager.RemoveComponent<PanicComponent>(playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.panic;
            }
        });
    }
}
