using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action playerWin;
    public static event Action LanternBoosterUsed;
    public static event Action SwapBoosterUsed;

    public static void OnPlayerWin()
    {
        playerWin?.Invoke();
    }

    public static void OnLanternBoosterUsed()
    {
        LanternBoosterUsed?.Invoke();
    }

    public static void OnSwapBoosterUsed()
    {
        SwapBoosterUsed?.Invoke();
    }
}
