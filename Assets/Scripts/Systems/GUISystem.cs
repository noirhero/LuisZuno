// Copyrighgt 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

[UpdateAfter(typeof(GameSystem))]
public class GUISystem : ComponentSystem {
    private GUIPreset _guiPreset;

    
    protected override void OnStartRunning() {
        Entities.ForEach((GUIPresetComponent presetComp) => {
            _guiPreset = presetComp.preset;
            _guiPreset.Initialize();
        });
    }


    protected override void OnUpdate() {
        Entities.ForEach((Entity playerEntity, ref GUIComponent guiComp) => {
            if (EntityManager.HasComponent<GUISyncComponent>(Utility.playerEntity)) {
                // common
                guiComp.currentUI = GUIState.clear;
                
                // case by case
                var guiSyncComp = EntityManager.GetComponentData<GUISyncComponent>(Utility.playerEntity);
                switch (guiSyncComp.currentGroup) {
                    case GUIGroupType.GameStart:
                        guiComp.currentUI |= GUIState.customize;
                        break;
                    case GUIGroupType.GameOver:
                        guiComp.currentUI |= GUIState.ending;
                        break;
                    case GUIGroupType.GameClear:
                        guiComp.currentUI |= GUIState.ending;
                        break;
                    case GUIGroupType.GamePlay:
                        UpdatePlayUI(ref guiComp);
                        break;
                }
                EntityManager.RemoveComponent<GUISyncComponent>(Utility.playerEntity);
            }

            if (0 != guiComp.currentUI) {
                UpdateUIPreset(ref guiComp);   
            }
        });

        // Teleport
        TeleportByUI();
    }

    
    private void UpdatePlayUI(ref GUIComponent guiComp) {
        // condition
        foreach (var system in World.DefaultGameObjectInjectionWorld.Systems) {
            if (system is TeleportSystem) {
                TeleportSystem teleportSystem = system as TeleportSystem;
                switch (teleportSystem.CurSubSceneType) {
                    case SubSceneType.sceneSelect :
                        guiComp.currentUI |= GUIState.scenarioSelect;
                        break;
                    case SubSceneType.Scenario001_Hallway :
                    case SubSceneType.Scenario001_Basement :
                        guiComp.currentUI |= GUIState.inventory;
                        guiComp.currentUI |= GUIState.bubble;
                        break;
                    case SubSceneType.Scenario001_LegacyOfClan :
                        guiComp.currentUI |= GUIState.inventory;
                        guiComp.currentUI |= GUIState.bubble;
                        guiComp.currentUI |= GUIState.madness;
                        break;
                    default:
                        guiComp.currentUI |= GUIState.customize;
                        break;
                }
            }
        }
    }
    
    

    private void UpdateUIPreset(ref GUIComponent guiComp) {
            // Clear
            if (GUIState.HasState(guiComp, GUIState.clear)) {
                _guiPreset.ending.Hide();
                _guiPreset.bubble.Hide();
                _guiPreset.madness.Hide();
                _guiPreset.inventory.Hide();
                _guiPreset.customize.Hide();
                _guiPreset.scenarioSelect.Hide();
                guiComp.currentUI ^= GUIState.clear;
            }
            else {
                // Customize
                if (GUIState.HasState(guiComp, GUIState.customize)) {
                    _guiPreset.customize.Show();
                    guiComp.currentUI ^= GUIState.customize;
                }
            
                // ScenarioSelect
                if (GUIState.HasState(guiComp, GUIState.scenarioSelect)) {
                    _guiPreset.scenarioSelect.Show();
                    guiComp.currentUI ^= GUIState.scenarioSelect;
                }
            
                // Inventory
                if (GUIState.HasState(guiComp, GUIState.inventory)) {
                    _guiPreset.inventory.Show();
                    guiComp.currentUI ^= GUIState.inventory;
                }
            
                // Madness
                if (GUIState.HasState(guiComp, GUIState.madness)) {
                    _guiPreset.madness.Show();
                    guiComp.currentUI ^= GUIState.madness;
                }
            
                // Bubble
                if (GUIState.HasState(guiComp, GUIState.bubble)) {
                    _guiPreset.bubble.Show();
                    guiComp.currentUI ^= GUIState.bubble;
                }

                // Ending
                if (GUIState.HasState(guiComp, GUIState.ending)) {
                    _guiPreset.ending.Show();
                    guiComp.currentUI ^= GUIState.ending;
                }
            }
    }
    
    
    private void TeleportByUI() {
        if (EntityManager.HasComponent<TeleportInfoComponent>(Utility.playerEntity)) {
            Entities.ForEach((Entity playerEntity, ref PlayerComponent playerComp, ref TeleportInfoComponent teleportInfoComp) => {
                if (playerComp.currentBehaviors > 0) {
                    return;
                }
                EntityManager.RemoveComponent<TeleportInfoComponent>(playerEntity);
                EntityManager.AddComponentData(playerEntity, new TeleportComponent(ref teleportInfoComp));
                playerComp.currentBehaviors |= BehaviorState.teleport;
            });
        }
    }
}
