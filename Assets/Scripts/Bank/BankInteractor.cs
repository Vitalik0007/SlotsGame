namespace Architecture
{
    public class BankInteractor : Interactor
    {
        private BankRepository repository;

        public int Money => this.repository.Money;

        public BankInteractor(BankRepository repository)
        {
            this.repository = repository;
        }

        public bool IsEnougthMoney(int value)
        {
            return Money >= value;
        }

        public void AddMoney(object sender, int value)
        {
            this.repository.Money += value;
            this.repository.Save();
        }

        public void SpendMoney(object sender, int value)
        {
            this.repository.Money -= value;
            this.repository.Save();
        }
    }
}
