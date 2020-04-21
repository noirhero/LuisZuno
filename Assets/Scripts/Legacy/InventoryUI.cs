// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using GlobalDefine;
using UnityEngine.UI;
using UnityEngine;

public class InventoryUI : LegacyUI {
    public Image[] item = new Image[3];
    private InventoryComponent _inventoryComp;

    
    public override void Show() {
        base.Show();
        _inventoryComp = Utility._entityMng.GetComponentData<InventoryComponent>(Utility._playerEntity);
    }

    
    private void Update() {
        ItemPresetData data;
        Utility._tablePreset.itemDatas.TryGetValue(_inventoryComp.item1.id, out data);
        SetItemSprite(0, data.sprite);
        Utility._tablePreset.itemDatas.TryGetValue(_inventoryComp.item2.id, out data);
        SetItemSprite(1, data.sprite);
        Utility._tablePreset.itemDatas.TryGetValue(_inventoryComp.item3.id, out data);
        SetItemSprite(2, data.sprite);
    }

    
    protected void SetItemSprite(int index, Sprite inImg) {
        if (item.Length >= index || index < 0) {
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
