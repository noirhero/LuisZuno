// Copyrighgt 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

public class GUISystem : ComponentSystem {
    private GUIPreset _guiPreset;


    protected override void OnStartRunning() {
        Entities.ForEach((GUIPresetComponent presetComp) => {
            _guiPreset = presetComp.preset;
            _guiPreset.Initialize();
        });
        
        EntityManager.AddComponentData(Utility._playerEntity, new GameStartComponent());
        EntityManager.AddComponentData(Utility._playerEntity, new GUIComponent(){
            currentUI = GUIState.customize,
        });
    }


    protected override void OnUpdate() {
        Entities.ForEach((Entity playerEntity, ref GUIComponent guiComp) => { 
            // Customize
            if (GUIState.HasState(guiComp, GUIState.customize)) {
                if (false == EntityManager.HasComponent<CustomizeComponent>(Utility._playerEntity)) {
                    EntityManager.AddComponentData(Utility._playerEntity, new CustomizeComponent());
                    EntityManager.AddComponentData(Utility._playerEntity, new GamePauseComponent());
                    _guiPreset.customize.Show();
                }
            }
            else {
                if (EntityManager.HasComponent<CustomizeComponent>(Utility._playerEntity)) {
                    EntityManager.RemoveComponent<CustomizeComponent>(Utility._playerEntity);
                    EntityManager.AddComponentData(Utility._playerEntity, new GameResumeComponent());
                    _guiPreset.customize.Hide();
                }
            }
            
            // ScenarioSelect
            if (GUIState.HasState(guiComp, GUIState.scenarioSelect)) {
                _guiPreset.scenarioSelect.Show();
            }
            else {
                _guiPreset.scenarioSelect.Hide();
            }
            
            // Inventory
            if (EntityManager.HasComponent<InventoryComponent>(Utility._playerEntity)) {
                if (GUIState.HasState(guiComp, GUIState.inventory)) {
                    _guiPreset.inventory.Show();
                }
                else {
                    _guiPreset.inventory.Hide();
                }
            }
            
            // Madness
            if (GUIState.HasState(guiComp, GUIState.madness)) {
                _guiPreset.madness.Show();
            }
            else {
                _guiPreset.madness.Hide();
            }
            
            // Bubble
            if (GUIState.HasState(guiComp, GUIState.bubble)) {
                _guiPreset.bubble.Show();
            }
            else {
                _guiPreset.bubble.Hide();
            }

            // Ending
            if (GUIState.HasState(guiComp, GUIState.ending)) {
                _guiPreset.ending.Show();
            }
            else {
                _guiPreset.ending.Hide();
            }
        });

        // Teleport
        if (EntityManager.HasComponent<TeleportInfoComponent>(Utility._playerEntity)) {
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
