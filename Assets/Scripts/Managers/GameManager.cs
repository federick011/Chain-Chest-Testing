
using System;
using UnityEngine;

[Serializable]
public class GameManager : SubManagerBase
{
    public enum GameStates
    {
        EndGame = 0,
        Playing,
        EndRound
    }

    [SerializeField] private GameStates gameState = GameStates.EndGame;

    [Space(5)]

    public int maxRounds;

    public int minChestAmount;
    public int maxChestAmount;

    public int minCoinsAmount;
    public int maxCoinsAmount;

    [Space(5)]

    [SerializeField] private int roundCount = 0;

    [SerializeField] private int chestAmountInScene;

    [SerializeField] private ChestsGameplayManager chestsGameplayManager;

    public int coinsCountReward = 0;

    //Delegates

    public static Action<GameStates> OnGameState = delegate { };

    public static Action OnButtonPlay = delegate { };


    public int RoundCount { get => roundCount; }
    public GameStates GameState { get => gameState;}
    public int ChestAmountInScene { get => chestAmountInScene; }

    public override void InitializeOnAwake(AppManager _appManager) 
    {
        base.InitializeOnAwake(_appManager);

        OnButtonPlay += SetPlayState;
    }

    public override void InitializeOnStart(AppManager _appManager)
    {
        base.InitializeOnStart(_appManager);

        ResetGameSettings();
    }

    private void SetMinAndMaxValuesFromScriptable() 
    {
        int _min = AppManager.Instance.scriptableChestSettings.minChestInScene;
        int _max = AppManager.Instance.scriptableChestSettings.maxChestInScene;

        //force at least two

        if (_min < 2) _min = 2;

        if (_max < 3) _max = 3;

        minChestAmount = _min;
        maxChestAmount = _max;

        _min = AppManager.Instance.scriptableChestSettings.minCoinsInChest;
        _max = AppManager.Instance.scriptableChestSettings.maxCoinsInChest;

        if (_min < 1) _min = 1;

        if (_max < 2) _max = 2;

        minCoinsAmount = _min;
        maxCoinsAmount = _max;
    }

    public void PreloadNextRoundSettings() 
    {
        roundCount++;

        if (roundCount >= maxRounds) 
        {
            EndGame();

            return;
        }

        SetGameState(GameStates.EndRound);

        SetMinAndMaxValuesFromScriptable();

        SetChestAmountInScene();

        CheckChestsManager();

        PanelPlayGame.PanelPlayTextSettings _textToShow = new PanelPlayGame.PanelPlayTextSettings();

        if (roundCount+1 >= maxRounds) _textToShow.textGameStete = string.Concat("Final Round");
        else _textToShow.textGameStete = string.Concat("Next Round ", roundCount);

        _textToShow.textReward = string.Concat(coinsCountReward);

        AppManager.Instance.UIManager.InstantiatePanelPlay(_textToShow); 
    }

    private void SetPlayState() 
    {
        if (GameState == GameStates.EndGame) 
        { 
            PreloadNextRoundSettings();

            OnButtonPlay?.Invoke();
        }

        SetGameState(GameStates.Playing);
    }

    public void SetGameState(GameStates _state) 
    {
        gameState = _state;

        OnGameState?.Invoke(gameState);
    }

    public void SetChestAmountInScene() 
    {
        int _value = 
            UnityEngine.Random.Range(
                AppManager.Instance.GameManager.minChestAmount, 
                AppManager.Instance.GameManager.maxChestAmount
                );

        chestAmountInScene = _value;
    }

    public void InitalizeRound() 
    {
        SetGameState(GameStates.Playing);
    }

    public void EndRound() 
    {
        PreloadNextRoundSettings();
    }

    public void EndGame(bool _loseGame = false) 
    {
        ResetGameSettings(_loseGame);
    }

    private void ResetGameSettings(bool _loseGame = false) 
    {
        SetGameState(GameStates.EndGame);

        if (chestsGameplayManager != null)
            AppManager.Instance.DestoyObject(chestsGameplayManager.gameObject);

        PanelPlayGame.PanelPlayTextSettings _textToShow = new PanelPlayGame.PanelPlayTextSettings();

        _textToShow.textGameStete = string.Concat((_loseGame) ? "You Lose" : "New Game");
        _textToShow.textReward = string.Concat(coinsCountReward);

        AppManager.Instance.UIManager.InstantiatePanelPlay(_textToShow);

        roundCount = 0;

        coinsCountReward = 0;

        int _maxRoundsRef = AppManager.Instance.scriptableChestSettings.maxRounds;

        maxRounds = (_maxRoundsRef < 3) ? 3 : _maxRoundsRef;//Force minimun 2 rounds by set 3 as value
    }

    private void CheckChestsManager() 
    {
        if (chestsGameplayManager != null) return;
        
        chestsGameplayManager = AppManager.Instance.scriptableChestSettings.InstantiateChestManager();
    }
    
}
