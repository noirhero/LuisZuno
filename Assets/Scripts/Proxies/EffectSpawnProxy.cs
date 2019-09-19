// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class EffectSpawnProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public GameObject prefab = null;
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(prefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null == prefab) {
            Debug.LogError("Set prefab, now!!!!!!");
            return;
        }
        
        dstManager.AddComponentData(entity, new EffectSpawnComponent() {
            preset = conversionSystem.GetPrimaryEntity(prefab)
        });
    }
}