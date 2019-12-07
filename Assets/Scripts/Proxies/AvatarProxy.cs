// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class AvatarProxy : EntityProxy {
    public ItemStruct[] defaultInventory = new ItemStruct[0];
    [FormerlySerializedAs("Status")] public AvatarStatusComponent status;

    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);
        
        dstManager.AddComponentData(entity, new InventoryComponent(defaultInventory));
        dstManager.AddComponentData(entity, new AvatarStatusComponent(ref status));
    }
}
