using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FortuneWheel : MonoBehaviour
{
    private Architecture.BankRepository bankRepository;
    private Architecture.BankInteractor bankInteractor;

    [SerializeField] private Text amountOfMoney;

    private int numberOfTurns;
    private int whatWeWin;

    private float speed;

    private bool canWeTurn;
    [SerializeField] private int minTurns;
    [SerializeField] private int maxTurns;

    private void Start()
    {
        this.bankRepository = new Architecture.BankRepository();
        this.bankRepository.Initialize();

        this.bankInteractor = new Architecture.BankInteractor(this.bankRepository);
        this.bankInteractor.Initialize();

        DisplayAmountOfMoney();
    }

    private void Update()
    {
        if (canWeTurn)
        {
            StartCoroutine(WheelRotation());
        }
    }

    private IEnumerator WheelRotation()
    {
        canWeTurn = false;

        numberOfTurns = Random.Range(minTurns, maxTurns);

        speed = 0.01f;

        for(int i = 0; i < numberOfTurns; i++)
        {
            transform.Rotate(0, 0, 30f);

            if (i > Mathf.RoundToInt(numberOfTurns * 0.5f))
            {
                speed = 0.02f;
            }
            if (i > Mathf.RoundToInt(numberOfTurns * 0.7f))
            {
                speed = 0.07f;
            }
            if (i > Mathf.RoundToInt(numberOfTurns * 0.9f))
            {
                speed = 0.1f;
            }

            yield return new WaitForSeconds(speed);
        }

        if (Mathf.RoundToInt(transform.eulerAngles.z) % 30 != 0)
        {
            transform.Rotate(0, 0, 30f);
        }

        whatWeWin = Mathf.RoundToInt(transform.eulerAngles.z);

        switch(whatWeWin)
        {
            case 0:
                this.bankInteractor.AddMoney(this, 75);
                break;
            case 30:
                this.bankInteractor.AddMoney(this, 100);
                break;
            case 60:
                this.bankInteractor.AddMoney(this, 125);
                break;
            case 90:
                this.bankInteractor.AddMoney(this, 150);
                break;
            case 120:
                this.bankInteractor.AddMoney(this, 175);
                break;
            case 150:
                this.bankInteractor.AddMoney(this, 200);
                break;
            case 180:
                this.bankInteractor.AddMoney(this, 225);
                break;
            case 210:
                this.bankInteractor.AddMoney(this, 250);
                break;
            case 240:
                this.bankInteractor.AddMoney(this, 275);
                break;
            case 270:
                this.bankInteractor.AddMoney(this, 300);
                break;
            case 300:
                this.bankInteractor.AddMoney(this, 25);
                break;
            case 330:
                this.bankInteractor.AddMoney(this, 50);
                break;
        }

        DisplayAmountOfMoney();
    }

    public void StartRotation()
    {
        canWeTurn = true;
    }

    private void DisplayAmountOfMoney()
    {
        amountOfMoney.text = this.bankRepository.Money.ToString();
    }
}
