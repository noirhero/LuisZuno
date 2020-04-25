// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

public class GameSystem : ComponentSystem {
    protected override void OnStartRunning() {
        if (Entity.Null == Utility.playerEntity || null == Utility.tablePreset) {
            EnableSystem(false);
        }
        else {
            EntityManager.AddComponentData(Utility.playerEntity, new GameStartComponent());
            EntityManager.AddComponentData(Utility.playerEntity, new GUIComponent() {
                currentUI = GUIState.customize,
            });
        }
    }

    
    protected override void OnUpdate() {
        if (Entity.Null == Utility.playerEntity || null == Utility.tablePreset)  {
            return;
        }

        if (EntityManager.HasComponent<GameStartComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GameStartComponent>(Utility.playerEntity);
            EnableSystem(true);
            return;
        }

        if (EntityManager.HasComponent<GameClearComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GameClearComponent>(Utility.playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GameOverComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GameOverComponent>(Utility.playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GamePauseComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GamePauseComponent>(Utility.playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GameResumeComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GameResumeComponent>(Utility.playerEntity);
            EnableSystem(true);
            UpdateUI();
            return;
        }
    }

    
    private void EnableSystem(bool inEnable) {
        foreach (var system in World.DefaultGameObjectInjectionWorld.Systems) {
            if (system.GetType() == typeof(TargetingSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(AutoMovementSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(IntelligenceSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(EffectSpawnSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(EntitySpawnSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(LifeCycleSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(HoldingSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(SearchingSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(PanicSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(MadnessSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(GUISystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(InventorySystem)) {
                system.Enabled = inEnable;
            }
        }
    }


    private void UpdateUI() {
        // 이동후 게임재개시 보여질 ui
        foreach (var system in World.DefaultGameObjectInjectionWorld.Systems) {
            if (system is TeleportSystem) {
                var guiComp = Utility.entityMng.GetComponentData<GUIComponent>(Utility.playerEntity);
                TeleportSystem teleportSystem = system as TeleportSystem;
                switch (teleportSystem.CurSubSceneType) {
                    case SubSceneType.sceneSelect :
                        guiComp.currentUI = GUIState.scenarioSelect;
                        break;
                    case SubSceneType.Scenario001_Hallway :
                    case SubSceneType.Scenario001_Basement :
                        guiComp.currentUI = GUIState.inventory;
                        guiComp.currentUI |= GUIState.bubble;
                        break;
                    case SubSceneType.Scenario001_LegacyOfClan :
                        guiComp.currentUI = GUIState.inventory;
                        guiComp.currentUI |= GUIState.bubble;
                        guiComp.currentUI |= GUIState.madness;
                        break;
                    default:
                        guiComp.currentUI = GUIState.none;
                        break;
                }
                Utility.entityMng.SetComponentData(Utility.playerEntity, guiComp);
            }
        }
    }
}
