// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class TeleportProxy : EntityProxy {
    [FormerlySerializedAs("TeleportSpot")] public GameObject teleportSpot;
    [FormerlySerializedAs("TeleportTime")] public float teleportTime;
    [FormerlySerializedAs("FadeInOutTime")] public float fadeInOutTime;
    [FormerlySerializedAs("Status")] public PropStatusComponent status;


    protected override void SetupPrefabs(List<GameObject> referencedPrefabs) {
        base.SetupPrefabs(referencedPrefabs);

        if (null != teleportSpot) {
            referencedPrefabs.Add(teleportSpot);
        }
    }


    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        if (null == teleportSpot) {
            Debug.LogError("Set teleportSpot, now!!!!!!");
            return;
        }

        dstManager.AddComponentData(entity, new ReactiveComponent());
        dstManager.AddComponentData(entity, new PropStatusComponent(ref status));

        var destPos = dstManager.GetComponentData<Translation>(conversionSystem.GetPrimaryEntity(teleportSpot));
        dstManager.AddComponentData(entity, new TeleportInfoComponent(destPos.Value, teleportTime, fadeInOutTime) {
        });
    }
}
