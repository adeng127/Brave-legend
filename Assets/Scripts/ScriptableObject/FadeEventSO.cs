using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;

    /// <summary>
    /// Öð½¥±äºÚ
    /// </summary>
    /// <param name="duration"></param>
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }
    /// <summary>
    /// Öð½¥±äÍ¸Ã÷
    /// </summary>
    /// <param name="duration"></param>
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
    }


    public void RaiseEvent(Color targer,float duration,bool fadeIn)
    {
        OnEventRaised?.Invoke(targer, duration, fadeIn);
    }
}
