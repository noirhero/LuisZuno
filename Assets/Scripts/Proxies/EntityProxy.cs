// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[RequiresEntityConversion]
public class EntityProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public NewSpritePreset preset = null;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        SetupComponents(entity, dstManager, conversionSystem);
    }

    protected virtual void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (ReferenceEquals(null, preset)) {
            return;
        }

        var sprite = GetComponent<SpriteRenderer>()?.sprite;
        if (ReferenceEquals(null, sprite)) {
            Debug.LogError("Add 'SpriteRenderer' component, now!!!!!");
            return;
        }
        var spriteScale = new float3(sprite.rect.width, sprite.rect.height, 1.0f) / sprite.pixelsPerUnit;
        spriteScale *= GetComponent<Transform>().localScale;
        spriteScale.z = 1.0f;
        dstManager.AddComponentData(entity, new NonUniformScale() {
            Value = spriteScale
        });

        dstManager.AddComponentData(entity, new SpritePivotComponent() {
            Value = new float3((sprite.rect.center - sprite.pivot) / sprite.pixelsPerUnit, 0.0f)
        }); 

        dstManager.AddSharedComponentData(entity, new NewSpritePresetComponent() {
            preset = preset
        });
        dstManager.AddComponentData(entity, new SpriteStateComponent() {
            hash = preset.datas.Keys.First()
        });
    }
}
