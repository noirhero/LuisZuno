// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using GlobalDefine;
using System.Collections.Generic;

[RequiresEntityConversion]
public class EntityProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public string presetPath;

    protected GameObject preset = null;


    public void DeclareReferencedPrefabs(List<GameObject> referencedPresets) {
        LoadAssets();
        SetupPrefabs(referencedPresets);
    }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        SetupComponents(entity, dstManager, conversionSystem);
    }
    

    protected virtual void LoadAssets() {
        preset = Utility.LoadObjectAtPath<GameObject>(presetPath);
    }


    protected virtual void SetupPrefabs(List<GameObject> referencedPresets) {
        if (null != preset) {
            referencedPresets.Add(preset);
        }
    }


    protected virtual void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var sprite = spriteRenderer ? spriteRenderer.sprite : null;
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

        var spritePreset = preset ? preset.GetComponent<SpritePreset>() : null;
        if (false == ReferenceEquals(null, spritePreset)) {
            dstManager.AddSharedComponentData(entity, new SpritePresetComponent() {
                preset = spritePreset
            });
            dstManager.AddComponentData(entity, new SpriteStateComponent() {
                hash = spritePreset.datas.Keys.First()
            });
        }
    }
}
