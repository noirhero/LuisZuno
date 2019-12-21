// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class EntitySpawnProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    [FormerlySerializedAs("Prefab")] public GameObject prefab = null;
    [FormerlySerializedAs("SpawnEffect")] public GameObject spawnEffect = null;
    [FormerlySerializedAs("DestroyEffect")] public GameObject destroyEffect = null;
    [FormerlySerializedAs("SpawnNumber")] public int number;
    [FormerlySerializedAs("LifeTime")] public float lifetime;
    [FormerlySerializedAs("VelocityMin")] public float velocityMin;
    [FormerlySerializedAs("VelocityMax")] public float velocityMax;


    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(prefab);
        referencedPrefabs.Add(spawnEffect);
        referencedPrefabs.Add(destroyEffect);
    }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null == prefab) {
            Debug.LogError("Set prefab, now!!!!!!");
            return;
        }

        dstManager.AddComponentData(entity, new EntitySpawnComponent() {
            prefab = conversionSystem.GetPrimaryEntity(prefab),
            number = number,
            spawnPosition = transform.position,
            velocityMin = velocityMin,
            velocityMax = velocityMax,
        });


        dstManager.AddComponentData(entity, new LifeCycleComponent() {
            spawnEffect = conversionSystem.GetPrimaryEntity(spawnEffect),
            destroyEffect = conversionSystem.GetPrimaryEntity(destroyEffect),
            lifetime = lifetime,
            duration = -1.0f,
        });
    }
}
