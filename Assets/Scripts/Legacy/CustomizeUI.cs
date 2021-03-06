﻿// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using GlobalDefine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class CustomizeUI : LegacyUI {
    public ToggleGroup backgroundGroup;
    public ToggleGroup valuesGroup;
    public ToggleGroup goalGroup;

    public Text backgroundText;
    public Text valuesText;
    public Text goalText;

    public Text remainText;
    public Text mentalityText;
    public Text agilityText;
    public Text physicalText;
    public Text searchText;
    public Text luckText;

    public GameObject decision;

    private CharacterBackground[] _defaultBackgrounds;

    
    private bool IsPlayerEntityHasCustomizeComponent() {
        return Utility.entityMng.HasComponent<CustomizeComponent>(Utility.playerEntity);
    }

    
    void Start() {
        decision.SetActive(false);
        
        InitializeDefaultBackgrounds();

        OnSelectedInBackground();
        OnSelectedInValues();
        OnSelectedInGoal();
        OnSelectedInMentality(0);
        OnSelectedInAgility(0);
        OnSelectedInPhysical(0);
        OnSelectedInSearch(0);
        OnSelectedInLuck(0);
    }


    void InitializeDefaultBackgrounds() {
        var types = Enum.GetValues(typeof(BackgroundType));
        _defaultBackgrounds = new CharacterBackground[types.Length];

        foreach (BackgroundType type in types) {
            switch (type) {
                case BackgroundType.BackStreetBoy: {
                        _defaultBackgrounds[(int)type] = new CharacterBackground() {
                            type = BackgroundType.BackStreetBoy,
                            madness = 0,
                            mentality = 120,
                            agility = 120,
                            physical = 70,
                            search = 110,
                            luck = 0
                        };
                    }
                    break;
                case BackgroundType.Poet: {
                        _defaultBackgrounds[(int)type] = new CharacterBackground() {
                            type = BackgroundType.Poet,
                            madness = 0,
                            mentality = 100,
                            agility = 110,
                            physical = 90,
                            search = 85,
                            luck = 20
                        };
                    }
                    break;
                case BackgroundType.VeteranSoldier: {
                        _defaultBackgrounds[(int)type] = new CharacterBackground() {
                            type = BackgroundType.VeteranSoldier,
                            madness = 0,
                            mentality = 90,
                            agility = 90,
                            physical = 160,
                            search = 80,
                            luck = 0
                        };
                    }
                    break;
                case BackgroundType.Priest: {
                        _defaultBackgrounds[(int)type] = new CharacterBackground() {
                            type = BackgroundType.Priest,
                            madness = 0,
                            mentality = 105,
                            agility = 100,
                            physical = 90,
                            search = 100,
                            luck = 5
                        };
                    }
                    break;
                case BackgroundType.Professor: {
                        _defaultBackgrounds[(int)type] = new CharacterBackground() {
                            type = BackgroundType.Professor,
                            madness = 20,
                            mentality = 150,
                            agility = 70,
                            physical = 85,
                            search = 120,
                            luck = 0
                        };
                    }
                    break;
                case BackgroundType.Detective: {
                        _defaultBackgrounds[(int)type] = new CharacterBackground() {
                            type = BackgroundType.Detective,
                            madness = 0,
                            mentality = 100,
                            agility = 110,
                            physical = 100,
                            search = 150,
                            luck = 0
                        };
                    }
                    break;
            }
        }
    }


    void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            OnSelectedInConfirm();
        }
    }


    public override void Show() {
        base.Show();
        Utility.entityMng.AddComponentData(Utility.playerEntity, new CustomizeComponent());
    }


    public override void Hide() {
        base.Hide();
        Utility.entityMng.RemoveComponent<CustomizeComponent>(Utility.playerEntity);
    }


    public void OnSelectedInBackground() {
        foreach (var toggle in backgroundGroup.ActiveToggles()) {
            backgroundText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);
            customizeComp.backgroundType = Utility.ToEnum<BackgroundType>(toggle.name);
            Utility.entityMng.SetComponentData(Utility.playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInValues() {
        foreach (var toggle in valuesGroup.ActiveToggles()) {
            valuesText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);
            customizeComp.valuesType = Utility.ToEnum<ValuesType>(toggle.name);
            Utility.entityMng.SetComponentData(Utility.playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInGoal() {
        foreach (var toggle in goalGroup.ActiveToggles()) {
            goalText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);
            customizeComp.goalType = Utility.ToEnum<GoalType>(toggle.name);
            Utility.entityMng.SetComponentData(Utility.playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInDecision() {
        decision.SetActive(true);
    }


    public void OnSelectedInConfirm() {
        AdjustPlayerStatus();

        decision.SetActive(false);
        
        Utility.entityMng.AddComponentData(Utility.playerEntity, new TeleportInfoComponent(
            SceneType.Town, SubSceneType.None, SubSceneType.sceneSelect,0, 0.0f, 0.0f));
    }


    public void OnSelectedInCancel() {
        decision.SetActive(false);
    }

    
    public void OnSelectedInMentality(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.mentality + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.mentality = pendingValue;
        Utility.entityMng.SetComponentData(Utility.playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        mentalityText.text = customizeComp.mentality.ToString();
    }


    public void OnSelectedInAgility(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.agility + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.agility = pendingValue;
        Utility.entityMng.SetComponentData(Utility.playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        agilityText.text = customizeComp.agility.ToString();
    }


    public void OnSelectedInPhysical(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.physical + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.physical = pendingValue;
        Utility.entityMng.SetComponentData(Utility.playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        physicalText.text = customizeComp.physical.ToString();
    }


    public void OnSelectedInSearch(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.search + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.search = pendingValue;
        Utility.entityMng.SetComponentData(Utility.playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        searchText.text = customizeComp.search.ToString();
    }


    public void OnSelectedInLuck(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.luck + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.luck = pendingValue;
        Utility.entityMng.SetComponentData(Utility.playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        luckText.text = customizeComp.luck.ToString();
    }


    public void AdjustPlayerStatus() {
        // the player must have these components
        var playerStatusComp = Utility.entityMng.GetComponentData<PlayerStatusComponent>(Utility.playerEntity);
        var customizeComp = Utility.entityMng.GetComponentData<CustomizeComponent>(Utility.playerEntity);

        playerStatusComp.status = _defaultBackgrounds[(int)customizeComp.backgroundType];
        playerStatusComp.moveSpeed = playerStatusComp.status.agility * 0.01f;
        playerStatusComp.madnessWeight = playerStatusComp.status.mentality * 0.01f;
        playerStatusComp.searchingWeight = playerStatusComp.status.search * 0.01f;

        var playerPresetComp = Utility.entityMng.GetSharedComponentData<PlayerPresetComponent>(Utility.playerEntity);
        playerPresetComp.preset.prefabPaths.TryGetValue(customizeComp.backgroundType, out var prefabPath);
        AdjustPlayerSprite(ref prefabPath, in playerPresetComp.sprite);

        Utility.entityMng.SetComponentData<PlayerStatusComponent>(Utility.playerEntity, playerStatusComp);
    }


    private void AdjustPlayerSprite(ref string prefabPath, in Sprite sprite) {
        var preset = Utility.LoadObjectAtPath<GameObject>(prefabPath);
        if (ReferenceEquals(null, preset)) {
            Debug.LogError($"Player preset load failed : {prefabPath}");
            return;
        }

        var spritePreset = preset.GetComponent<SpritePreset>();
        if (false == ReferenceEquals(null, spritePreset)) {
            if (Utility.entityMng.HasComponent<SpritePresetComponent>(Utility.playerEntity)) {
                Utility.entityMng.SetSharedComponentData(Utility.playerEntity, new SpritePresetComponent() {
                    preset = spritePreset
                });
                Utility.entityMng.SetComponentData(Utility.playerEntity, new SpriteStateComponent() {
                    hash = spritePreset.datas.Keys.First()
                });
            }
            else {
                Utility.entityMng.AddSharedComponentData(Utility.playerEntity, new SpritePresetComponent() {
                    preset = spritePreset
                });
                Utility.entityMng.AddComponentData(Utility.playerEntity, new SpriteStateComponent() {
                    hash = spritePreset.datas.Keys.First()
                });
            }
        }

        var presetSpriteRenderer = preset.GetComponent<SpriteRenderer>();
        var presetSprite = presetSpriteRenderer ? presetSpriteRenderer.sprite : null;
        if (false == ReferenceEquals(null, presetSprite)) {
            var localScale = preset.GetComponent<Transform>().localScale;
            var spriteScale = new float3(presetSprite.rect.width, presetSprite.rect.height, 1.0f) / presetSprite.pixelsPerUnit;
            spriteScale *= localScale;
            spriteScale.z = 1.0f;
            Utility.entityMng.AddComponentData(Utility.playerEntity, new NonUniformScale() {
                Value = spriteScale
            });

            var applyPivot = (presetSprite.rect.center - presetSprite.pivot) / presetSprite.pixelsPerUnit * localScale;
            Utility.entityMng.AddComponentData(Utility.playerEntity, new SpritePivotComponent() {
                Value = new float3(applyPivot, 0.0f)
            });
        }
    }
}
