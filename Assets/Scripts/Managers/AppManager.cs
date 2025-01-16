using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    [Header("Mono Sub Managers")]

    public InputManager inputManager;

    [Header("Sub Managers")]

    public GameManager GameManager = new GameManager();

    public UIManager UIManager = new UIManager();

    [Header("Scriptables")]

    public ScriptableChestSettings scriptableChestSettings;

    [Header("Utils")]

    public Camera mainCamera;


    private void Awake()
    {
        if(Instance == null) Instance = this;

        DontDestroyOnLoad(gameObject);

        //Lets initialize sub managers

        InitializeSubManagersOnAwake(UIManager);
        InitializeSubManagersOnAwake(GameManager);
    }

    private void Start()
    {
        InitializeSubManagersOnStart(UIManager);
        InitializeSubManagersOnStart(GameManager);
    }

    private void InitializeSubManagersOnAwake(SubManagerBase _subManagerToInitialize)
    {
        if (_subManagerToInitialize == null) return;

        _subManagerToInitialize.InitializeOnAwake(this);
    }

    private void InitializeSubManagersOnStart(SubManagerBase _subManagerToInitialize)
    {
        if (_subManagerToInitialize == null) return;

        _subManagerToInitialize.InitializeOnStart(this);
    }

    public GameObject InstantiateGenericObject(GameObject _objectToClone, Transform _parent)
    {
        if (_objectToClone == null) return null;

        return Instantiate(_objectToClone, _parent);
    }

    public void DestoyObject(GameObject _objectToDestroy, float time = 0) 
    {
        if(_objectToDestroy == null) return;

        Destroy(_objectToDestroy, time);
    }
}
