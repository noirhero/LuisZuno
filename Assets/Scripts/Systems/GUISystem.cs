// Copyrighgt 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;
using GlobalDefine;

public class GUISystem : ComponentSystem {
    private TablePreset _tablePreset;
    private GUIPreset _guiPreset;
    private Entity _playerEntity = Entity.Null;
    

    private void UpdateInventoryUI() {
        if (EntityManager.HasComponent<InventoryComponent>(_playerEntity)) {
            InventoryComponent inventoryComp = EntityManager.GetComponentData<InventoryComponent>(_playerEntity);
            SetItemSprite(inventoryComp.item1.id, _guiPreset.item1);
            SetItemSprite(inventoryComp.item2.id, _guiPreset.item2);
            SetItemSprite(inventoryComp.item3.id, _guiPreset.item3);
        }
    }


    private void SetItemSprite(Int64 inID, Image inImg) {
        ItemPresetData data;
        if (_tablePreset.itemDatas.TryGetValue(inID, out data)) {
            inImg.gameObject.SetActive(Utility.IsValid(inID));
            inImg.sprite = data.sprite;
        }
    }


    private void UpdateBubbleUI() {
        _guiPreset.HideBubble();

        if (false == EntityManager.HasComponent<ReactiveComponent>(_playerEntity)) {
            return;
        }

        SearchingComponent searchingComp = EntityManager.GetComponentData<SearchingComponent>(_playerEntity);
        if (searchingComp.elapsedSearchingTime <= 0) {
            return;
        }

        // bubble default
        string bubbleMassage = "... ";
        float timeRate = searchingComp.elapsedSearchingTime / searchingComp.searchingTime;

        // bubble position
        Translation playerPos = EntityManager.GetComponentData<Translation>(_playerEntity);
        Vector3 convert2DPos = Camera.main.WorldToScreenPoint(playerPos.Value);

        // panic check
        if (EntityManager.HasComponent<AvatarStatusComponent>(_playerEntity)) {
            AvatarStatusComponent avatarStatusComp = EntityManager.GetComponentData<AvatarStatusComponent>(_playerEntity);
            if(avatarStatusComp.InPanic) {
                bubbleMassage = "#$%^";
            }
        }

        // todo - temporary
        int showMessageLength = (int)((float)bubbleMassage.Length * timeRate);

        // set
        _guiPreset.ShowBubble(convert2DPos, bubbleMassage.Substring(0, showMessageLength));
    }


    private void UpdateGaugeUI() {
        AvatarStatusComponent statusComp = EntityManager.GetComponentData<AvatarStatusComponent>(_playerEntity);
        _guiPreset.SetMadness(statusComp.madness / statusComp.maxMadness);
    }


    protected override void OnStartRunning() {
        Entities.ForEach((TablePresetComponent presetComp) => {
            _tablePreset = presetComp.preset;
        });

        Entities.ForEach((GUIPresetComponent presetComp) => {
            _guiPreset = presetComp.preset;
        });

        Entities.WithAll<PlayerComponent>().ForEach((Entity entity) => {
            _playerEntity = entity;
        });

        if (null != _guiPreset) {
            _guiPreset.Initialize();
        }
    }


    protected override void OnUpdate() {
        if (null == _guiPreset) {
            return;
        }

        if (_playerEntity.Equals(Entity.Null)) {
            return;
        }

        // set gui - inventory
        UpdateInventoryUI();

        // set gui - bubble
        UpdateBubbleUI();

        // set gui - madness
        UpdateGaugeUI();
    }
}
