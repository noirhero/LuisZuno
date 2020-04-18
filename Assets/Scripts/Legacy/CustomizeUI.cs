// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Unity.Entities;
using GlobalDefine;
using Unity.Mathematics;
using Unity.Transforms;

public class CustomizeUI : MonoBehaviour {
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

    private Entity _playerEntity = Entity.Null;
    private EntityManager _cachedEntityMng;
    private EntityManager _EntityMng {
        get {
            if (World.DefaultGameObjectInjectionWorld.EntityManager != _cachedEntityMng) {
                _cachedEntityMng = World.DefaultGameObjectInjectionWorld.EntityManager;
            }

            return _cachedEntityMng;
        }
    }
    private CharacterBackground[] _defaultBackgrounds;

    private bool IsPlayerEntityHasCustomizeComponent() {
        return _EntityMng.HasComponent<CustomizeComponent>(_playerEntity);
    }


    void Start() {
        decision.SetActive(false);

        var playerEntities = _EntityMng.GetAllEntities()
            .Where(entity => _EntityMng.HasComponent(entity, typeof(PlayerComponent)));

        foreach (var entity in playerEntities) {
            _playerEntity = entity;
            break;
        }

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


    public void OnSelectedInBackground() {
        foreach (var toggle in backgroundGroup.ActiveToggles()) {
            backgroundText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.backgroundType = Utility.ToEnum<BackgroundType>(toggle.name);
            _EntityMng.SetComponentData(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInValues() {
        foreach (var toggle in valuesGroup.ActiveToggles()) {
            valuesText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.valuesType = Utility.ToEnum<ValuesType>(toggle.name);
            _EntityMng.SetComponentData(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInGoal() {
        foreach (var toggle in goalGroup.ActiveToggles()) {
            goalText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.goalType = Utility.ToEnum<GoalType>(toggle.name);
            _EntityMng.SetComponentData(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInDecision() {
        decision.SetActive(true);
    }


    public void OnSelectedInConfirm() {
        decision.SetActive(false);
        AdjustPlayerStatus();

        _EntityMng.RemoveComponent<CustomizeComponent>(_playerEntity);
        _EntityMng.AddComponentData(_playerEntity, new CustomizeCompleteComponent());
    }


    public void OnSelectedInCancel() {
        decision.SetActive(false);
    }

    
    public void OnSelectedInMentality(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.mentality + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.mentality = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        mentalityText.text = customizeComp.mentality.ToString();
    }


    public void OnSelectedInAgility(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.agility + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.agility = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        agilityText.text = customizeComp.agility.ToString();
    }


    public void OnSelectedInPhysical(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.physical + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.physical = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        physicalText.text = customizeComp.physical.ToString();
    }


    public void OnSelectedInSearch(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.search + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.search = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        searchText.text = customizeComp.search.ToString();
    }


    public void OnSelectedInLuck(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.luck + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.luck = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        luckText.text = customizeComp.luck.ToString();
    }


    public void AdjustPlayerStatus() {
        // the player must have these components
        var playerStatusComp = _EntityMng.GetComponentData<PlayerStatusComponent>(_playerEntity);
        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);

        playerStatusComp.status = _defaultBackgrounds[(int)customizeComp.backgroundType];
        playerStatusComp.moveSpeed = playerStatusComp.status.agility * 0.01f;
        playerStatusComp.madnessWeight = playerStatusComp.status.mentality * 0.01f;
        playerStatusComp.searchingWeight = playerStatusComp.status.search * 0.01f;

        var playerPresetComp = _EntityMng.GetSharedComponentData<PlayerPresetComponent>(_playerEntity);
        string prefabPath;
        playerPresetComp.preset.prefabPaths.TryGetValue(customizeComp.backgroundType, out prefabPath);
        AdjustPlayerSprite(ref prefabPath, ref playerPresetComp.sprite);

        _EntityMng.SetComponentData<PlayerStatusComponent>(_playerEntity, playerStatusComp);
    }


    public void AdjustPlayerSprite(ref string prefabPath, ref Sprite sprite) {
        var preset = Utility.LoadObjectAtPath<GameObject>(prefabPath);
        var spritePreset = preset.GetComponent<SpritePreset>();
        if (ReferenceEquals(null, spritePreset)) {
            return;
        }

        var localScale = GetComponent<Transform>().localScale;
        var spriteScale = new float3(sprite.rect.width, sprite.rect.height, 1.0f) / sprite.pixelsPerUnit;
        spriteScale *= localScale;
        spriteScale.z = 1.0f;
        _EntityMng.AddComponentData(_playerEntity, new NonUniformScale() {
            Value = spriteScale
        });

        var applyPivot = (sprite.rect.center - sprite.pivot) / sprite.pixelsPerUnit * localScale;
        _EntityMng.AddComponentData(_playerEntity, new SpritePivotComponent() {
            Value = new float3(applyPivot, 0.0f)
        });

        if (_EntityMng.HasComponent<SpritePresetComponent>(_playerEntity)) {
            var spiretPresetComp = _EntityMng.GetSharedComponentData<SpritePresetComponent>(_playerEntity);
            spiretPresetComp.preset = spritePreset;
        }
        else {
            _EntityMng.AddSharedComponentData(_playerEntity, new SpritePresetComponent() {
                preset = spritePreset
            });
        }
        
        _EntityMng.AddComponentData(_playerEntity, new SpriteStateComponent() {
            hash = spritePreset.datas.Keys.First()
        });
    }
}
