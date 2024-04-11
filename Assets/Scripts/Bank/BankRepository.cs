using UnityEngine;

namespace Architecture
{
    public class BankRepository : Repository
    {
        private const string KEY = "BANK_KEY";
        private int money = 10;

        public int Money
        {
            get
            {
                return this.money;
            }

            set
            {
                this.money = value;
            }
        }

        public override void Initialize()
        {
            this.Money = PlayerPrefs.GetInt(KEY, Money);
        }

        public override void Save()
        {
            PlayerPrefs.SetInt(KEY, Money);
        }
    }
}
