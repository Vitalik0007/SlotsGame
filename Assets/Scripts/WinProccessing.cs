using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinProccessing : MonoBehaviour
{
    public ParticleSystem coinRainParticleSystem;

    private void Start()
    {
        EventManager.playerWin += OnHandleWin;
    }

    private void OnHandleWin()
    {
        //Invoke("Win", 2f);
        coinRainParticleSystem.Play();
    }

    private void Win()
    {
        coinRainParticleSystem.Play();
    }

    private void OnDestroy()
    {
        EventManager.playerWin -= OnHandleWin;
    }
}
