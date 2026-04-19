using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    //确定按钮
    public Button btnSure;
    //提示信息
    public Text txtInfo;

    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }

    /// <summary>
    /// 改变提示的方法
    /// </summary>
    /// <param name="str"></param>
    public void ChangeInfo(string str)
    {
        txtInfo.text = str;
    }
}
