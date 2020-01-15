// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class TeleportProxy : EntityProxy, IDeclareReferencedPrefabs {
    [FormerlySerializedAs("TeleportSpot")] public GameObject teleportSpot;
    [FormerlySerializedAs("TeleportTime")] public float teleportTime;
    [FormerlySerializedAs("StartEffectPreset")] public GameObject startEffectPreset = null;
    [FormerlySerializedAs("EndEffectPreset")] public GameObject endEffectPreset = null;


    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(teleportSpot);
        referencedPrefabs.Add(startEffectPreset);
        referencedPrefabs.Add(endEffectPreset);
    }


    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        if (null == teleportSpot) {
            Debug.LogError("Set teleportSpot, now!!!!!!");
            return;
        }

        dstManager.AddComponentData(entity, new ReactiveComponent());

        var destPos = dstManager.GetComponentData<Translation>(conversionSystem.GetPrimaryEntity(teleportSpot));
        dstManager.AddComponentData(entity, new TeleportInfoComponent(destPos.Value, teleportTime) {
            startEffect = conversionSystem.GetPrimaryEntity(startEffectPreset),
            endEffect = conversionSystem.GetPrimaryEntity(endEffectPreset),
        });
    }
}
