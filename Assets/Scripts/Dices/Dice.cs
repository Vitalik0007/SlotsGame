using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    [SerializeField] private List<Sprite> diceValueImages;

    private Dictionary<Sprite, int> diceValues = new Dictionary<Sprite, int>();
    private Image childImageComponent;
    private int currentDiceValue;

    public event System.Action<int> AnimationCompleted;

    private void Start()
    {
        for (int i = 0; i < diceValueImages.Count; i++)
        {
            diceValues.Add(diceValueImages[i], i + 1);
        }

        Transform childImageTransform = transform.Find("CircleImage");
        if (childImageTransform != null)
        {
            childImageComponent = childImageTransform.GetComponent<Image>();
        }
    }

    public void ShowDiceCircle()
    {
        StartCoroutine(AnimateDiceCircle());
    }

    private IEnumerator AnimateDiceCircle()
    {
        currentDiceValue = 0;
        int animationSteps = 10;
        float animationDelay = 0.1f;

        for (int i = 0; i < animationSteps; i++)
        {
            int randomIndex = Random.Range(1, diceValueImages.Count);
            childImageComponent.sprite = diceValueImages[randomIndex];

            yield return new WaitForSeconds(animationDelay);
        }

        yield return new WaitForSeconds(1f);

        currentDiceValue = diceValues[childImageComponent.sprite];
        AnimationCompleted?.Invoke(currentDiceValue);
    }
}
