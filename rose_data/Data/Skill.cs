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

        #endregion

        #region Enums

        public enum SkillType : int
        {
            INVALID_SKILL = 0,
            BASE_ACTION = 1,
            CREATE_WINDOW = 2,
            IMMEDIATE = 3,
            ENFORCE_WEAPON = 4,
            ENFORCE_BULLET = 5,
            FIRE_BULLET = 6,
            AREA_TARGET = 7,
            SELF_BOUND_DURATION = 8,
            TARGET_BOUND_DURATION = 9,
            SELF_BOUND = 10,
            TARGET_BOUND = 11,
            SELF_STATE_DURATION = 12,
            TARGET_STATE_DURATION = 13,
            SUMMON_PET = 14,
            PASSIVE_SKILL = 15,
            EMOTE = 16,
            SELF_DAMAGE = 17,
            WARP = 18,
            SELF_AND_TARGET = 19,
            RESURRECT = 20,
            MAX_SKILL_TYPES
        };

        public enum TargetFilter : int
        {
            SELF = 0,
            GROUP,
            GUILD,
            ALL_FRIENDLY,
            MOB,
            ALL_ENEMIES,
            PC_ENEMIES,
            ALL_PCs,
            ALL_CHARACTERS,
            DEAD_USERS,
            ENEMY_MOB,
            MAX_TARGET_TYPES
        };

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
            int.TryParse(row[7], out _range);
            int.TryParse(row[8], out _class);
            int.TryParse(row[10], out _power);
            int.TryParse(row[21], out _cooldown);
            int.TryParse(row[86], out _zulyNeededToLevelUp);
            int.TryParse(row[87], out _usageAttribute);
            _type = (SkillType) type;
        }
    }
}