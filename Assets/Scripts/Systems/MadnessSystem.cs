// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using GlobalDefine;

public class MadnessSystem : ComponentSystem {

    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        Entities.ForEach((Entity entity, ref AvatarStatusComponent statusComp) => {
            // 광기 카드를 받은 상태다!
            if (EntityManager.HasComponent<MadnessComponent>(entity)) {
                MadnessComponent madnessComp = EntityManager.GetComponentData<MadnessComponent>(entity);

                if (madnessComp.transitionStartMadness <= 0.0f) {
                    madnessComp.transitionStartMadness = statusComp.madness;
                }

                madnessComp.elapsedTransitionTime += deltaTime;

                float dest = madnessComp.transitionStartMadness + madnessComp.totalTransitionValue;
                statusComp.madness = Mathf.Lerp(madnessComp.transitionStartMadness, dest, madnessComp.elapsedTransitionTime /** madnessComp.transitionAccel*/);

                // finished
                if ((statusComp.madness >= dest) || (madnessComp.elapsedTransitionTime >= 10.0f)) {
                    statusComp.madness = Mathf.Clamp(dest, 0, statusComp.maxMadness);
                    madnessComp.totalTransitionValue = 0.0f;
                    madnessComp.transitionStartMadness = 0.0f;
                    madnessComp.transitionAccel = 0.0f;
                    madnessComp.elapsedTransitionTime = 0.0f;
                }

                EntityManager.SetComponentData<MadnessComponent>(entity, madnessComp);
            }
        });
    }
}
