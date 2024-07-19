using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonConfirm : BaseItem
{
    public Text m_Title;
    public Text m_Des;
    public Button m_ConfirmBtn;
    public Button m_CancelBtn;

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="title">提示面板标题</param>
    /// <param name="des">提示的内容信息</param>
    /// <param name="confirmAction">点击确认按钮事件</param>
    /// <param name="cancleAction">点击取消按钮事件</param>
    public void Show(string title,string des,UnityEngine.Events.UnityAction confirmAction,UnityEngine.Events.UnityAction cancleAction)
    {
        m_Title.text = title;
        m_Des.text = des;
        AddButtonClickListener(m_ConfirmBtn, confirmAction);
        AddButtonClickListener(m_CancelBtn, cancleAction);
    }
}
