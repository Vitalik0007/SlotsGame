using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject[] tutorialPanels;
    private int currentPanelIndex = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextPanel();
        }
    }

    void ShowNextPanel()
    {
        tutorialPanels[currentPanelIndex].SetActive(false);

        currentPanelIndex++;

        if (currentPanelIndex < tutorialPanels.Length)
        {
            tutorialPanels[currentPanelIndex].SetActive(true);
        }
        else
        {
            tutorialPanel.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OpenTutorPanel()
    {
        tutorialPanel.SetActive(true);
    }
}