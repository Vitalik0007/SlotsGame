using UnityEngine;

namespace Architecture
{
    public class DiceRepository : Repository
    {
        private const string KEY = "DICE_KEY";
        private int initialDiceCount = 1;

        public int DiceCount
        {
            get { return initialDiceCount; }
            set { this.initialDiceCount = value; }
        }

        public override void Initialize()
        {
            this.DiceCount = PlayerPrefs.GetInt(KEY, DiceCount);
        }

        public override void Save()
        {
            PlayerPrefs.SetInt(KEY, DiceCount);
        }
    }
}