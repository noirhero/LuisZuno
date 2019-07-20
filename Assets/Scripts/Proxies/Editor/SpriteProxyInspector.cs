// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteProxy))]
[CanEditMultipleObjects]
public class SpriteProxyInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if(true == GUILayout.Button("Set clips and then click.")) {
            InitializeSprites(target as SpriteProxy);
        }
    }

    private void InitializeSprites(SpriteProxy proxy) {
        proxy.spriteDatas.Clear();

        foreach(var clip in proxy.clips) {
            List<Sprite> sprites = new List<Sprite>();

            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip)) {
                foreach (var frame in AnimationUtility.GetObjectReferenceCurve(clip, binding)) {
                    sprites.Add((Sprite)frame.value);
                }
            }
            if(0 == sprites.Count) {
                continue;
            }

            proxy.spriteDatas.Add(new SpriteData() {
                clip = clip,
                sprites = sprites
            });
        }
    }
}
