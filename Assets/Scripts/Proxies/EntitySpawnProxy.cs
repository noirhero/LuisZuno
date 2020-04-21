// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class EntitySpawnProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public EntitySpawnInfoComponent entitySpawn;
    public GameObject spawnPreset = null;
    public GameObject spawnEffectPreset = null;
    public GameObject destroyEffectPreset = null;


    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(spawnPreset);
        referencedPrefabs.Add(spawnEffectPreset);
        referencedPrefabs.Add(destroyEffectPreset);
    }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new EntitySpawnInfoComponent(ref entitySpawn) {
            preset = conversionSystem.GetPrimaryEntity(spawnPreset),
            spawnEffect = conversionSystem.GetPrimaryEntity(spawnEffectPreset),
            destroyEffect = conversionSystem.GetPrimaryEntity(destroyEffectPreset),
        });
    }
}
