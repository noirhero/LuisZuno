// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using GlobalDefine;

public class MadnessSystem : ComponentSystem {

    protected override void OnCreate() {
        Enabled = false;
    }

    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        // 패시브
        Entities.ForEach((Entity entity, ref PassiveMadnessComponent passiveMadnessComp) => {
            // Get Player entity
            Entity playerEntity = Entity.Null;
            Entities.WithAll<PlayerComponent>().ForEach((Entity e) => {
                playerEntity = e;
            });

            var statusComp = EntityManager.GetComponentData<PlayerStatusComponent>(playerEntity);

            passiveMadnessComp.ElapsedTickTime += deltaTime;

            var finished = false;
            if (passiveMadnessComp.ElapsedDuration >= passiveMadnessComp.duration) {    // 광기에 영향을 주는 총 시간이 지남
                finished = true;
            }

            if (passiveMadnessComp.ElapsedTickTime >= passiveMadnessComp.tickTime) {    // 광기 받을 시간이 왔어요
                passiveMadnessComp.ElapsedDuration += passiveMadnessComp.tickTime;
                passiveMadnessComp.ElapsedTickTime = 0.0f;

                statusComp.madness += passiveMadnessComp.value;

                if (statusComp.madness >= statusComp.maxMadness) {
                    statusComp.madness = statusComp.maxMadness;
                    finished = true;
                }
            }

            EntityManager.SetComponentData<PlayerStatusComponent>(playerEntity, statusComp);

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
                    statusComp.madness += madnessComp.valueForInterrupted;
                    madnessComp.valueForInterrupted = 0.0f;
                    madnessComp.elapsedTransitionTime = 0.0f;
                    madnessComp.transitionStartValue = statusComp.madness;
                }

                // initialize
                if (madnessComp.transitionStartValue <= 0.0f) {
                    madnessComp.transitionStartValue = statusComp.madness;
                }

                madnessComp.elapsedTransitionTime += deltaTime;

                float dest = madnessComp.transitionStartValue + madnessComp.value;
                float speed = madnessComp.elapsedTransitionTime / madnessComp.duration;

                statusComp.madness = Mathf.Lerp(madnessComp.transitionStartValue, dest, speed);

                EntityManager.SetComponentData<MadnessComponent>(playerEntity, madnessComp);

                // finished
                if ((statusComp.madness >= dest) || (madnessComp.elapsedTransitionTime >= madnessComp.duration)) {
                    statusComp.madness = dest;
                    EntityManager.RemoveComponent<MadnessComponent>(playerEntity);
                }
            }

        });
    }
}
