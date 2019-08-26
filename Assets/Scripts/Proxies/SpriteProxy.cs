// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class SpriteProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public SpritePreset preset = null;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null == preset) {
            Debug.LogError("Set preset, now!!!!!!");
            return;
        }
        
        dstManager.AddSharedComponentData(entity, new SpritePresetComponent() {
            preset = preset
        });

        dstManager.AddComponentData(entity, new SpriteAnimComponent() {
            nameHash = preset.datas.Keys.First()
        });

        dstManager.AddComponentData(entity, new MovementComponent());
    }
}