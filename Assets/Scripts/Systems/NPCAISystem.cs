// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

public class NPCAISystem : ComponentSystem {

    protected override void OnStartRunning() {

    }

    protected override void OnUpdate() {
        Entities.ForEach((Entity entity, NPCAIPresetComponent AIPresetComp, ref NPCComponent NPCComp) => {
            var preset = AIPresetComp.preset;
            var currentData = preset.AIDatas[AIPresetComp.CurrentIndex];
            currentData.ElapsedTime += Time.DeltaTime;

            if (currentData.ElapsedTime >= currentData.time) {
                currentData.ElapsedTime = 0.0f;
                if (preset.AIDatas.Count <= ++AIPresetComp.CurrentIndex) {
                    AIPresetComp.CurrentIndex = 0;
                }

                currentData = preset.AIDatas[AIPresetComp.CurrentIndex];
                NPCComp.currentAnim = currentData.animation;
            }
        });
    }
}
