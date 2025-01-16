using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestsGameplayManager : MonoBehaviour
{
    public enum GlobalChestStates
    {
        WaitingToOpen = 0,
        OpeningChest,
        ChestOpen
    }

    [SerializeField] private GlobalChestStates globalChestStates = GlobalChestStates.WaitingToOpen;

    [SerializeField] private List<ChestSettingData> chestSettings = new List<ChestSettingData>();

    [SerializeField] private string chestLayer;

    [SerializeField] private int openedChestCount = 0;

    [SerializeField] private int chestsToOpenReference;

    public void Initialize() 
    { 
    }

    private void OnEnable()
    {
        GameManager.OnGameState += ChekGameStateCalls;

        InputManager.OnClickScreen += CheckChestRaycast;

        GameManager.OnButtonPlay += InitializeChest;
    }

    private void OnDisable()
    {
        GameManager.OnGameState -= ChekGameStateCalls;

        InputManager.OnClickScreen -= CheckChestRaycast;

        GameManager.OnButtonPlay -= InitializeChest;
    }

    private void ChekGameStateCalls(GameManager.GameStates _gameStates) 
    {
        switch (_gameStates)
        {
            case GameManager.GameStates.Playing:
                break;
            case GameManager.GameStates.EndRound:
                break;
            case GameManager.GameStates.EndGame:
                break;
            
        }
    }

    int randomEmptyChest;

    private void InitializeChest() 
    {
        ClearCurrentChest();//Lets clear our current chests from scene and data

        openedChestCount = 0;

        int _chestToGenerate = AppManager.Instance.GameManager.ChestAmountInScene;

        Vector3 _chestPosition = Vector3.zero;

        int _valueToChangePose = 0;

        float _valuePosRef = 3.5f;

        float _valueToIncreasePos = _valuePosRef;

        randomEmptyChest = UnityEngine.Random.Range(0, _chestToGenerate);

        for(int i = 0; i < _chestToGenerate; i++) 
        {
            if(i != 0) 
            {
                _valueToChangePose++;

                if (_valueToChangePose % 2 == 0) _chestPosition.x = _valueToIncreasePos;
                else _chestPosition.x = _valueToIncreasePos * -1;

                if (_valueToChangePose >= 2)
                {
                    _valueToChangePose = 0;

                    _valueToIncreasePos += _valuePosRef;
                }
            }
            

            InstantiateChest(_chestPosition);
        }

        if (chestSettings.Count < 1) return;

        chestsToOpenReference = chestSettings.Count;

        for (int j = 0; j < chestSettings.Count; j++) 
        {
            if(chestSettings[j].chestCoins == 0) 
            {
                chestsToOpenReference -= 1;
                break;
            }
        }
    }

    private void InstantiateChest(Vector3 _position) 
    {
        GameObject _chestPrefab = AppManager.Instance.scriptableChestSettings.prefabChestReference;

        if (_chestPrefab == null) return;

        GameObject _chestClone = Instantiate(_chestPrefab, transform);

        if(_chestClone == null) return;

        chestSettings.Add(new ChestSettingData());

        int _index = chestSettings.Count - 1;

        chestSettings[_index].chestObject = _chestClone;

        chestSettings[_index].chestObject.name = string.Concat(_chestClone.name, " - " ,_index);

        chestSettings[_index].indexID = _index;

        chestSettings[_index].name = chestSettings[_index].chestObject.name;

        chestSettings[_index].chestObject.transform.position = _position;

        chestSettings[_index].chestPosition = _position;

        int _minCoin = AppManager.Instance.scriptableChestSettings.minCoinsInChest;
        int _maxCoin = AppManager.Instance.scriptableChestSettings.maxCoinsInChest;

        if(_minCoin < 1)_minCoin = 1;
        if(_maxCoin < 2)_maxCoin = 2;

        int _value = UnityEngine.Random.Range(_minCoin, _maxCoin);

        if(AppManager.Instance.GameManager.RoundCount > 1 && _index == randomEmptyChest)  _value = 0;

        chestSettings[_index].chestCoins = _value;

        if (chestSettings[_index].chestObject == null) return;

        Animator _animator = chestSettings[_index].chestObject.GetComponent<Animator>();

        if(_animator ==  null) return;

        chestSettings[_index].chestAnimator = _animator;
    }

    private void ClearCurrentChest() 
    {
        if(chestSettings.Count == 0) return;

        for(int i = 0; i < chestSettings.Count; i++) 
        {
            if (chestSettings[i] == null) continue;

            Destroy(chestSettings[i].chestObject);
        }

        chestSettings.Clear();
    }

    

    private void CheckChestRaycast(Vector3 _pos) 
    {
        if(AppManager.Instance.GameManager.GameState != GameManager.GameStates.Playing) return;

        Vector2 _pos2D = new Vector2(_pos.x, _pos.y);

        RaycastHit2D hit = Physics2D.Raycast(_pos2D, Vector2.zero);

        if (hit.collider == null) return;

        if(hit.collider.gameObject.layer != LayerMask.NameToLayer(chestLayer)) return;

        if (chestSettings.Count < 1) return;

        for (int i = 0; i < chestSettings.Count; i++) 
        {
            if (chestSettings[i].chestObject != hit.collider.gameObject) continue;

            if(chestSettings[i].chestCoins == 0) 
            {
                openedChestCount = 0;

                chestsToOpenReference = 0;

                AppManager.Instance.GameManager.coinsCountReward = 0;

                AppManager.Instance.GameManager.EndGame(true);

                break;
            }

            openedChestCount++;

            AppManager.Instance.GameManager.coinsCountReward += chestSettings[i].chestCoins;

            BoxCollider2D _collider2D = chestSettings[i].chestObject.GetComponent<BoxCollider2D>();

            if(_collider2D != null) Destroy(_collider2D);

            chestSettings[i].chestAnimator.CrossFadeInFixedTime("Chest_Disappear", 0.15f);

            if (openedChestCount >= chestsToOpenReference)
            {
                openedChestCount = 0;

                chestsToOpenReference = 0;

                ClearCurrentChest();

                AppManager.Instance.GameManager.EndRound();

                break;
            }
        }
    }
}
