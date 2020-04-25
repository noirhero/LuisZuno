// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PresetProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public int guid;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.RemoveComponent<LocalToWorld>(entity);
        dstManager.RemoveComponent<Translation>(entity);
        dstManager.RemoveComponent<Rotation>(entity);

        dstManager.AddSharedComponentData(entity, new PresetComponent() {
            guid = guid,
            preset = GetComponent<SpritePreset>(),
            npcAIPreset = GetComponent<NPCAIPreset>()
        });
    }
}
