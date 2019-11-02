// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[RequiresEntityConversion]
public class NoneAvatarProxy : EntityProxy {
    public Int64 dropItem;
    public NoneAvatarStatusComponent Status;
    public override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);
        
        dstManager.AddComponentData(entity, new NoneAvatarStatusComponent(ref Status));
        if (Utility.IsVaild(dropItem)) {
            dstManager.AddComponentData(entity, new DropComponent() {
                dropItemID = dropItem,
            });
        }
    }
}
