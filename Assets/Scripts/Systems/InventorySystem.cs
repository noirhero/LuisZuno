// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

public class InventorySystem : ComponentSystem {
    protected override void OnUpdate() {
        Entities.ForEach((Entity entity, ref InventoryComponent inventoryComp) => {
            if (inventoryComp.bHasPendingItem == false)
                return;

            // TODO
        });
    }
}
