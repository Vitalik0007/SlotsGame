using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private GameObject dicePanel;
    [SerializeField] private Text betText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Text spinText;
    [SerializeField] private SlotMachine slotMachine;

    private Architecture.DiceRepository diceRepository;
    private Architecture.DiceInteractor diceInteractor;
    private Architecture.BankRepository bankRepository;
    private Architecture.BankInteractor bankInteractor;

    private List<GameObject> diceInstances = new List<GameObject>();

    public int currentBet;
    public int totalSpins;

    private void Start()
    {
        this.diceRepository = new Architecture.DiceRepository();
        this.diceRepository.Initialize();

        this.diceInteractor = new Architecture.DiceInteractor(this.diceRepository);
        this.diceInteractor.Initialize();

        this.bankRepository = new Architecture.BankRepository();
        this.bankRepository.Initialize();

        this.bankInteractor = new Architecture.BankInteractor(this.bankRepository);
        this.bankInteractor.Initialize();

        SpawnInitialDice();

        betText.text = currentBet + "";
    }

    private void SpawnInitialDice()
    {
        ClearDice();

        for (int i = 0; i < this.diceRepository.DiceCount; i++)
        {
            SpawnDice();
        }
    }

    private void SpawnDice()
    {
        GameObject newDice = Instantiate(dicePrefab, dicePanel.transform.position, Quaternion.identity);

        newDice.transform.SetParent(dicePanel.transform, false);

        diceInstances.Add(newDice);
    }

    private void ClearDice()
    {
        foreach (GameObject dice in diceInstances)
        {
            Destroy(dice);
        }
        diceInstances.Clear();
    }

    public void RollDice()
    {
        totalSpins = 0;

        if (this.bankInteractor.Money - currentBet >= 0)
        {
            ChangeMoney(currentBet);
            foreach (var dice in diceInstances)
            {
                Dice newDice = dice.GetComponent<Dice>();
                newDice.ShowDiceCircle();
                newDice.AnimationCompleted += OnDiceAnimationCompleted;
            }
            Invoke(nameof(OpenWinPanel), 3f);
        }
        else
            notificationPanel.SetActive(true);
    }

    private void OnDiceAnimationCompleted(int diceValue)
    {
        totalSpins += diceValue;
    }

    private void OpenWinPanel()
    {
        winPanel.SetActive(true);
        spinText.text = "You get: " + totalSpins;
        slotMachine.ShowAmountOfSpins();
    }

    public void CloseWinPanel()
    {
        winPanel.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void ChangeMoney(int count)
    {
        this.bankInteractor.SpendMoney(this, count);
    }

    public void IncreaseBet(int amount)
    {
        if (currentBet + amount <= this.bankRepository.Money)
            currentBet += amount;
        else
            Debug.Log("Not enought money");

        betText.text = currentBet + "";
    }

    public void DecreaseBet(int amount)
    {
        currentBet -= amount;
        if (currentBet < 50)
        {
            currentBet = 50;
        }
        betText.text = currentBet + "";
    }

    public void CollectReward()
    {
        this.gameObject.SetActive(false);
    }
}
