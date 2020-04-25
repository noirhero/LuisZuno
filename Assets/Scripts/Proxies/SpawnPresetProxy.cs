// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnPresetProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var sprite = GetComponent<SpriteRenderer>()?.sprite;
        if (false == ReferenceEquals(null, sprite)) {
            var localScale = GetComponent<Transform>().localScale;
            var spriteScale = new float3(sprite.rect.width, sprite.rect.height, 1.0f) / sprite.pixelsPerUnit;
            spriteScale *= localScale;
            spriteScale.z = 1.0f;
            dstManager.AddComponentData(entity, new NonUniformScale() {
                Value = spriteScale
            });

            var applyPivot = (sprite.rect.center - sprite.pivot) / sprite.pixelsPerUnit * localScale;
            dstManager.AddComponentData(entity, new SpritePivotComponent() {
                Value = new float3(applyPivot, 0.0f)
            });
        }
    }
}
