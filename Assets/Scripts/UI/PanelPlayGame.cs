using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelPlayGame : MonoBehaviour
{
    [Serializable]
    public class PanelPlayTextSettings 
    {
        public string textReward;
        public string textGameStete;
    }

    [SerializeField] private TextMeshProUGUI gameStateText;
    [SerializeField] private TextMeshProUGUI rewardText;

    public void PressButtonPlay() 
    {
        GameManager.OnButtonPlay?.Invoke();

        Destroy(gameObject);
    }


    public void SetGameStateText(string _text)
    {
        if (gameStateText == null) return;

        gameStateText.SetText(_text);
    }

    public void SetRewardText(string _text)
    {
        if (rewardText == null) return;

        rewardText.SetText(_text);
    }
}
