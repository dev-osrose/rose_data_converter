using System.Collections.Generic;
using Revise.STB;
using Revise.STL;

namespace rose_data.Data
{
    public class Skill
    {
        #region Structs

        public struct UseAbility
        {
            public UseAbility(int type, int value) : this()
            {
                Set(type, value);
            }

            public void Set(int type, int value)
            {
                this.Type = type;
                this.Value = value;
            }

            public int Type { get; set; }
            public int Value { get; set; }
        }

        public struct IncreaseAbility
        {
            public IncreaseAbility(int type, int value, int rate) : this()
            {
                Set(type, value, rate);
            }

            public void Set(int type, int value, int rate)
            {
                this.Type = type;
                this.Value = value;
                this.Rate = rate;
            }

            public int Type { get; set; }
            public int Value { get; set; }
            public int Rate { get; set; }
        }

        #endregion

        #region Private Fields

        private int _id;
        private string _skillName;
        private string _skillDesc;
        private int _level;
        private SkillType _type;
        private int _range;
        private int _class;
        private int _power;
        private int _pointsToLevelUp;
        private int _zulyNeededToLevelUp;
        private int _usageAttribute;
        private int _cooldown;
        private int _successRate;
        private int _duration;
        private int _damageType;
        private int _warpZoneNumber;
        private int _warpZoneXPosition;
        private int _warpZoneYPosition;
        private int _reloadType;
        private int _summonPet;
        private int _actionMode;
        private int _iconNumber;
        private int _animationCasting;
        private int _animationCastingSpeed;
        private int _animationActionType;
        private int _animationActionSpeed;
        private int _animationHitCount;
        private int _availableClassSet;
        private int _repeatAnimationCasting;
        private int _repeatAnimationCastingCount;

        #endregion

        #region Enums

        public enum SkillType
        {
            InvalidSkill = 0,
            BaseAction = 1,
            CreateWindow = 2,
            Immediate = 3,
            EnforceWeapon = 4,
            EnforceBullet = 5,
            FireBullet = 6,
            AreaTarget = 7,
            SelfBoundDuration = 8,
            TargetBoundDuration = 9,
            SelfBound = 10,
            TargetBound = 11,
            SelfStateDuration = 12,
            TargetStateDuration = 13,
            SummonPet = 14,
            PassiveSkill = 15,
            Emote = 16,
            SelfDamage = 17,
            Warp = 18,
            SelfAndTarget = 19,
            Resurrect = 20,
            MaxSkillTypes
        };

        public enum TargetFilter
        {
            Self = 0,
            Group,
            Guild,
            AllFriendly,
            Mob,
            AllEnemies,
            PcEnemies,
            AllPCs,
            AllCharacters,
            DeadUsers,
            EnemyMob,
            MaxTargetTypes
        };

        public enum ActionModes
        {
            Stop,
            Attack,
            Restore
        }

        #endregion

        #region Properties

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _skillName;
            set => _skillName = value;
        }

        public string Description
        {
            get => _skillDesc;
            set => _skillDesc = value;
        }

        public SkillType Type
        {
            get => _type;
            set => _type = value;
        }

        public int Level
        {
            get => _level;
            set => _level = value;
        }

        public int Range
        {
            get => _range;
            set => _range = value;
        }

        public int Class
        {
            get => _class;
            set => _class = value;
        }

        public int Power
        {
            get => _power;
            set => _power = value;
        }
        
        public List<UseAbility> StatsRequiredToUse { get; set; } = new List<UseAbility>();

        public List<IncreaseAbility> StatsToBuff { get; set; } = new List<IncreaseAbility>();

        public int Cooldown
        {
            get => _cooldown;
            set => _cooldown = value;
        }

        public int PointsToLevelUp
        {
            get => _pointsToLevelUp;
            set => _pointsToLevelUp = value;
        }

        public int ZulyNeededToLevelUp
        {
            get => _zulyNeededToLevelUp;
            set => _zulyNeededToLevelUp = value;
        }

        public int RideStateToUse
        {
            get => _usageAttribute;
            set => _usageAttribute = value;
        }

        public int SuccessRate
        {
            get => _successRate;
            set => _successRate = value;
        }

        public int Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public int DamageType
        {
            get => _damageType;
            set => _damageType = value;
        }

        public int WarpZoneNumber
        {
            get => _warpZoneNumber;
            set => _warpZoneNumber = value;
        }

        public int WarpZoneXPosition
        {
            get => _warpZoneXPosition;
            set => _warpZoneXPosition = value;
        }

        public int WarpZoneYPosition
        {
            get => _warpZoneYPosition;
            set => _warpZoneYPosition = value;
        }

        public int ReloadType
        {
            get => _reloadType;
            set => _reloadType = value;
        }

        public int SummonPet
        {
            get => _summonPet;
            set => _summonPet = value;
        }

        public int ActionMode
        {
            get => _actionMode;
            set => _actionMode = value;
        }
        
        public List<int> RequiredWeaponTypes { get; set; } = new List<int>();// this maps to the Item.SubType from the item stb

        public int IconNumber
        {
            get => _iconNumber;
            set => _iconNumber = value;
        }

        public int CastAnimation
        {
            get => _animationCasting;
            set => _animationCasting = value;
        }

        public int AnimationCastSpeed
        {
            get => _animationCastingSpeed;
            set => _animationCastingSpeed = value;
        }

        public int AnimationActionType
        {
            get => _animationActionType;
            set => _animationActionType = value;
        }

        public int AnimationActionSpeed
        {
            get => _animationActionSpeed;
            set => _animationActionSpeed = value;
        }

        public int AnimationHitCount
        {
            get => _animationHitCount;
            set => _animationHitCount = value;
        }

        public int RepeatCastingAnimation
        {
            get => _repeatAnimationCasting;
            set => _repeatAnimationCasting = value;
        }

        public int RepeatCastingAnimationCount
        {
            get => _repeatAnimationCastingCount;
            set => _repeatAnimationCastingCount = value;
        }

        public int AvailableClassSet
        {
            get => _availableClassSet;
            set => _availableClassSet = value;
        }
        
        public List<int> AvailableUnions { get; set; } = new List<int>(); // this maps to the Item.SubType from the item stb

        #endregion

        public Skill(int id)
        {
            Id = id;
        }

        public void Load(DataRow row, StringTableRow strTableRow)
        {
            _skillName = strTableRow.GetText();
            _skillDesc = strTableRow.GetDescription();

            int.TryParse(row[3], out _level);
            int.TryParse(row[4], out _pointsToLevelUp);
            int.TryParse(row[6], out var type);
            _type = (SkillType) type;

            int.TryParse(row[7], out _range);
            int.TryParse(row[8], out _class);
            int.TryParse(row[10], out _power);
            int.TryParse(row[14], out _successRate);
            int.TryParse(row[15], out _duration);
            int.TryParse(row[16], out _damageType);
            
            for (var useIndex = 0; useIndex < 2; useIndex++)
            {
                int.TryParse(row[(17 + (useIndex * 2))], out var reqType);
                int.TryParse(row[(18 + (useIndex * 2))], out var reqValue);
                
                if (reqType == 0) continue;
                
                StatsRequiredToUse.Add(new UseAbility(reqType, reqValue));
            }
            
            for (var abilityIndex = 0; abilityIndex < 2; abilityIndex++)
            {
                int.TryParse(row[(22 + (abilityIndex * 3))], out var incAbility);
                int.TryParse(row[(23 + (abilityIndex * 3))], out var incValue);
                int.TryParse(row[(24 + (abilityIndex * 3))], out var incRate);
                
                if (incAbility == 0) continue;
                
                StatsToBuff.Add(new IncreaseAbility(incAbility, incValue, incRate));
            }

            int.TryParse(row[21], out _cooldown);

            int.TryParse(row[22], out _warpZoneNumber);
            int.TryParse(row[23], out _warpZoneXPosition);
            int.TryParse(row[24], out _warpZoneYPosition);

            int.TryParse(row[28], out _reloadType);
            int.TryParse(row[29], out _summonPet);
            int.TryParse(row[30], out _actionMode);
            
            for (var needWeaponIndex = 0; needWeaponIndex < 5; needWeaponIndex++)
            {
                int.TryParse(row[(31 + needWeaponIndex)], out var weapon);

                if (weapon == 0) continue;
                RequiredWeaponTypes.Add(weapon);
            }
            
            int.TryParse(row[36], out _availableClassSet); // This has something to do with LIST_CLASS.stb
            
            for (var availableUnionIndex = 0; availableUnionIndex < 3; availableUnionIndex++)
            {
                int.TryParse(row[(37 + availableUnionIndex)], out var availableUnion);
                
                if (availableUnion == 0) continue;
                AvailableUnions.Add(availableUnion);
            }
            
            for (var needSkillIndex = 0; needSkillIndex < 3; needSkillIndex++)
            {
                int.TryParse(row[(40 + (needSkillIndex * 2))], out var skillIndex);
                int.TryParse(row[(41 + (needSkillIndex * 2))], out var skillLevel);
                
                if (skillIndex == 0) continue;
                // RequiredLearnedSkills[needSkillIndex].Set(skillIndex, skillLevel);
            }
            
            for (var needAbilityIndex = 0; needAbilityIndex < 3; needAbilityIndex++)
            {
                int.TryParse(row[(46 + (needAbilityIndex * 2))], out var abilityType);
                int.TryParse(row[(47 + (needAbilityIndex * 2))], out var abilityValue);
                
                if (abilityType == 0) continue;
                // RequiredStats[needAbilityIndex].Set(abilityType, abilityValue);
            }

            int.TryParse(row[52], out _iconNumber);
            int.TryParse(row[53], out _animationCasting);
            int.TryParse(row[54], out _animationCastingSpeed);
            
            int.TryParse(row[55], out _repeatAnimationCasting);
            int.TryParse(row[56], out _repeatAnimationCastingCount);
            
            for (var castingEffectIndex = 0; castingEffectIndex < 2; castingEffectIndex++)
            {
                int.TryParse(row[(57 + (castingEffectIndex * 3))], out var effect);
                int.TryParse(row[(58 + (castingEffectIndex * 3))], out var effectPoint);
                int.TryParse(row[(59 + (castingEffectIndex * 3))], out var sound);
                
                if (effect == 0) continue;
                // CastingEffects[castingEffectIndex].Set(effect, effectPoint, sound);
            }

            int.TryParse(row[69], out _animationActionType);
            int.TryParse(row[70], out _animationActionSpeed);
            int.TryParse(row[71], out _animationHitCount);

            int.TryParse(row[86], out _zulyNeededToLevelUp);
            int.TryParse(row[87], out _usageAttribute);
        }
    }
}