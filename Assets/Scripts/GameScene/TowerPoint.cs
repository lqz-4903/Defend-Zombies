using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    //造塔点关联的 塔对象
    private GameObject towerObj = null;
    //造塔点关联的 塔的数据
    public TowerInfo nowTowerInfo = null;
    //可以造的三个塔的ID
    public List<int> chooseIDs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //如果现在已经有塔了 就没有必要再显示 升级界面 或者 造塔界面了
        if (nowTowerInfo != null && nowTowerInfo.nextLev == 0)
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other)
    {
        //如果不希望游戏界面下方的造塔界面显示 直接传空
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
