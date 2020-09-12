using System.Globalization;
using Revise.STB;
using Revise.STL;

namespace rose_data.Data
{
    public class Skill
    {
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
        private int _animation;
        private int _animationSpeed;
        private int _animationActionType;
        private int _animationActionSpeed;
        private int _animationHitCount;

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

        public int UsageAttribute
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

        public int IconNumber
        {
            get => _iconNumber;
            set => _iconNumber = value;
        }

        public int Animation
        {
            get => _animation;
            set => _animation = value;
        }

        public int AnimationSpeed
        {
            get => _animationSpeed;
            set => _animationSpeed = value;
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

        #endregion
        
        public Skill(int id)
        {
            Id = id;
        }

        public void Load(DataRow row, StringTableRow strTableRow)
        {
            _skillName = strTableRow.GetText();
            _skillDesc = strTableRow.GetDescription();
            int type = 0;
            
            int.TryParse(row[3], out _level);
            int.TryParse(row[4], out _pointsToLevelUp);
            int.TryParse(row[6], out type);
            _type = (SkillType) type;
            
            int.TryParse(row[7], out _range);
            int.TryParse(row[8], out _class);
            int.TryParse(row[10], out _power);
            int.TryParse(row[14], out _successRate);
            int.TryParse(row[15], out _duration);
            int.TryParse(row[16], out _damageType);
            int.TryParse(row[21], out _cooldown);
            
            int.TryParse(row[22], out _warpZoneNumber);
            int.TryParse(row[23], out _warpZoneXPosition);
            int.TryParse(row[24], out _warpZoneYPosition);
            
            int.TryParse(row[28], out _reloadType);
            int.TryParse(row[29], out _summonPet);
            int.TryParse(row[30], out _actionMode);
            
            int.TryParse(row[52], out _iconNumber);
            int.TryParse(row[53], out _animation);
            int.TryParse(row[54], out _animationSpeed);
            
            int.TryParse(row[69], out _animationActionType);
            int.TryParse(row[70], out _animationActionSpeed);
            int.TryParse(row[71], out _animationHitCount);
            
            int.TryParse(row[86], out _zulyNeededToLevelUp);
            int.TryParse(row[87], out _usageAttribute);
        }
    }
}