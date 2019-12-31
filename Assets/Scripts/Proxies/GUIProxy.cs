// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class GUIProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public GUIPreset preset = null;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null != preset) {
            dstManager.AddSharedComponentData(entity, new GUIPresetComponent(preset));
        }
    }
}
