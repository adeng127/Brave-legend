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
    [Header("�¼�����")]
    public SceneLoadEventSO LoadEventSO;
    public VoidEventSO newGame;
    public VoidEventSO BackMenuEvent;
    [Header("�㲥")]
    public VoidEventSO afterSceneLoaderEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unLoadedSceneEvent;
    [Header("����")]
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
    /// ���������¼�����
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
            //ж�أ�����
            fadeEvent.FadeIn(FadeScreenTime);
        }
        yield return new WaitForSeconds(FadeScreenTime);
        //����Ѫ����ʾ
        unLoadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, PositionToGo, true) ;
          yield return CurrentToLoad.sceneReference.UnLoadScene();
        //�ر�����
        playerTrans.gameObject.SetActive(false);


        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption=sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);

        loadingOption.Completed += OnLoadCompleted;
    }

    /// <summary>
    /// ����������ɺ�
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
            //����������ɺ��¼�
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
