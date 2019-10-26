// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[RequiresEntityConversion]
public class NoneAvatarProxy : EntityProxy {
    public NoneAvatarStatusComponent Status;
	public ItemStruct dropItem;

    public override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new NoneAvatarStatusComponent(ref Status));
		dstManager.AddComponentData(entity, new ItemComponet(dropItem));
    }
}
