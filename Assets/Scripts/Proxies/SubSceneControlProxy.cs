// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class SubSceneControlProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public SubScenePreset preset;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (ReferenceEquals(preset, null)) {
            Debug.LogError("Set preset, now!!!!!");
            dstManager.DestroyEntity(entity);
            return;
        }

        dstManager.RemoveComponent<Translation>(entity);
        dstManager.RemoveComponent<Rotation>(entity);
        dstManager.RemoveComponent<LocalToWorld>(entity);

        dstManager.AddSharedComponentData(entity, new SubScenePresetComponent() {
            preset = preset
        });

        dstManager.AddComponentData(entity, new SubSceneLoadComponent() {
            type = (int) SubSceneType.sceneSelect
        });
    }
}