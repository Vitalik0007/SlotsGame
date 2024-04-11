using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlotMachine : MonoBehaviour
{
    private Architecture.BankRepository bankRepository;
    private Architecture.BankInteractor bankInteractor;

    public Text moneyText;
    public Slot[] slots;
    public float timeInterval = 0.025f;
    private int stoppedSlots;
    private bool isSpin = false;

    [SerializeField] private Text spinText;
    [SerializeField] private Text winText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject dicePanel;
    [SerializeField] private DiceManager diceManager;

    private List<Architecture.SlotValue> stoppedSymbols = new List<Architecture.SlotValue>();
    private Dictionary<Architecture.SlotValue, int> symbolBalances = new Dictionary<Architecture.SlotValue, int>();
    private Dictionary<Architecture.SlotValue, float> symbolCoefficients = new Dictionary<Architecture.SlotValue, float>();
    private List<Vector3> winningSymbolPositions = new List<Vector3>();
    private List<GameObject> winningSymbols = new List<GameObject>();

    private int totalWin = 0;

    private void Start()
    {
        stoppedSlots = slots.Length;

        this.bankRepository = new Architecture.BankRepository();
        this.bankRepository.Initialize();

        this.bankInteractor = new Architecture.BankInteractor(this.bankRepository);
        this.bankInteractor.Initialize();

        InitializeSymbolCoefficients();

        ShowAmountOfMoney();
        ShowAmountOfSpins();
    }

    public void Spin()
    {
        if (!isSpin && diceManager.totalSpins > 0)
        {
            diceManager.totalSpins--;
            ShowAmountOfSpins();
            isSpin = true;
            foreach (Slot i in slots)
            {
                i.StartCoroutine("Spin");
            }
        }
        else if (diceManager.totalSpins == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WaitResults()
    {
        stoppedSlots -= 1;
        if (stoppedSlots <= 0)
        {
            stoppedSlots = slots.Length;
            CheckResults();
        }
    }

    public void CheckResults()
    {
        isSpin = false;
        CollectStoppedSymbols();
        UpdateSymbolBalances();
        CheckCombination();
    }

    private void ShowAmountOfMoney()
    {
        moneyText.text = this.bankRepository.Money.ToString();
    }

    public void ShowAmountOfSpins()
    {
        spinText.text = diceManager.totalSpins.ToString();
    }

    private void CollectStoppedSymbols()
    {
        foreach (Slot slot in slots)
        {
            stoppedSymbols.AddRange(slot.stoppedSymbols);
        }
    }

    private void UpdateSymbolBalances()
    {
        foreach (Architecture.SlotValue symbol in stoppedSymbols)
        {
            if (symbolBalances.ContainsKey(symbol))
            {
                symbolBalances[symbol]++;
            }
            else
            {
                symbolBalances[symbol] = 1;
            }
        }
    }

    private void InitializeSymbolCoefficients()
    {
        symbolCoefficients[Architecture.SlotValue.Lemon] = 1.2f;
        symbolCoefficients[Architecture.SlotValue.Bell] = 1.3f;
        symbolCoefficients[Architecture.SlotValue.Watermelon] = 1.4f;
        symbolCoefficients[Architecture.SlotValue.Seven] = 1.5f;
        symbolCoefficients[Architecture.SlotValue.Diamond] = 1.6f;
        symbolCoefficients[Architecture.SlotValue.BigWin] = 1.7f;
        symbolCoefficients[Architecture.SlotValue.Cherry] = 1.8f;
        symbolCoefficients[Architecture.SlotValue.Orange] = 1.9f;
    }

    private void CheckCombination()
    {
        int combinationCount = 0;
        int numberOfSymbols;
        bool isContinuousCombination;

        foreach (var kvp in symbolBalances)
        {
            if (kvp.Value >= 2)
            {
                isContinuousCombination = false;
                numberOfSymbols = 0;

                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].stoppedSymbols.Contains(kvp.Key))
                    {
                        numberOfSymbols++;
                        if (numberOfSymbols >= 2 && !isContinuousCombination)
                            isContinuousCombination = true;
                        else
                            continue;
                    }
                    else
                        break;
                }

                if (isContinuousCombination)
                {
                    combinationCount++;

                    foreach (var slot in slots)
                    {
                        for (int i = 0; i < slot.stoppedSymbols.Count; i++)
                        {
                            if (slot.stoppedSymbols[i] == kvp.Key)
                            {
                                foreach (var position in slot.symbolPositions[kvp.Key])
                                {
                                    if (numberOfSymbols == 0)
                                        break;
                                    winningSymbolPositions.Add(position);
                                    numberOfSymbols--;
                                }
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Total combinations: " + combinationCount);

        stoppedSymbols.Clear();
        symbolBalances.Clear();

        ShowWinningSymbols(winningSymbolPositions);
    }

    private void ShowWinningSymbols(List<Vector3> winningSymbolPositions)
    {
        totalWin = 0;
        winningSymbols.Clear();

        foreach (Slot slot in slots)
        {
            for (int i = 0; i < slot.stoppedSymbols.Count; i++)
            {
                foreach (var position in slot.symbolPositions[slot.stoppedSymbols[i]])
                {
                    if (winningSymbolPositions.Contains(position))
                    {
                        totalWin += CalculateWinning(slot.stoppedSymbols[i]);
                        winningSymbols.Add(slot.symbolGameObj[slot.stoppedSymbols[i]]);
                    }
                }
            }
        }

        if (totalWin > 0)
        {
            StartCoroutine(AnimateWinningSymbols(winningSymbols));
            winText.text = "Your win: " + totalWin;
        }
    }

    private int CalculateWinning(Architecture.SlotValue symbol)
    {
        float coefficient = symbolCoefficients[symbol];

        int win = (int)(diceManager.currentBet * coefficient);

        return win;
    }

    public void CollectReward()
    {
        EventManager.OnPlayerWin();
        this.bankInteractor.AddMoney(this, totalWin);
        ShowAmountOfMoney();
        winPanel.SetActive(false);
        winningSymbolPositions.Clear();
    }

    private IEnumerator AnimateWinningSymbols(List<GameObject> winningSymbols)
    {
        foreach (var symbol in winningSymbols)
        {
            float scaleFactor = 1.8f;
            float duration = 0.15f;
            Vector3 originalScale = symbol.transform.localScale;
            Vector3 targetScale = originalScale * scaleFactor;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                symbol.transform.localScale = Vector3.Lerp(originalScale, targetScale, time / duration);
                symbol.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                yield return null;
            }

            time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                symbol.transform.localScale = Vector3.Lerp(targetScale, originalScale, time / duration);
                yield return null;
            }
        }

        winPanel.SetActive(true);
    }
}