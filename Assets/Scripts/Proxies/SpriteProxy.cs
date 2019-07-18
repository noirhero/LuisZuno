// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

[Serializable]
public struct SpriteData {
    public AnimationClip clip;
    public List<Sprite> sprites;
}

[RequiresEntityConversion]
public class SpriteProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public Mesh mesh = null;
    public Material material = null;
    public List<AnimationClip> clips = new List<AnimationClip>();

    public List<SpriteData> spriteDatas = new List<SpriteData>();

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        int dataCount = spriteDatas.Count;

        Texture[] textures = new Texture[dataCount];

        int[] nameHashes = new int[dataCount];
        float[] lengths = new float[dataCount];
        float[] frameRates = new float[dataCount];
        int[] offsets = new int[dataCount];
        int[] counts = new int[dataCount];
        float3[] scales = new float3[dataCount];
        float3[] posOffsets = new float3[dataCount];

        Vector4[] rects = new Vector4[dataCount * 16];


        int idx = 0;
        int offset = 0;
        foreach(var spriteData in spriteDatas) {
            textures[idx] = spriteData.sprites[0].texture;

            foreach (var b in Encoding.ASCII.GetBytes(spriteData.clip.name)) {
                nameHashes[idx] += b;
            }

            lengths[idx] = spriteData.clip.length;
            frameRates[idx] = spriteData.clip.length / (float)spriteData.sprites.Count;
            offsets[idx] = offset;
            counts[idx] = spriteData.sprites.Count;

            float pixelRatioX = spriteData.sprites[0].texture.width / 100.0f;
            float pixelRatioY = spriteData.sprites[0].texture.height / 100.0f;
            float scaleX = spriteData.sprites[0].rect.width / spriteData.sprites[0].texture.width * pixelRatioX;
            float scaleY = spriteData.sprites[0].rect.height / spriteData.sprites[0].texture.height * pixelRatioY;
            
            scales[idx] = new float3(scaleX, scaleY, 1.0f);
            posOffsets[idx] = new float3(0.0f, scaleY * 0.5f, 0.0f);

            int spriteIdx = 0;
            foreach(var sprite in spriteData.sprites) {
                rects[spriteIdx++].Set(
                    sprite.rect.width / sprite.texture.width,
                    sprite.rect.height / sprite.texture.height,
                    sprite.rect.x / sprite.texture.width,
                    sprite.rect.y / sprite.texture.height
                );
            }

            ++idx;
            offset += spriteData.sprites.Count;
        }

        dstManager.AddSharedComponentData(entity, new SpriteMeshComponent() {
            mesh = mesh,
            material = material,
            textures = textures,
            nameHashes = new NativeArray<int>(nameHashes, Allocator.Persistent),
            lengths = new NativeArray<float>(lengths, Allocator.Persistent),
            frameRates = new NativeArray<float>(frameRates, Allocator.Persistent),
            offsets = new NativeArray<int>(offsets, Allocator.Persistent),
            counts = new NativeArray<int>(counts, Allocator.Persistent),
            scales = new NativeArray<float3>(scales, Allocator.Persistent),
            posOffsets = new NativeArray<float3>(posOffsets, Allocator.Persistent),
            rects = new NativeArray<Vector4>(rects, Allocator.Persistent)
        });

        dstManager.AddComponentData(entity, new SpriteAnimComponent() {
            nameHash = nameHashes[0]
        });
    }
}
