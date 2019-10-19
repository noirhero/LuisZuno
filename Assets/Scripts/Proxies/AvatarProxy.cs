// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[RequiresEntityConversion]
public class AvatarProxy : EntityProxy {
    public Int32 sane;
    public Int32 curiosity;

    public override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new MovementComponent(-1.0f));

        dstManager.AddComponentData(entity, new AvatarStatusComponent() {
            sane = sane,
            curiosity = curiosity,
            bInPanic = false
        });
    }
}
