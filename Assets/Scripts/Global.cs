// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using System.Linq;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using UnityEditor;

namespace GlobalDefine {
    public enum AnimationType { Walk, Idle, SomethingDoIt, NyoNyo }
    public enum BackgroundType { BackStreetBoy, Poet, VeteranSoldier, Priest, Professor, Detective }
    public enum ValuesType { Mercy, Greedy, Curiosity, SenseOfDuty }
    public enum GoalType { TraceOfParents, CreateOfMasterpiece, Rich }
    public enum SceneType { None, Cenetery, Town, Scenario001 }
    public enum SubSceneType { None, Crafting, Housing, sceneSelect, Scenario001_Hallway, Scenario001_Basement, Scenario001_LegacyOfClan }

    [Serializable]
    public struct CharacterBackground {
        public BackgroundType type;
        public float madness;
        public float agility;
        public float physical;
        public float mentality;
        public float search;
        public float luck;
    }

    [Serializable]
    public struct ItemStruct {
        public Int64 id;
        public Int64 addedTime;

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


        public static bool SpawnEffect(int inIndex, [ReadOnly] Entity inPrefabs, float3 inPos,
            in EntityCommandBuffer.Concurrent inCmdBuf) {
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


        public static bool SetLifeCycle(int inIndex, in Entity inEntity, float inLifeTime,
            in Entity inSpawnEffect, in Entity inDestroyEffect, in EntityCommandBuffer.Concurrent inCmdBuf) {
            inCmdBuf.AddComponent(inIndex, inEntity, new LifeCycleComponent() {
                spawnEffect = inSpawnEffect,
                destroyEffect = inDestroyEffect,
                lifetime = inLifeTime,
                duration = 0.0f,
            });
            return true;
        }


        public static bool SetLifeCycle(in Entity inEntity, float inLifeTime, 
            in Entity inSpawnEffect, in Entity inDestroyEffect) {
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(inEntity, new LifeCycleComponent() {
                spawnEffect = inSpawnEffect,
                destroyEffect = inDestroyEffect,
                lifetime = inLifeTime,
                duration = 0.0f,
            });
            return true;
        }


        public static T LoadObjectAtPath<T> (string inPath) where T : UnityEngine.Object {
            if (0 >= inPath.Length) {
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<T>(inPath);
        }
        
        
        private static EntityManager _cachedEntityMng = null;
        public static EntityManager entityMng {
            get {
                if (World.DefaultGameObjectInjectionWorld.EntityManager != _cachedEntityMng) {
                    _cachedEntityMng = World.DefaultGameObjectInjectionWorld.EntityManager;
                }
                return _cachedEntityMng;
            }
        }
        
        
        private static Entity _cachedPlayerEntity = Entity.Null;
        public static Entity playerEntity {
            get {
                if (Entity.Null == _cachedPlayerEntity) {
                    var cachedEntities = entityMng.GetAllEntities()
                        .Where(entity => entityMng.HasComponent(entity, typeof(PlayerComponent)));

                    foreach (var entity in cachedEntities) {
                        _cachedPlayerEntity = entity;
                        break;
                    }
                }
                return _cachedPlayerEntity;
            }
        }
        
        
        private static TablePreset _cachedTablePreset = null;
        public static TablePreset tablePreset {
            get {
                if (null == _cachedTablePreset) {
                    var cachedEntities = entityMng.GetAllEntities()
                        .Where(entity => entityMng.HasComponent(entity, typeof(TablePresetComponent)));
                    foreach (var entity in cachedEntities) {
                        _cachedTablePreset = entityMng.GetSharedComponentData<TablePresetComponent>(entity).preset;
                        break;
                    }
                }
                return _cachedTablePreset;
            }
        }
    }

    public static class BehaviorState {
        public const int searching = 0x1;
        public const int panic = 0x2;
        public const int pendingItem = 0x4;
        public const int spawning = 0x8;
        public const int teleport = 0x10;

        public static bool HasState(PlayerComponent playerComp, int compareState) { return ((playerComp.currentBehaviors & compareState) != 0); }
    }
    
    public static class GUIState {
        public const int none = 0x1;
        public const int customize = 0x2;
        public const int scenarioSelect = 0x4;
        public const int inventory = 0x8;
        public const int madness = 0x10;
        public const int bubble = 0x20;
        public const int ending = 0x40;

        public static bool HasState(GUIComponent guiComp, int compareState) { return ((guiComp.currentUI & compareState) != 0); }
    }
}
