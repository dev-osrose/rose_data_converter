using System;
using System.Globalization;
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

            public int Value { get; set; }

            public int Type { get; set; }
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

            public int Value { get; set; }

            public int Type { get; set; }
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

        public ItemType SubType { get; set; }

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

        public Requirement[] Requirements { get; set; } = new Requirement[2];

        public Bonus[] Bonuses { get; set; } = new Bonus[2];

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

        #endregion

        public Item(int id, int type)
        {
            Id = id;
            SubType = (ItemType) type;
        }

        public void Load(DataRow row, StringTableRow strTableRow)
        {
            Name = strTableRow.GetText();
            Description = strTableRow.GetDescription();

            int usage = 0, type = 0;
            int.TryParse(row[4], out usage);
            int.TryParse(row[5], out type);
            Usage = (ItemUsageRestrictions) usage;
            SubType = (ItemType) type;

            int.TryParse(row[6], out _basePrice);
            double.TryParse(row[7], out _priceRate);
            int.TryParse(row[8], out _weight);
            int.TryParse(row[9], out _quality);
            int.TryParse(row[10], out _iconNumber);
            int.TryParse(row[11], out _fieldModel);
            int.TryParse(row[17], out _equipableJobs);

            try
            {
                for (var reqIndex = 0; reqIndex < 2; reqIndex++)
                {
                    int reqType = 0, reqValue = 0;
                    int.TryParse(row[(20 + (reqIndex * 2))], out reqType);
                    int.TryParse(row[(21 + (reqIndex * 2))], out reqValue);
                    
                    Requirements[reqIndex].Set(reqType, reqValue);
                }

                for (var bonusIndex = 0; bonusIndex < 2; bonusIndex++)
                {
                    int bonusType = 0, bonusValue = 0;
                    int.TryParse(row[(25 + (bonusIndex * 3))], out bonusType);
                    int.TryParse(row[(26 + (bonusIndex * 3))], out bonusValue);
                    
                    Bonuses[bonusIndex].Set(bonusType, bonusValue);
                }

                int.TryParse(row[30], out _durability);
                int.TryParse(row[31], out _rarityType);
                int.TryParse(row[32], out _defense);
                int.TryParse(row[33], out _resistances);

                switch (SubType)
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
                        break;
                    }
                    case ItemType.SubWeapons:
                    {
                        int.TryParse(row[31], out _jemSlots);
                        int.TryParse(row[35], out _jemPosition);
                        break;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
    }
}