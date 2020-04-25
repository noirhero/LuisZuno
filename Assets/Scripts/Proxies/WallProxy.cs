// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class WallProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new ReactiveComponent());
        dstManager.AddComponentData(entity, new TurningComponent());
    }
}
