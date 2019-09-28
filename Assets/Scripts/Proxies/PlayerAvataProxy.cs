// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;

public class PlayerAvataProxy : AvatarProxy {
    public override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new PlayerComponent());
    }
}
