// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;

[RequiresEntityConversion]
public class ScenarioClearProxy : EntityProxy {

    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new ReactiveComponent());
        dstManager.AddComponentData(entity, new ScenarioClearComponent());
    }
}