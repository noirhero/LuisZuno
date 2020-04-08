// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

[RequiresEntityConversion]
public class ScenarioInformationProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public ScenarioInfomationPreset preset;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPresets) {
        referencedPresets.Add(preset.gameObject);
    }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddSharedComponentData(entity, new ScenarioInformationPresetComponent(preset));
    }
}
