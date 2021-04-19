using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Revise.STB;
using Revise.STL;
using Revise.CON;

namespace rose_data.Data
{
    public class Npc
    {
        private string _name;
        private string _desc;
        private int _walkSpeed;
        private int _runSpeed;
        private int _scale;
        private int _rightWeapon;
        private int _leftWeapon;
        private int _level;
        private int _hp;
        private int _attack;
        private int _hit;
        private int _def;
        private int _res;
        private int _avoid;
        private int _attackSpeed;
        private int _isMagicDamage;
        private int _aiType;
        private int _giveExp;
        private int _type;
        private int _markNumber;
        private int _unionNumber;
        private int _sellTab0;
        private int _sellTab1;
        private int _sellTab2;
        private int _sellTab3;
        private int _faceIcon;
        private int _dropType;
        private int _dropMoney;
        private int _dropItem;
        private int _needSummonCount;
        private int _canTarget;
        private int _attackRange;
        private int _hitMaterialType;
        private int _summonMobType;
        private int _normalEffectSound;
        private int _attackSound;
        private int _hitSound;
        private int _handHitEffect;
        private int _deadEffect;
        private int _dieSound;
        private int _questType;
        
        private int _glowColor;
        private int _height;
        private int _createEffect;
        private int _createSound;
        
        public int Id { get; set; }
        
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Desc
        {
            get => _desc;
            set => _desc = value;
        }

        public int WalkSpeed
        {
            get => _walkSpeed;
            set => _walkSpeed = value;
        }

        public int RunSpeed
        {
            get => _runSpeed;
            set => _runSpeed = value;
        }

        public int Scale
        {
            get => _scale;
            set => _scale = value;
        }

        public int RightWeapon
        {
            get => _rightWeapon;
            set => _rightWeapon = value;
        }

        public int LeftWeapon
        {
            get => _leftWeapon;
            set => _leftWeapon = value;
        }

        public int Level
        {
            get => _level;
            set => _level = value;
        }

        public int Hp
        {
            get => _hp;
            set => _hp = value;
        }

        public int Attack
        {
            get => _attack;
            set => _attack = value;
        }

        public int Hit
        {
            get => _hit;
            set => _hit = value;
        }

        public int Def
        {
            get => _def;
            set => _def = value;
        }

        public int Res
        {
            get => _res;
            set => _res = value;
        }

        public int Avoid
        {
            get => _avoid;
            set => _avoid = value;
        }

        public int AttackSpeed
        {
            get => _attackSpeed;
            set => _attackSpeed = value;
        }

        public int IsMagicDamage
        {
            get => _isMagicDamage;
            set => _isMagicDamage = value;
        }

        public int AiType
        {
            get => _aiType;
            set => _aiType = value;
        }

        public int GiveExp
        {
            get => _giveExp;
            set => _giveExp = value;
        }

        public int Type
        {
            get => _type;
            set => _type = value;
        }

        public int MarkNumber
        {
            get => _markNumber;
            set => _markNumber = value;
        }

        public int UnionNumber
        {
            get => _unionNumber;
            set => _unionNumber = value;
        }

        public int SellTab0
        {
            get => _sellTab0;
            set => _sellTab0 = value;
        }

        public int SellTab1
        {
            get => _sellTab1;
            set => _sellTab1 = value;
        }

        public int SellTab2
        {
            get => _sellTab2;
            set => _sellTab2 = value;
        }

        public int SellTab3
        {
            get => _sellTab3;
            set => _sellTab3 = value;
        }

        public int FaceIcon
        {
            get => _faceIcon;
            set => _faceIcon = value;
        }

        public int DropType
        {
            get => _dropType;
            set => _dropType = value;
        }

        public int DropMoney
        {
            get => _dropMoney;
            set => _dropMoney = value;
        }

        public int DropItem
        {
            get => _dropItem;
            set => _dropItem = value;
        }

        public int NeedSummonCount
        {
            get => _needSummonCount;
            set => _needSummonCount = value;
        }

        public int CanTarget
        {
            get => _canTarget;
            set => _canTarget = value;
        }

        public int AttackRange
        {
            get => _attackRange;
            set => _attackRange = value;
        }

        public int HitMaterialType
        {
            get => _hitMaterialType;
            set => _hitMaterialType = value;
        }

        public int SummonMobType
        {
            get => _summonMobType;
            set => _summonMobType = value;
        }

        public int NormalEffectSound
        {
            get => _normalEffectSound;
            set => _normalEffectSound = value;
        }

        public int AttackSound
        {
            get => _attackSound;
            set => _attackSound = value;
        }

        public int HitSound
        {
            get => _hitSound;
            set => _hitSound = value;
        }

        public int HandHitEffect
        {
            get => _handHitEffect;
            set => _handHitEffect = value;
        }

        public int DeadEffect
        {
            get => _deadEffect;
            set => _deadEffect = value;
        }

        public int DieSound
        {
            get => _dieSound;
            set => _dieSound = value;
        }

        public int QuestType
        {
            get => _questType;
            set => _questType = value;
        }

        public int GlowColor
        {
            get => _glowColor;
            set => _glowColor = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public int CreateEffect
        {
            get => _createEffect;
            set => _createEffect = value;
        }

        public int CreateSound
        {
            get => _createSound;
            set => _createSound = value;
        }

        private const int NpcType = 999;
        
        public Npc(int id)
        {
            Id = id;
        }

        public void Load(DataRow row, StringTableRow strTableRow)
        {
            _name = strTableRow.GetText();
            _desc = row[41];
            
            int.TryParse(row[03], out _walkSpeed);
            int.TryParse(row[04], out _runSpeed);
            int.TryParse(row[05], out _scale);
            int.TryParse(row[06], out _rightWeapon);
            int.TryParse(row[07], out _leftWeapon);
            int.TryParse(row[08], out _level);
            int.TryParse(row[09], out _hp);
            int.TryParse(row[10], out _attack);
            int.TryParse(row[11], out _hit);
            int.TryParse(row[12], out _def);
            int.TryParse(row[13], out _res);
            int.TryParse(row[14], out _avoid);
            int.TryParse(row[15], out _attackSpeed);
            int.TryParse(row[16], out _isMagicDamage);
            int.TryParse(row[17], out _aiType);
            int.TryParse(row[18], out _giveExp);
            
            int.TryParse(row[28], out _type);

            if (_type == NpcType)
            {
                // This is a NPC so do stoof
                
                int.TryParse(row[19], out _markNumber); // NPC icon
                
                int.TryParse(row[21], out _unionNumber);
                int.TryParse(row[22], out _sellTab0);
                int.TryParse(row[23], out _sellTab1);
                int.TryParse(row[24], out _sellTab2);
                int.TryParse(row[25], out _sellTab3);
                
                int.TryParse(row[30], out _faceIcon);
            }
            else
            {
                int.TryParse(row[19], out _dropType); // mobs
                int.TryParse(row[20], out _dropMoney);
                int.TryParse(row[21], out _dropItem);
                
                int.TryParse(row[22], out _needSummonCount);
                int.TryParse(row[26], out _canTarget);
                int.TryParse(row[27], out _attackRange);
                int.TryParse(row[29], out _hitMaterialType);
                int.TryParse(row[30], out _summonMobType);
            }

            int.TryParse(row[31], out _normalEffectSound);
            int.TryParse(row[32], out _attackSound);
            int.TryParse(row[33], out _hitSound);
            int.TryParse(row[34], out _handHitEffect);
            int.TryParse(row[35], out _deadEffect);
            int.TryParse(row[36], out _dieSound);
            
            int.TryParse(row[39], out _questType);
            int.TryParse(row[40], out _glowColor);
            int.TryParse(row[42], out _height);
            int.TryParse(row[43], out _createEffect);
            int.TryParse(row[44], out _createSound);
        }
    }
}
