using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO SceneToGo;
    public Vector3 PositionToGo;
    public void TriggerAction()
    {
        loadEventSO.loadRequestEvent(SceneToGo, PositionToGo, true);
    }
}
