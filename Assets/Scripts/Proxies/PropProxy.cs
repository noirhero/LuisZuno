// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class PropProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public PropStatusComponent status;
    public ReactiveComponent reactive;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new PropStatusComponent(ref status));
        dstManager.AddComponentData(entity, new ReactiveComponent(ref reactive));
    }
}
