using System;
using UnityEngine;

[Serializable]
public class UIManager : SubManagerBase
{
    public ScriptableUIPrefabs scriptableUIPrefabs;

    public Canvas mainCanvas;

    private GameObject panelPlay;

    public override void InitializeOnAwake(AppManager _appManager)
    {
        base.InitializeOnAwake(_appManager);
    }

    public override void InitializeOnStart(AppManager _appManager)
    {
        base.InitializeOnStart(_appManager);

        InstantiateUIManager(_appManager);
    }

    private void InstantiateUIManager(AppManager _appManager = null) 
    {
        if (_appManager == null) _appManager = AppManager.Instance;

        if (scriptableUIPrefabs == null) return;
        if (scriptableUIPrefabs.UIManagerObject == null) return;

        GameObject _clone = _appManager.InstantiateGenericObject(scriptableUIPrefabs.UIManagerObject, _appManager.transform);

        if(_clone == null) return;

        mainCanvas = _clone.transform.Find("UIMainCanvas").gameObject.GetComponent<Canvas>();
    } 

    public void InstantiatePanelPlay(PanelPlayGame.PanelPlayTextSettings _textToShow) 
    {
        if (panelPlay != null) return;
        if (scriptableUIPrefabs == null) return;
        if (scriptableUIPrefabs.UIPanelPlayPrefab == null) return;

        panelPlay = AppManager.Instance.InstantiateGenericObject(scriptableUIPrefabs.UIPanelPlayPrefab, mainCanvas.transform);

        if (panelPlay == null) return;

        PanelPlayGame _cloneComponent = panelPlay.GetComponent<PanelPlayGame>();

        if (_cloneComponent == null) return;

        if(_textToShow != null) 
        {
            _cloneComponent.SetGameStateText(_textToShow.textGameStete);
            _cloneComponent.SetRewardText(_textToShow.textReward);
        }
        else 
        {
            _cloneComponent.SetGameStateText("");
            _cloneComponent.SetRewardText("");
        }

    }

    public void DestroyPanelPlay() 
    {
        if(panelPlay == null) return;

        AppManager.Instance.DestoyObject(panelPlay.gameObject);
    }
}
