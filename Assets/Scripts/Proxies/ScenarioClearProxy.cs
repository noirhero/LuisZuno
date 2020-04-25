// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class ScenarioClearProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public TeleportInfoComponent teleportInfo;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new ReactiveComponent());
        dstManager.AddComponentData(entity, new ScenarioClearComponent() { teleportInfoComp = teleportInfo });
    }
}
