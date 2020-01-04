// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class PropProxy : EntityProxy {
    [FormerlySerializedAs("Status")] public PropStatusComponent status;
    [FormerlySerializedAs("Reactive")] public ReactiveComponent reactive;

    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);
        
        dstManager.AddComponentData(entity, new PropStatusComponent(ref status));
        dstManager.AddComponentData(entity, new ReactiveComponent(ref reactive));
    }
}
