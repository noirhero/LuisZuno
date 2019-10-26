// Copyrighgt 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using GlobalDefine;

public class GUISystem : ComponentSystem {
    private GUIPreset _guiPreset;
    private Entity _player = Entity.Null;

    protected override void OnStartRunning() {
        Entities.ForEach((GUIPresetComponent presetComp) => {
            _guiPreset = presetComp.preset;
        });

        Entities.ForEach((Entity entity, ref InventoryComponent inventoryComp, ref ReactiveComponent reactiveComp) => {
            if (GlobalDefine.EntityType.Player != reactiveComp.type)
                return;

            _player = entity;
        });
    }

    protected override void OnUpdate() {
        if (null == _guiPreset)
            return;

        if (_player.Equals(Entity.Null))
            return;

        // set gui - inventory
        InventoryComponent InventoryComp = EntityManager.GetComponentData<InventoryComponent>(_player);
        _guiPreset.ShowItem0(InventoryComp.item0);
        _guiPreset.ShowItem1(InventoryComp.item1);
        _guiPreset.ShowItem2(InventoryComp.item2);
    }
}
