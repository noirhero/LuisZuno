// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using GlobalDefine;

public class EndingUI : LegacyUI {
    public Text endingMsg;
    public float speed = 1.0f;


    private void Start() {
        Utility.entityMng.AddComponentData(Utility.playerEntity, new TeleportInfoComponent(
            SceneType.Cenetery,SubSceneType.None,SubSceneType.None, 0,0.0f, 0.0f));
    }

    private void Update() {
        var convert2DPos = Camera.main.WorldToViewportPoint(GetTransform().localPosition);
        if (convert2DPos.y <= 0.0f) {
            var destPos = Vector3.up * (Time.deltaTime * speed);
            GetTransform().Translate(destPos);   
        }
    }
}