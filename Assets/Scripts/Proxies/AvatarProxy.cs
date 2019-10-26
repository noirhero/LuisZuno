// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

[RequiresEntityConversion]
public class AvatarProxy : EntityProxy {
    public ItemStruct[] defaultInventory = new ItemStruct[0];

    public override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new MovementComponent(-1.0f));

        dstManager.AddComponentData(entity, new InventoryComponent(defaultInventory));
    }
}
