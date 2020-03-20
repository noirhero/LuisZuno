// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using System.Collections.Generic;

[RequiresEntityConversion]
public class EntityProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public GameObject preset = null;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPresets) {
        SetupPrefabs(referencedPresets);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        SetupComponents(entity, dstManager, conversionSystem);
    }

    protected virtual void SetupPrefabs(List<GameObject> referencedPresets) {
        referencedPresets.Add(preset);
    }

    protected virtual void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null == preset)
            return;

        var spritePreset = preset.GetComponent<SpritePreset>();
        if (ReferenceEquals(null, spritePreset)) {
            return;
        }

        var sprite = GetComponent<SpriteRenderer>()?.sprite;
        if (ReferenceEquals(null, sprite)) {
            Debug.LogError("Add 'SpriteRenderer' component, now!!!!!");
            return;
        }

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

        dstManager.AddSharedComponentData(entity, new SpritePresetComponent() {
            preset = spritePreset
        });
        dstManager.AddComponentData(entity, new SpriteStateComponent() {
            hash = spritePreset.datas.Keys.First()
        });
    }
}
