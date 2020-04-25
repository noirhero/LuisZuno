// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using UnityEngine.Serialization;
using UnityEngine;

[RequiresEntityConversion]
public class MadnessEnvironmentProxy : MonoBehaviour, IConvertGameObjectToEntity {
    [FormerlySerializedAs("Environment")] public PassiveMadnessComponent passiveMadness;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.RemoveComponent<LocalToWorld>(entity);
        dstManager.RemoveComponent<Rotation>(entity);
        dstManager.RemoveComponent<Translation>(entity);

        dstManager.AddComponentData(entity, new PassiveMadnessComponent(passiveMadness));
    }
}
