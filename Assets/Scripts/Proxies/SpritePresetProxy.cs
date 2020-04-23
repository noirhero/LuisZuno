// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class SpritePresetProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public int guid;


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var spritePreset = GetComponent<SpritePreset>();
        if (ReferenceEquals(null, spritePreset)) {
            Debug.LogError("Add 'SpritePreset' now!!!!!");
            dstManager.DestroyEntity(entity);
            return;
        }

        dstManager.RemoveComponent<LocalToWorld>(entity);
        dstManager.RemoveComponent<Translation>(entity);
        dstManager.RemoveComponent<Rotation>(entity);

        dstManager.AddSharedComponentData(entity, new SpritePresetGuidComponent() {
            guid = guid,
            preset = spritePreset
        });
    }
}
