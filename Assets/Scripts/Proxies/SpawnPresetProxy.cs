﻿// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;

public class SpawnPresetProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public SpritePreset preset = null;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null == preset) {
            Debug.LogError("Set preset, now!!!!!!");
            return;
        }
        dstManager.AddSharedComponentData(entity, new SpritePresetComponent(preset));

        dstManager.AddComponentData(entity, new SpriteAnimComponent() {
            nameHash = preset.datas.Keys.First()
        });
    }
}