using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private SpriteMask mask;
    private int randomValue;
    private float speed;
    public List<Architecture.SlotValue> stoppedSymbols = new List<Architecture.SlotValue>();
    private SlotMachine slotMachine;

    private List<float> targetPositions = new List<float> { -2.3f, -1.09f, 0.12f, 1.33f, 2.54f, 3.75f, 4.96f, 6.17f };
    public Dictionary<Architecture.SlotValue, List<Vector3>> symbolPositions = new Dictionary<Architecture.SlotValue, List<Vector3>>();
    public Dictionary<Architecture.SlotValue, GameObject> symbolGameObj = new Dictionary<Architecture.SlotValue, GameObject>();
    private float tolerance = 0.01f;

    private void Start()
    {
        slotMachine = gameObject.GetComponentInParent<SlotMachine>();
    }

    public IEnumerator Spin()
    {
        randomValue = Random.Range(0, 90);
        speed = 100f + randomValue;

        while (speed >= 10f)
        {
            speed = speed / 1.01f;
            transform.Translate(Vector2.up * Time.deltaTime * -speed);

            if (transform.localPosition.y <= -2.3f)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, 7.38f);
            }

            yield return new WaitForSeconds(slotMachine.timeInterval);
        }

        StartCoroutine("EndSpin");
        yield return null;
    }

    private IEnumerator EndSpin()
    {
        while (speed >= 2f)
        {
            float targetY = GetNearestTargetPosition(transform.localPosition.y);
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(transform.localPosition.x, targetY), speed * Time.deltaTime);

            if (Mathf.Abs(transform.localPosition.y - targetY) < tolerance)
            {
                speed = 0;
            }

            speed = speed / 1.01f;

            yield return new WaitForSeconds(slotMachine.timeInterval);
        }

        // Перевірка, чи слот знаходиться на своєму цільовому значенні, інакше виправте його позицію
        float finalTargetY = GetNearestTargetPosition(transform.localPosition.y);
        if (Mathf.Abs(transform.localPosition.y - finalTargetY) > 0.0f)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, finalTargetY);
        }

        speed = 0;
        CheckResults();
        yield return null;
    }

    private float GetNearestTargetPosition(float currentY)
    {
        float nearestTarget = targetPositions[0];
        float minDifference = Mathf.Abs(currentY - nearestTarget);

        foreach (float target in targetPositions)
        {
            float difference = Mathf.Abs(currentY - target);
            if (difference < minDifference)
            {
                minDifference = difference;
                nearestTarget = target;
            }
        }
        return nearestTarget;
    }

    private void CheckResults()
    {
        stoppedSymbols.Clear();
        symbolPositions.Clear();

        Bounds maskBounds = mask.GetComponent<SpriteMask>().bounds;

        foreach (Transform symbolTransform in transform)
        {
            if (maskBounds.Contains(symbolTransform.position))
            {
                string symbolTag = symbolTransform.tag;
                switch (symbolTag)
                {
                    case "Lemon":
                        symbolGameObj[Architecture.SlotValue.Lemon] = symbolTransform.gameObject;
                        stoppedSymbols.Add(Architecture.SlotValue.Lemon);
                        if (!symbolPositions.ContainsKey(Architecture.SlotValue.Lemon))
                        {
                            symbolPositions[Architecture.SlotValue.Lemon] = new List<Vector3>();
                        }
                        symbolPositions[Architecture.SlotValue.Lemon].Add(symbolTransform.localPosition);
                        break;
                    case "Bell":
                        symbolGameObj[Architecture.SlotValue.Bell] = symbolTransform.gameObject;
                        stoppedSymbols.Add(Architecture.SlotValue.Bell);
                        if (!symbolPositions.ContainsKey(Architecture.SlotValue.Bell))
                        {
                            symbolPositions[Architecture.SlotValue.Bell] = new List<Vector3>();
                        }
                        symbolPositions[Architecture.SlotValue.Bell].Add(symbolTransform.localPosition);
                        break;
                    case "Seven":
                        symbolGameObj[Architecture.SlotValue.Seven] = symbolTransform.gameObject;
                        stoppedSymbols.Add(Architecture.SlotValue.Seven);
                        if (!symbolPositions.ContainsKey(Architecture.SlotValue.Seven))
                        {
                            symbolPositions[Architecture.SlotValue.Seven] = new List<Vector3>();
                        }
                        symbolPositions[Architecture.SlotValue.Seven].Add(symbolTransform.localPosition);
                        break;
                    case "Watermelon":
                        symbolGameObj[Architecture.SlotValue.Watermelon] = symbolTransform.gameObject;
                        stoppedSymbols.Add(Architecture.SlotValue.Watermelon);
                        if (!symbolPositions.ContainsKey(Architecture.SlotValue.Watermelon))
                        {
                            symbolPositions[Architecture.SlotValue.Watermelon] = new List<Vector3>();
                        }
                        symbolPositions[Architecture.SlotValue.Watermelon].Add(symbolTransform.localPosition);
                        break;
                    case "Diamond":
                        symbolGameObj[Architecture.SlotValue.Diamond] = symbolTransform.gameObject;
                        stoppedSymbols.Add(Architecture.SlotValue.Diamond);
                        if (!symbolPositions.ContainsKey(Architecture.SlotValue.Diamond))
                        {
                            symbolPositions[Architecture.SlotValue.Diamond] = new List<Vector3>();
                        }
                        symbolPositions[Architecture.SlotValue.Diamond].Add(symbolTransform.localPosition);
                        break;
                    case "BigWin":
                        symbolGameObj[Architecture.SlotValue.BigWin] = symbolTransform.gameObject;
                        stoppedSymbols.Add(Architecture.SlotValue.BigWin);
                        if (!symbolPositions.ContainsKey(Architecture.SlotValue.BigWin))
                        {
                            symbolPositions[Architecture.SlotValue.BigWin] = new List<Vector3>();
                        }
                        symbolPositions[Architecture.SlotValue.BigWin].Add(symbolTransform.localPosition);
                        break;
                    case "Cherry":
                        symbolGameObj[Architecture.SlotValue.Cherry] = symbolTransform.gameObject;
                        stoppedSymbols.Add(Architecture.SlotValue.Cherry);
                        if (!symbolPositions.ContainsKey(Architecture.SlotValue.Cherry))
                        {
                            symbolPositions[Architecture.SlotValue.Cherry] = new List<Vector3>();
                        }
                        symbolPositions[Architecture.SlotValue.Cherry].Add(symbolTransform.localPosition);
                        break;
                    case "Orange":
                        symbolGameObj[Architecture.SlotValue.Orange] = symbolTransform.gameObject;
                        stoppedSymbols.Add(Architecture.SlotValue.Orange);
                        if (!symbolPositions.ContainsKey(Architecture.SlotValue.Orange))
                        {
                            symbolPositions[Architecture.SlotValue.Orange] = new List<Vector3>();
                        }
                        symbolPositions[Architecture.SlotValue.Orange].Add(symbolTransform.localPosition);
                        break;
                }
            }
        }
        slotMachine.WaitResults();
    }
}
