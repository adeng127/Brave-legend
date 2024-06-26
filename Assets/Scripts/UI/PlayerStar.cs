using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStar : MonoBehaviour
{
    public Image HealthImage;
    public Image HealthDelayImage;
    public Image PowerImage;

    ///<summary>
    ///接收血量变化
    /// 
    /// </summary>

    private void Update()
    {
        if (HealthDelayImage.fillAmount > HealthImage.fillAmount)
        {
            HealthDelayImage.fillAmount -= Time.deltaTime;
        }
    }
    public void OnHealthChange(float persentage)
    {
        HealthImage.fillAmount = persentage;
    }

}
