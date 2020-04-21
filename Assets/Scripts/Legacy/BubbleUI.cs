// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using GlobalDefine;
using UnityEngine;
using UnityEngine.UI;
using Unity.Transforms;

public class BubbleUI : LegacyUI {
    public Text bubbleMsg;
    public Image bubbleImg;
    public Vector3 pivot = new Vector3(20.0f, 170.0f, 0.0f);

    
    private void Update() {
        string cachedStr = GetBubbleMessage();
        if (string.Empty == cachedStr) {
            bubbleImg.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
        }
        else {
            bubbleImg.color = Color.white;
            Translation playerPos = Utility.entityMng.GetComponentData<Translation>(Utility.playerEntity);
            Vector3 convert2DPos = Camera.main.WorldToScreenPoint(playerPos.Value);
            convert2DPos += pivot;
            GetTransform().SetPositionAndRotation(convert2DPos, Quaternion.identity);
        }
        
        bubbleMsg.text = cachedStr;
    }
    
    
    private string GetBubbleMessage() {
        var playerComp = Utility.entityMng.GetComponentData<PlayerComponent>(Utility.playerEntity);
        if (0 == playerComp.currentBehaviors) {     // walking or doing nothing
            return string.Empty;
        }

        // bubble default
        string bubbleMassage = "... ";
        float timeRate = 0.0f;

        // Searching
        if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
            var searchingComp = Utility.entityMng.GetComponentData<SearchingComponent>(Utility.playerEntity);
            if (0 < searchingComp.elapsedSearchingTime) {
                timeRate = searchingComp.elapsedSearchingTime / searchingComp.searchingTime;
            }
        }
        // Panic
        else if (BehaviorState.HasState(playerComp, BehaviorState.panic)) {
            var panicComp = Utility.entityMng.GetComponentData<PanicComponent>(Utility.playerEntity);
            if (0 < panicComp.elapsedPanicTime) {
                bubbleMassage = "#$%^";
                timeRate = panicComp.elapsedPanicTime / panicComp.panicTime;
            }
        }

        if (0.0f >= timeRate) {
            return string.Empty;
        }

        // todo - temporary
        int showMessageLength = (int)((float)bubbleMassage.Length * timeRate);

        return bubbleMassage.Substring(0, showMessageLength);
    }
}
