using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data 
{
    public string SceneToSave;
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();
    public Dictionary<string, float> floatSavedData = new Dictionary<string, float>();

    public void SaveGameScene(GameSceneSO saveScene)
    {
        SceneToSave = JsonUtility.ToJson(saveScene);
        Debug.Log(SceneToSave);
    }

    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(SceneToSave, newScene);

        return newScene;
    }
}
