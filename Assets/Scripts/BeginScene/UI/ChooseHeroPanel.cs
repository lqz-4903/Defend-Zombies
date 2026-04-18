using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //左右键
    public Button btnLeft;
    public Button btnRight;
    
    //开始和返回
    public Button btnStart;
    public Button btnExit;

    //购买按钮
    public Button btnUnLock;
    public Text txtUnLock;
    
    //左上角拥有的钱
    public Text txtMoney;


    public override void Init()
    {
        
    }
}
