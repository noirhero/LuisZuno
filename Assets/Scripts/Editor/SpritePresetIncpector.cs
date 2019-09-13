// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpritePreset))]
[CanEditMultipleObjects]
public class SpritePresetIncpector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if (true == GUILayout.Button("Set clips and then click.")) {
            InitializePrset(target as SpritePreset);
        }
    }

    private void InitializePrset(SpritePreset preset) {
        preset.datas.Clear();

        foreach (var clip in preset.clips) {
            List<Sprite> sprites = new List<Sprite>();

            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip)) {
                foreach (var frame in AnimationUtility.GetObjectReferenceCurve(clip, binding)) {
                    sprites.Add((Sprite) frame.value);
                }
            }

            if (0 == sprites.Count) {
                continue;
            }

            var firstSprite = sprites[0];
            float pixelRatioX = firstSprite.texture.width / 100.0f;
            float pixelRatioY = firstSprite.texture.height / 100.0f;
            float scaleX = firstSprite.rect.width / firstSprite.texture.width * pixelRatioX;
            float scaleY = firstSprite.rect.height / firstSprite.texture.height * pixelRatioY;

            var presetData = new SpritePresetData() {
                texture = sprites[0].texture,
                length = clip.length,
                frameRate = clip.length / (float) sprites.Count,
                scale = new float3(scaleX, scaleY, 1.0f),
                posOffset = new float3(0.0f, scaleY * 0.5f, 0.0f),
                rects = new List<Vector4>()
            };
            foreach (var sprite in sprites) {
                presetData.rects.Add(new Vector4(
                    sprite.rect.width / sprite.texture.width,
                    sprite.rect.height / sprite.texture.height,
                    sprite.rect.x / sprite.texture.width,
                    sprite.rect.y / sprite.texture.height));
            }

            int hash = 0;
            foreach (var b in Encoding.ASCII.GetBytes(clip.name)) {
                hash += b;
            }

            preset.datas.Add(hash, presetData);
        }
    }
}