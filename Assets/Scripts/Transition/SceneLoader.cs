using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour,ISaveable
{
  
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    [Header("事件监听")]
    public SceneLoadEventSO LoadEventSO;
    public VoidEventSO newGame;
    public VoidEventSO BackMenuEvent;
    [Header("广播")]
    public VoidEventSO afterSceneLoaderEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unLoadedSceneEvent;
    [Header("场景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    private GameSceneSO CurrentToLoad;
    private GameSceneSO sceneToLoad;
    private Vector3 PositionToGo;
    private bool fadeScreen;
    private bool isLoading;

    public float FadeScreenTime;
    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference,LoadSceneMode.Additive);
        /* CurrentToLoad = firstLoadScene;
         CurrentToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);*/

    }

    private void Start()
    {

        LoadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();
    }
    private void OnEnable()
    {
        LoadEventSO.loadRequestEvent += OnLoadRequestEvent;
        newGame.OnEventRaise += NewGame;
        BackMenuEvent.OnEventRaise += OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        LoadEventSO.loadRequestEvent -= OnLoadRequestEvent;
        newGame.OnEventRaise -= NewGame;
        BackMenuEvent.OnEventRaise += OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        LoadEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        LoadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }
    /// <summary>
    /// 场景加载事件请求
    /// </summary>
    /// <param name="locationToGo"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>
    private void OnLoadRequestEvent(GameSceneSO locationToGo, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        sceneToLoad = locationToGo;
        PositionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        if (CurrentToLoad != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }

    }

    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            //卸载，渐出
            fadeEvent.FadeIn(FadeScreenTime);
        }
        yield return new WaitForSeconds(FadeScreenTime);
        //调整血条显示
        unLoadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, PositionToGo, true) ;
          yield return CurrentToLoad.sceneReference.UnLoadScene();
        //关闭人物
        playerTrans.gameObject.SetActive(false);


        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption=sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);

        loadingOption.Completed += OnLoadCompleted;
    }

    /// <summary>
    /// 场景加载完成后
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoadCompleted(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<SceneInstance> obj)
    {
        CurrentToLoad = sceneToLoad;

        playerTrans.position = PositionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeEvent.FadeOut(FadeScreenTime);
        }
        isLoading = false;

        if (CurrentToLoad.sceneType == SceneType.Location)
        {
            //场景加载完成后事件
            afterSceneLoaderEvent.OnEventRaise();
        }
        
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(CurrentToLoad);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            PositionToGo = data.characterPosDict[playerID];
            sceneToLoad = data.GetSavedScene();
            OnLoadRequestEvent(sceneToLoad,PositionToGo,true);
        }

    }
}
