using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStartBar : MonoBehaviour
{ 
    public Image healthImage;//健康图像：血条
    public Image healthDelayImage;//健康延迟图像：血条延迟
    public Image powerImage;//能量条
    public float redSpeed =5.0f;//血条延迟速度

    void Update()
    {
        if(healthDelayImage.fillAmount >healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -=Time.deltaTime * redSpeed;
        }

    }

    /// <summary>
    /// 接受health的百分比
    /// </summary>
    /// <param name="percentage"百分比/血量最大化current/Max</param>
    public void OnHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;
    }
}
