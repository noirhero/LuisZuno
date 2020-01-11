// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Serialization;

namespace GlobalDefine {
    public enum AnimationType { Walk, Idle, SomethingDoIt, NyoNyo }
    public enum BackgroundType { BackStreetBoy, Poem, VeteranSoldier, Priest, Professor, Detective }
    public enum ValuesType { Mercy, Greedy, Curiosity, SenseOfDuty }
    public enum GoalType { TraceOfParents, CreateOfMasterpiece, Rich }

    [Serializable]
    public struct ItemStruct {
        public Int64 id;
        [FormerlySerializedAs("AddedTime")] public Int64 addedTime;
        
        public void Empty() {
            id = 0;
            addedTime = 0;
        }
    }

    public static class Utility {
        public static bool IsValid(Int64 index) {
            return index != 0;
        }
        public static T ToEnum<T>(string inValue) {
            return (T)Enum.Parse(typeof(T), inValue);
        }
        public static bool SpawnEffect(int inIndex, ref EntityCommandBuffer.Concurrent inCmdBuf, 
            [ReadOnly] Entity inPrefabs, float3 inPos) {
            if (Entity.Null == inPrefabs)
                return false;

            Entity effect = inCmdBuf.CreateEntity(inIndex);
            inCmdBuf.AddComponent(inIndex, effect, new Translation() {
                Value = inPos,
            });
            inCmdBuf.AddComponent(inIndex, effect, new EffectSpawnComponent() {
                prefab = inPrefabs,
                lifetime = 0.4f,
                duration = 0.0f,
            });
            return true;
        }
        public static bool SetLifeCycle(int inIndex, ref EntityCommandBuffer.Concurrent inCmdBuf, 
            ref Entity inEntity, float inLifeTime) {
            inCmdBuf.AddComponent(inIndex, inEntity, new LifeCycleComponent() {
                spawnEffect = Entity.Null,
                destroyEffect = Entity.Null,
                lifetime = inLifeTime,
                duration = 0.0f,
            });
            return true;
        }
        public static bool SetLifeCycle(int inIndex, ref EntityCommandBuffer.Concurrent inCmdBuf,
            ref Entity inEntity, float inLifeTime, ref Entity inSpawnEffect, ref Entity inDestroyEffect) {
            inCmdBuf.AddComponent(inIndex, inEntity, new LifeCycleComponent() {
                spawnEffect = inSpawnEffect,
                destroyEffect = inDestroyEffect,
                lifetime = inLifeTime,
                duration = 0.0f,
            });
            return true;
        }
        public static bool SetLifeCycle(ref Entity inEntity, float inLifeTime, ref Entity inSpawnEffect, ref Entity inDestroyEffect) {
            World.Active.EntityManager.AddComponentData(inEntity, new LifeCycleComponent() {
                spawnEffect = inSpawnEffect,
                destroyEffect = inDestroyEffect,
                lifetime = inLifeTime,
                duration = 0.0f,
            });
            return true;
        }
    }

    public static class BehaviorState {
        public const int searching = 0x1;
        public const int panic = 0x2;
        public const int pendingItem = 0x4;
        public const int spawning = 0x8;

        public static bool HasState(PlayerComponent playerComp, int compareState) { return ((playerComp.currentBehaviors & compareState) != 0); }
    }
}
