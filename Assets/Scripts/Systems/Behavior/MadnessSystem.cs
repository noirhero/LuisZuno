// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using GlobalDefine;
using UnityEngine;
using Unity.Entities;

[UpdateAfter(typeof(GameSystem))]
public class MadnessSystem : ComponentSystem {
    protected override void OnUpdate() {
        var deltaTime = Time.DeltaTime;

        // 패시브
        Entities.ForEach((Entity entity, ref PassiveMadnessComponent passiveMadnessComp) => {
            var statusComp = EntityManager.GetComponentData<PlayerStatusComponent>(Utility.playerEntity);

            passiveMadnessComp.ElapsedTickTime += deltaTime;

            var finished = passiveMadnessComp.ElapsedDuration >= passiveMadnessComp.duration;
            var calculatedTickTime = passiveMadnessComp.tickTime / statusComp.madnessWeight;

            if (passiveMadnessComp.ElapsedTickTime >= calculatedTickTime) {    // 광기 받을 시간이 왔어요
                passiveMadnessComp.ElapsedDuration += calculatedTickTime;
                passiveMadnessComp.ElapsedTickTime = 0.0f;

                statusComp.status.madness += passiveMadnessComp.value;

                if (statusComp.status.madness >= statusComp.maxMadness) {
                    statusComp.status.madness = statusComp.maxMadness;
                    finished = true;
                }
            }

            EntityManager.SetComponentData<PlayerStatusComponent>(Utility.playerEntity, statusComp);

            if (finished) {
                EntityManager.RemoveComponent<PassiveMadnessComponent>(entity);

                // TODO : GameOver
            }
        });

        Entities.ForEach((Entity playerEntity, ref PlayerStatusComponent statusComp, ref PlayerComponent playerComp) => {
            // 액티브
            if (EntityManager.HasComponent<MadnessComponent>(playerEntity)) {
                var madnessComp = EntityManager.GetComponentData<MadnessComponent>(playerEntity);

                // 이미 올라갈 매드니스가 있는데 또 들어왔을 경우 이전값 바로 적용
                if (madnessComp.valueForInterrupted > 0.0f) {
                    statusComp.status.madness += madnessComp.valueForInterrupted;
                    madnessComp.valueForInterrupted = 0.0f;
                    madnessComp.elapsedTransitionTime = 0.0f;
                    madnessComp.transitionStartValue = statusComp.status.madness;
                }

                // initialize
                if (madnessComp.transitionStartValue <= 0.0f) {
                    madnessComp.transitionStartValue = statusComp.status.madness;
                }

                madnessComp.elapsedTransitionTime += deltaTime;

                float dest = madnessComp.transitionStartValue + (madnessComp.value * statusComp.madnessWeight);
                float speed = madnessComp.elapsedTransitionTime / madnessComp.duration;

                statusComp.status.madness = Mathf.Lerp(madnessComp.transitionStartValue, dest, speed);

                EntityManager.SetComponentData<MadnessComponent>(playerEntity, madnessComp);

                // finished
                if ((statusComp.status.madness >= dest) || (madnessComp.elapsedTransitionTime >= madnessComp.duration)) {
                    statusComp.status.madness = dest;
                    EntityManager.RemoveComponent<MadnessComponent>(playerEntity);
                }
            }

        });

        // 패시브가 아니더라도 체크되도록 위로 빼놓음(임시)
        var tempStatusComp = EntityManager.GetComponentData<PlayerStatusComponent>(Utility.playerEntity);
        if (tempStatusComp.status.madness >= tempStatusComp.maxMadness) {
            EntityManager.AddComponentData<GameOverComponent>(Utility.playerEntity, new GameOverComponent());
        }
    }
}
