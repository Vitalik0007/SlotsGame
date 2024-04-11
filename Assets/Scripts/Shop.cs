using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Text diceCount;

    private Architecture.BankRepository bankRepository;
    private Architecture.BankInteractor bankInteractor;
    private Architecture.DiceRepository diceRepository;
    private Architecture.DiceInteractor diceInteractor;

    private void Start()
    {
        this.bankRepository = new Architecture.BankRepository();
        this.bankRepository.Initialize();

        this.bankInteractor = new Architecture.BankInteractor(this.bankRepository);
        this.bankInteractor.Initialize();

        this.diceRepository = new Architecture.DiceRepository();
        this.diceRepository.Initialize();

        this.diceInteractor = new Architecture.DiceInteractor(this.diceRepository);
        this.diceInteractor.Initialize();

        diceCount.text = this.diceRepository.DiceCount + "";
    }

    public void OpenShopPanel()
    {
        shopPanel.SetActive(true);
    }

    public void CloseShopPanel()
    {
        shopPanel.SetActive(false);
    }

    public void BuyDice()
    {
        if (this.bankRepository.Money >= 200)
        {
            this.bankInteractor.SpendMoney(this, 200);
            this.diceInteractor.AddDice(this, 1);
        }

        diceCount.text = this.diceRepository.DiceCount + "";
    }
}
