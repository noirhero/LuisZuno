// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class OldClosetProxy : PropProxy {
    [FormerlySerializedAs("SpawnInfo")] public EntitySpawnInfoComponent spawnInfo;
    [FormerlySerializedAs("SpawnPreset")] public GameObject spawnPreset = null;
    [FormerlySerializedAs("SpawnEffectPreset")] public GameObject spawnEffectPreset = null;
    [FormerlySerializedAs("DestroyEffectPreset")] public GameObject destroyEffectPreset = null;


    protected override void SetupPrefabs(List<GameObject> referencedPrefabs) {
        base.SetupPrefabs(referencedPrefabs);

        referencedPrefabs.Add(spawnPreset);
        referencedPrefabs.Add(spawnEffectPreset);
        referencedPrefabs.Add(destroyEffectPreset);
    }


    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new EntitySpawnInfoComponent(ref spawnInfo) {
            prefab = conversionSystem.GetPrimaryEntity(spawnPreset),
            spawnEffect = conversionSystem.GetPrimaryEntity(spawnEffectPreset),
            destroyEffect = conversionSystem.GetPrimaryEntity(destroyEffectPreset),
        });
    }
}