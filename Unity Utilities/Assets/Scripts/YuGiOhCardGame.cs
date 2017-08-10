///A namespace containing monster classes for the Yu-Gi-Oh card game, 
///with most of the attributes you can find on your average monster, as enums.

///The namespace is best suited for Unity, but you want to use it for something else, 
///you can amend that by removing the 'using' statement, the 'SerializeField' attributes, 
///and swapping out the 'Texture2D' image reference with something else. 
///If I've missed anything, you'll likely find it.

///This was done up in about 3 hours as a fun little exercise, 
///and for this reason it's not quite complete. 

///Feel free to finish it off and commit it to GitHub: see http://yugioh.wikia.com/wiki/Attribute 
///and scroll down to 'Gameplay and Terminology', 
///then hit 'Show' to see a full list of the things you might like to add.

using UnityEngine;

namespace YuGiOh_Card_Game
{
    public enum Attribute { AIR, WATER, EARTH, FIRE, DARK, LIGHT, DIVINE }
    public enum Edition { UNLIMITED, FIRST, LIMITED }
    public enum MonsterSpecies
    {
        Aqua, Beast, BeastWarrior, Creator_God, Cyberse, Dinosaur,
        DivineBeast, Dragon, Fairy, Fiend, Fish, Insect, Machine, Plant, Psychic,
        Pyro, Reptile, Rock, Sea_Serpent, Spellcaster, Thunder, Warrior, WingedBeast, Wyrm, Zombie
    }
    public enum MonsterCardType { None, Ritual, Fusion, Synchro, Xyz, Pendulum, Link }
    public enum MonsterAbility { None, Tuner, Flip, Toon, Spirit, Union, Gemini }
    public enum MonsterEffectType { Normal, Effect }

    [System.Serializable]
    public class ID
    {
        [SerializeField]
        protected string cardSetAbbr; //The booster pack, starter deck or other set that this card is available in.
        protected string regionalAbbr; //The region of the world that this card was printed for use in.
        protected string setPositionNum; //The card's number in the set it belongs to. 
        public string cardNumber { get { return cardSetAbbr + "-" + regionalAbbr + setPositionNum; } } //The full card number. 
    }

    [System.Serializable]
    public class Card : MonoBehaviour
    {
        [SerializeField]
        protected string _title; //The card's name, written in capitals at the top. 
        public string title { get { return _title; } }

        [SerializeField]
        protected Texture2D _image; //The image shown prominently on a card, in a large square in the middle. 
        public Texture2D image { get { return _image; } }

        [SerializeField]
        protected ID _id; //The card number, displayed to the bottom right of the card's image. 
        public ID id { get { return _id; } }

        [SerializeField]
        protected string _passcode; //The eight digit code at the bottom left of the card. 
        public string passcode { get { return _passcode; } }

        [SerializeField]
        protected Edition _edition; //The edition of a card, denoted by the colour of the holographic foil in the bottom right corner of the card.
        public Edition edition { get { return _edition; } }
    }

    [System.Serializable]
    public class Monster : Card
    {
        [SerializeField]
        protected Attribute _attr; //The symbol displayed in the top right of the card.
        public Attribute attr { get { return _attr; } }

        [SerializeField]
        protected MonsterSpecies _species; //The monster's type, displayed in Bold CAPITALS in the top left of the card's description in brackets. 
        public MonsterSpecies species { get { return _species; } }

        [SerializeField]
        protected MonsterCardType _cardType; //The summoning method, displayed in Bold CAPITALS in the top left of the card's description in brackets.
        public MonsterCardType cardType { get { return _cardType; } }

        [SerializeField]
        protected MonsterAbility _playType; //The ability, displayed in Bold CAPITALS in the top left of the card's description in brackets.
        public MonsterAbility playType { get { return _playType; } }

        [SerializeField]
        protected MonsterEffectType _effectType; //Denotes whether the monster has an effect, 
        //displayed in Bold CAPITALS in the top left of the card's description in brackets if it does.
        public MonsterEffectType effectType { get { return _effectType; } }

        [SerializeField]
        protected int _ATK; //Attack points of the monster, located to the bottom right of the card. 
        public int ATK { get { return _ATK; } }
    }

    [System.Serializable]
    public class Level_Monster : Monster
    {
        [SerializeField]
        protected int _DEF; //Defense points of the monster, located to the bottom right of the card. 
        public int DEF { get { return _DEF; } }

        [SerializeField]
        protected int _level; //Level of the monster, located in between the name and the image of the card, aligned right. 
        public int level { get { return _level; } }
    }

    [System.Serializable]
    public class Levelless_Monster : Monster
    {
        //A blank class for XYZ and Link monsters to bypass having levels: XYZs have Rank, and Links have nothing.
    }

    [System.Serializable]
    public class XYZ : Levelless_Monster
    {
        [SerializeField]
        protected int _rank; //The rank of the monster, located in the same position as the level, but aligned left. 
        public int rank { get { return _rank; } }

        [SerializeField]
        protected int _DEF; //Defense points of the monster, located to the right of the ATK value.
        public int DEF { get { return _DEF; } }
    }

    [System.Serializable]
    public class Link : Levelless_Monster
    {
        [SerializeField]
        protected int _linkNumber; //The LINK number of the monster, 
        public int linkNumber { get { return _linkNumber; } }        
    }
}
