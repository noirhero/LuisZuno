// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class TableProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public TablePreset preset = null;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null != preset) {
            dstManager.AddSharedComponentData(entity, new TablePresetComponent(preset));
        }
    }
}
