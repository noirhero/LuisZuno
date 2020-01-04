// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class MadnessEnvironmentProxy : EntityProxy {
    [FormerlySerializedAs("Environment")] public PassiveMadnessComponent passiveMadness;

    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new PassiveMadnessComponent(passiveMadness));
    }
}
