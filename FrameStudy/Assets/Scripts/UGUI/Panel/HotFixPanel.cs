using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotFixPanel : MonoBehaviour
{
    public Image m_ProgressImage;//进度条ImageSlider
    public Text m_SpeedText;//速度Text
    public Text m_SliderTopText;//提示信息Text

    [Header("热更信息界面")]
    public GameObject m_InfoPanel;//热更信息面板
    public Text m_HotContentText;//热更内容Text
}
