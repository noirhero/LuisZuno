// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using GlobalDefine;

[RequiresEntityConversion]
public class EntityProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public SpritePreset preset = null;
    public EntityType entityType = EntityType.None;
    public float entityReactionTime = 3.0f;
    public int entityReactionLimitCount = 3;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        SetupComponents(entity, dstManager, conversionSystem);
    }

    protected virtual void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null != preset) {
            dstManager.AddSharedComponentData(entity, new SpritePresetComponent(preset));
            dstManager.AddComponentData(entity, new SpriteAnimComponent() {
                nameHash = preset.datas.Keys.First()
            });
        }

        dstManager.AddComponentData(entity, new ReactiveComponent() {
            type = entityType,
            reactionTime = entityReactionTime,
            reactionLimitCount = entityReactionLimitCount
        });

        dstManager.AddComponentData(entity, new TargetComponent() {
            lastTargetIndex = int.MinValue,
            targetIndex = int.MaxValue,
            targetDistance = float.PositiveInfinity,
        });
    }
}
