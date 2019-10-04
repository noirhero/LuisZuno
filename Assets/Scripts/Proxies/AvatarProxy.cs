// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;

[RequiresEntityConversion]
public class AvatarProxy : EntityProxy {

    public override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new MovementComponent());
    }
}
