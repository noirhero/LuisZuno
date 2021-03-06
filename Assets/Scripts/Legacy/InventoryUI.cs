﻿// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using GlobalDefine;
using UnityEngine.UI;
using UnityEngine;

public class InventoryUI : LegacyUI {
    public Image[] item = new Image[3];

    
    private void Update() {
        ItemPresetData data;
        var inventoryComp = Utility.entityMng.GetComponentData<InventoryComponent>(Utility.playerEntity);
        Utility.tablePreset.itemDatas.TryGetValue(inventoryComp.item1.id, out data);
        SetItemSprite(0, data.sprite);
        Utility.tablePreset.itemDatas.TryGetValue(inventoryComp.item2.id, out data);
        SetItemSprite(1, data.sprite);
        Utility.tablePreset.itemDatas.TryGetValue(inventoryComp.item3.id, out data);
        SetItemSprite(2, data.sprite);
    }

    
    protected void SetItemSprite(int index, Sprite inImg) {
        if (item.Length <= index || index < 0) {
            return;    
        }
        
        if (null == inImg) {
            item[index].gameObject.SetActive(false);
        }
        else {
            item[index].gameObject.SetActive(true);
            item[index].sprite = inImg;
        }
    }
}
