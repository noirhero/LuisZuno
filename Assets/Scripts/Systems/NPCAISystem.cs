// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

public class NPCAISystem : ComponentSystem {

    protected override void OnStartRunning() {

    }

    protected override void OnUpdate() {
        Entities.ForEach((Entity entity, NPCAIPresetComponent aiPresetComp, ref NPCComponent npcComp) => {
            var preset = aiPresetComp.preset;
            var currentData = preset.AIDatas[npcComp.AICurrentIndex];
            npcComp.AIElapsedTIme += Time.DeltaTime;

            if (currentData.time < npcComp.AIElapsedTIme) {
                npcComp.AIElapsedTIme = 0.0f;
                if (++npcComp.AICurrentIndex >= preset.AIDatas.Count) {
                    npcComp.AICurrentIndex = 0;
                }

                currentData = preset.AIDatas[npcComp.AICurrentIndex];    // updated data
                npcComp.currentAnim = currentData.animation;
            }
        });
    }
}
