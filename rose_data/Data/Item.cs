using System;
using System.Collections.Generic;
using Revise.STB;
using Revise.STL;

namespace rose_data.Data
{
    public class Item
    {
        #region Structs

        public struct Requirement
        {
            public Requirement(int type, int value) : this()
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

        public struct Bonus
        {
            public Bonus(int type, int value) : this()
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

        #endregion

        #region Private Fields

        private int _basePrice;
        private double _priceRate;
        private int _weight;
        private int _quality;
        private int _iconNumber;
        private int _fieldModel;
        private int _equipableJobs;
        private int _durability;
        private int _rarityType;
        private int _defense;
        private int _resistances;
        private int _movementSpeed;
        private int _jemSlots;
        private int _jemPosition;
        private int _range;
        private int _attack;
        private int _attackSpeed;
        private int _isMagic;
        private int _equipSound;
        private int _makeNumber;
        private int _skillLevel;
        private int _craftingId;
        private int _craftDifficulty;
        private int _itemRarityRate;
        private int _hairType;

        // start ItemType.Weapons
        private int _bulletEffect;
        private int _defaultEffect;
        private int _attackStartSound;
        private int _attackFireSound;
        private int _attackHitSound;

        private int _gemPosition;
        // end ItemType.Weapons

        // start ItemType.Consumables
        private int _storeSkin;
        private int _needStatType;
        private int _needStatValue;
        private int _addStatType;
        private int _addStatValue;
        private int _learnSkill;
        private int _useSkill;
        private int _useEffect;
        private int _useSound;
        private int _useCooldown;
        private int _delayTimeType;

        private int _delayTimeTick;
        // end ItemType.Consumables

        // start ItemType.Gems
        private int _gemIcon;
        private int _gemAttackEffect;

        // end ItemType.Gems

        // start ItemType.Natural
        private int _bulletId;
        // end ItemType.Natural

        #endregion

        #region Enums

        public enum ItemType
        {
            Invalid = 0,
            Face = 1,
            Helmet = 2,
            Armor = 3,
            Gloves = 4,
            Boots = 5,
            Backpack = 6,
            Jewels = 7,
            Weapons = 8,
            SubWeapons = 9,
            Consumables = 10,
            Etc = 11,
            Gems = Etc,
            Natural = 12,
            Quest = 13,
            MountParts = 14,
            MaxItemTypes
        }

        public enum ItemUsageRestrictions
        {
            None,
            NoSell,
            NoDropOrTrade,
            NoSellDropOrTrade,
            AllowBankStorage,
            MaxItemRestrictions
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public ItemType Type { get; set; }

        public int SubType { get; set; }

        public ItemUsageRestrictions Usage { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int BasePrice
        {
            get => _basePrice;
            set => _basePrice = value;
        }

        public double PriceRate
        {
            get => _priceRate;
            set => _priceRate = value;
        }

        public int Weight
        {
            get => _weight;
            set => _weight = value;
        }

        public int Quality
        {
            get => _quality;
            set => _quality = value;
        }

        public int IconNumber
        {
            get => _iconNumber;
            set => _iconNumber = value;
        }

        public int FieldModel
        {
            get => _fieldModel;
            set => _fieldModel = value;
        }

        public int EquipableJobs
        {
            get => _equipableJobs;
            set => _equipableJobs = value;
        }

        public List<Requirement> Requirements { get; set; }
        public List<Bonus> Bonuses { get; set; }
        public List<Bonus> GemBonuses { get; set; }

        public int Durability
        {
            get => _durability;
            set => _durability = value;
        }

        public int RarityType
        {
            get => _rarityType;
            set => _rarityType = value;
        }

        public int Defense
        {
            get => _defense;
            set => _defense = value;
        }

        public int Resistances
        {
            get => _resistances;
            set => _resistances = value;
        }

        public int MovementSpeed
        {
            get => _movementSpeed;
            set => _movementSpeed = value;
        }

        public int JemSlots
        {
            get => _jemSlots;
            set => _jemSlots = value;
        }

        public int JemPosition
        {
            get => _jemPosition;
            set => _jemPosition = value;
        }

        public int @Range
        {
            get => _range;
            set => _range = value;
        }

        public int Attack
        {
            get => _attack;
            set => _attack = value;
        }

        public int AttackSpeed
        {
            get => _attackSpeed;
            set => _attackSpeed = value;
        }

        public int IsMagic
        {
            get => _isMagic;
            set => _isMagic = value;
        }

        public List<int> RequiredUnions { get; set; }

        public int EquipSound
        {
            get => _equipSound;
            set => _equipSound = value;
        }

        public int MakeNumber
        {
            get => _makeNumber;
            set => _makeNumber = value;
        }

        public int SkillLevel
        {
            get => _skillLevel;
            set => _skillLevel = value;
        }

        public int CraftingId
        {
            get => _craftingId;
            set => _craftingId = value;
        }

        public int CraftingDifficulty
        {
            get => _craftDifficulty;
            set => _craftDifficulty = value;
        }

        public int UnionPointTradeValue
        {
            get => _craftDifficulty;
            set => _craftDifficulty = value;
        }

        public int ItemRarityRate
        {
            get => _itemRarityRate;
            set => _itemRarityRate = value;
        }

        public int HairType
        {
            get => _hairType;
            set => _hairType = value;
        }

        public int BulletEffect
        {
            get => _bulletEffect;
            set => _bulletEffect = value;
        }

        public int DefaultEffect
        {
            get => _defaultEffect;
            set => _defaultEffect = value;
        }

        public int AttackStartSound
        {
            get => _attackStartSound;
            set => _attackStartSound = value;
        }

        public int AttackFireSound
        {
            get => _attackFireSound;
            set => _attackFireSound = value;
        }

        public int AttackHitSound
        {
            get => _attackHitSound;
            set => _attackHitSound = value;
        }

        public int GemPosition
        {
            get => _gemPosition;
            set => _gemPosition = value;
        }

        public int StoreSkin
        {
            get => _storeSkin;
            set => _storeSkin = value;
        }

        public int NeedStatType
        {
            get => _needStatType;
            set => _needStatType = value;
        }

        public int NeedStatValue
        {
            get => _needStatValue;
            set => _needStatValue = value;
        }

        public int AddStatType
        {
            get => _addStatType;
            set => _addStatType = value;
        }

        public int AddStatValue
        {
            get => _addStatValue;
            set => _addStatValue = value;
        }

        public int LearnSkill
        {
            get => _learnSkill;
            set => _learnSkill = value;
        }

        public int UseSkill
        {
            get => _useSkill;
            set => _useSkill = value;
        }

        public string Script { get; private set; }

        public int UseEffect
        {
            get => _useEffect;
            set => _useEffect = value;
        }

        public int UseSound
        {
            get => _useSound;
            set => _useSound = value;
        }

        public int UseCooldown
        {
            get => _useCooldown;
            set => _useCooldown = value;
        }

        public int DelayTimeType
        {
            get => _delayTimeType;
            set => _delayTimeType = value;
        }

        public int DelayTimeTick
        {
            get => _delayTimeTick;
            set => _delayTimeTick = value;
        }

        public int GemIcon
        {
            get => _gemIcon;
            set => _gemIcon = value;
        }

        public int GemAttackEffect
        {
            get => _gemAttackEffect;
            set => _gemAttackEffect = value;
        }

        public int BulletId
        {
            get => _bulletId;
            set => _bulletId = value;
        }

        #endregion

        public Item(int id, int type)
        {
            Id = id;
            Type = (ItemType) type;
        }

        public void Load(DataRow row, StringTableRow strTableRow)
        {
            Name = strTableRow.GetText();
            Description = strTableRow.GetDescription();

            int.TryParse(row[4], out var usage);
            int.TryParse(row[5], out var type);
            Usage = (ItemUsageRestrictions) usage;
            SubType = type;

            int.TryParse(row[6], out _basePrice);
            double.TryParse(row[7], out _priceRate);
            int.TryParse(row[8], out _weight);
            int.TryParse(row[9], out _quality);
            int.TryParse(row[10], out _iconNumber);
            int.TryParse(row[11], out _fieldModel);

            int.TryParse(row[12], out _equipSound);
            int.TryParse(row[13], out _makeNumber);
            int.TryParse(row[14], out _skillLevel);
            int.TryParse(row[15], out _craftingId);
            int.TryParse(row[16], out _craftDifficulty);

            switch (Type)
            {
                case ItemType.Consumables:
                case ItemType.Gems:
                case ItemType.Jewels:
                case ItemType.Natural:
                case ItemType.Quest:
                    int.TryParse(row[17], out _itemRarityRate);
                    break;
                case ItemType.Armor:
                case ItemType.Backpack:
                case ItemType.Boots:
                case ItemType.Face:
                case ItemType.Gloves:
                case ItemType.Helmet:
                case ItemType.MountParts:
                case ItemType.SubWeapons:
                case ItemType.Weapons:
                {
                    int.TryParse(row[17], out _equipableJobs);

                    RequiredUnions = new List<int>();
                    for (var requireUnionIndex = 0; requireUnionIndex < 2; requireUnionIndex++)
                    {
                        int.TryParse(row[(18 + requireUnionIndex)], out var requiredUnion);

                        if (requiredUnion == 0) continue;
                        RequiredUnions.Add(requiredUnion);
                    }

                    Requirements = new List<Requirement>();
                    for (var reqIndex = 0; reqIndex < 2; reqIndex++)
                    {
                        int.TryParse(row[(20 + (reqIndex * 2))], out var reqType);
                        int.TryParse(row[(21 + (reqIndex * 2))], out var reqValue);

                        if (reqType == 0) continue;
                        Requirements.Add(new Requirement(reqType, reqValue));
                    }

                    Bonuses = new List<Bonus>();
                    for (var bonusIndex = 0; bonusIndex < 2; bonusIndex++)
                    {
                        int.TryParse(row[(25 + (bonusIndex * 3))], out var bonusType);
                        int.TryParse(row[(26 + (bonusIndex * 3))], out var bonusValue);

                        if (bonusType == 0) continue;
                        Bonuses.Add(new Bonus(bonusType, bonusValue));
                    }

                    int.TryParse(row[30], out _durability);
                    int.TryParse(row[31], out _rarityType);
                    int.TryParse(row[32], out _defense);
                    int.TryParse(row[33], out _resistances);
                    break;
                }
            }

            try
            {
                switch (Type)
                {
                    case ItemType.Backpack:
                    case ItemType.Boots:
                    {
                        int.TryParse(row[34], out _movementSpeed);
                        break;
                    }
                    case ItemType.Weapons:
                    {
                        int.TryParse(row[31], out _jemSlots);
                        int.TryParse(row[34], out _range);
                        int.TryParse(row[36], out _attack);
                        int.TryParse(row[37], out _attackSpeed);
                        int.TryParse(row[38], out _isMagic);

                        int.TryParse(row[39], out _bulletEffect);
                        int.TryParse(row[40], out _defaultEffect);
                        int.TryParse(row[41], out _attackStartSound);
                        int.TryParse(row[42], out _attackFireSound);
                        int.TryParse(row[43], out _attackHitSound);
                        int.TryParse(row[44], out _gemPosition);
                        break;
                    }
                    case ItemType.SubWeapons:
                    {
                        int.TryParse(row[31], out _jemSlots);
                        int.TryParse(row[35], out _jemPosition);
                        break;
                    }
                    case ItemType.Helmet:
                    {
                        int.TryParse(row[34], out _hairType);
                        break;
                    }
                    case ItemType.Consumables:
                    {
                        int.TryParse(row[9], out _storeSkin);
                        int.TryParse(row[18], out _needStatType);
                        int.TryParse(row[19], out _needStatValue);
                        int.TryParse(row[20], out _addStatType);
                        int.TryParse(row[21], out _addStatValue);
                        int.TryParse(row[20], out _learnSkill);
                        int.TryParse(row[21], out _useSkill);

                        // int.TryParse(row[22], out _script);
                        Script = row[22];

                        if (SubType == 316)
                        {
                            int.TryParse(row[23], out var conFileIdx); // Need to load con file and export it now?}
                        }
                        else
                            int.TryParse(row[23], out _useEffect);

                        int.TryParse(row[24], out _useSound);
                        int.TryParse(row[25], out _useCooldown);
                        int.TryParse(row[26], out _delayTimeType);
                        int.TryParse(row[27], out _delayTimeTick);

                        break;
                    }
                    case ItemType.Gems:
                    {
                        GemBonuses = new List<Bonus>();
                        for (var bonusIndex = 0; bonusIndex < 2; bonusIndex++)
                        {
                            int.TryParse(row[(17 + (bonusIndex * 2))], out var bonusType);
                            int.TryParse(row[(18 + (bonusIndex * 2))], out var bonusValue);
                            
                            if (bonusType == 0) continue;
                            GemBonuses.Add(new Bonus(bonusType, bonusValue));
                        }

                        int.TryParse(row[21], out _gemIcon);
                        int.TryParse(row[22], out _gemAttackEffect);
                        break;
                    }
                    case ItemType.Natural:
                    {
                        int.TryParse(row[18], out _bulletId);
                        break;
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}