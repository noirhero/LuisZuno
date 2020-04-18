// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class BubbleUI : LegacyUI {
    public Text bubbleMsg;
    [FormerlySerializedAs("Pivot")] public Vector3 pivot = new Vector3(20.0f, 170.0f, 0.0f);
}
