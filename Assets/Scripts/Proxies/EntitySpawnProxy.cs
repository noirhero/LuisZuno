// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class EntitySpawnProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    [FormerlySerializedAs("Prefab")] public GameObject prefab = null;
    [FormerlySerializedAs("SpawnNumber")] public int count;
    [FormerlySerializedAs("SpawnDirection")] public float3 direction;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(prefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null == prefab) {
            Debug.LogError("Set prefab, now!!!!!!");
            return;
        }

        dstManager.AddComponentData(entity, new EntitySpawnComponent() {
            prefab = conversionSystem.GetPrimaryEntity(prefab),
            spawnNumber = count,
            spawnPosition = transform.position,
            spawnDirection = direction
        });
    }
}
