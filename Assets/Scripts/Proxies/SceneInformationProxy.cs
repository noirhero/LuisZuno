// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

[RequiresEntityConversion]
public class SceneInformationProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public SceneInfomationPreset scenePreset;


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddSharedComponentData(entity, new SceneInformationPresetComponent() {
            preset = scenePreset
        });
    }
}
