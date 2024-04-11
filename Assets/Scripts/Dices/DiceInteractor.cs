namespace Architecture
{
    public class DiceInteractor : Interactor
    {
        private const int MAX_DICE = 3;
        private DiceRepository diceRepository;

        public int DiceCount => this.diceRepository.DiceCount;

        public DiceInteractor(DiceRepository diceRepository)
        {
            this.diceRepository = diceRepository;
        }

        public void AddDice(object sender, int value)
        {
            if (this.diceRepository.DiceCount < MAX_DICE)
            {
                this.diceRepository.DiceCount += value;
                this.diceRepository.Save();
            }
        }
    }
}