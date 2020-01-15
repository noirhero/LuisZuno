// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FadeInOutSystem : ComponentSystem {
    PostEffectPresetComponent fadeInOutPresetComp;
    FloatParameter valueParam;

    protected override void OnCreate() {
        Enabled = true;

        Entities.ForEach((Entity entity) => {
            if (EntityManager.HasComponent<PostEffectPresetComponent>(entity)) {
                fadeInOutPresetComp = World.Active.EntityManager.GetSharedComponentData<PostEffectPresetComponent>(entity);
            }
        });

        valueParam = new FloatParameter { value = 1.0f };
    }


    protected override void OnUpdate() {

        if (null == fadeInOutPresetComp.volume) {
            Entities.ForEach((Entity entity) => {
                if (EntityManager.HasComponent<PostEffectPresetComponent>(entity)) {
                    fadeInOutPresetComp = World.Active.EntityManager.GetSharedComponentData<PostEffectPresetComponent>(entity);
                }
            });
        }

        Entities.ForEach((Entity entity) => {
            if (EntityManager.HasComponent<FadeInComponent>(entity)) {
                var fadeInComp = EntityManager.GetComponentData<FadeInComponent>(entity);
                fadeInComp.elapsedTime += Time.DeltaTime;
                EntityManager.SetComponentData<FadeInComponent>(entity, fadeInComp);

                if (fadeInComp.time < fadeInComp.elapsedTime) {
                    valueParam.value = 0.0f;
                    EntityManager.RemoveComponent<FadeInComponent>(entity);
                }
                else {
                    float time = fadeInComp.elapsedTime / fadeInComp.time;
                    valueParam.value = Mathf.Lerp(1.0f, 0.0f, time);
                }
            }
            else if (EntityManager.HasComponent<FadeOutComponent>(entity)) {
                var fadeOutComp = EntityManager.GetComponentData<FadeOutComponent>(entity);
                fadeOutComp.elapsedTime += Time.DeltaTime;
                EntityManager.SetComponentData<FadeOutComponent>(entity, fadeOutComp);

                if (fadeOutComp.time < fadeOutComp.elapsedTime) {
                    valueParam.value = 1.0f;
                    EntityManager.RemoveComponent<FadeOutComponent>(entity);
                }
                else {
                    float time = fadeOutComp.elapsedTime / fadeOutComp.time;
                    valueParam.value = Mathf.Lerp(0.0f, 1.0f, time);
                }
            }

            if (fadeInOutPresetComp.volume && fadeInOutPresetComp.volume.profile.HasSettings<FadeInOutEffect>()) {
                var fadeInOutEffect = fadeInOutPresetComp.volume.profile.GetSetting<FadeInOutEffect>();
                fadeInOutEffect.value = valueParam;
                //Debug.Log("fadeInOutEffect.value : " + fadeInOutEffect.value.value);
            }
        });
    }
}
