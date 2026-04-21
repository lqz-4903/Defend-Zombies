using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerObject : MonoBehaviour
{
    private Animator animator;

    //1.玩家属性的初始化
    //玩家攻击力
    private int atk;
    //玩家拥有的钱
    public int money;
    //旋转的速度
    private float roundSpeed = 100;

    //跟随的摄像机
    private CameraMove cameraMove;

    //持枪对象才有的开火点
    public Transform gunPoint;

    //记录角色信息
    private RoleInfo role;
    //记录是不是持枪角色
    private bool gunRole;
    //射线
    private LineRenderer line;
    //射线起点
    private Vector3 startPos;
    //射线终点
    private Vector3 endPos;

    private void Awake()
    {
        role = GameDataMgr.Instance.nowSelRole;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        cameraMove = GameObject.Find("_Cinematic_camera1").GetComponent<CameraMove>();
        cameraMove.SetTarget(this.transform);

        Cursor.lockState = CursorLockMode.Locked;

        if (role == null)
            return;
        DrawRay();

    }

    // Update is called once per frame
    void Update()
    {
        //2.移动变化 动作变化
        //移动动作的变换 由于动作有位移 我们也应用了动作的位移 所以只要改变这两个值 就会有动作的变化 和 速度的变化
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // 按住Shift时加速奔跑
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f;

        animator.SetFloat("VSpeed", vertical * speedMultiplier);
        animator.SetFloat("HSpeed", horizontal * speedMultiplier);

        //旋转
        this.transform.Rotate(Vector3.up, roundSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 1);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
            animator.SetTrigger("Roll");

        if (Input.GetMouseButton(0))
            animator.SetTrigger("Fire");

        if (gunRole)
        {
            startPos = gunPoint.position;
            endPos = gunPoint.position + transform.forward * 100f;

            line.SetPosition(0, startPos);
            line.SetPosition(1, endPos);
        }

        if (Input.GetKey(KeyCode.LeftAlt))
            Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// 初始化玩家属性
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="money"></param>
    public void InitPlayerInfo(int atk, int money)
    {
        this.atk = atk;
        this.money = money;

        //更新界面上钱的数量
        UpdateMoney();
    }

    //3.攻击动作的不同处理
    /// <summary>
    /// 专门用于处理刀类武器攻击动作的伤害检测事件
    /// </summary>
    public void KnifeEvent()
    {
        //进行伤害检测
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1,
                                                     1 << LayerMask.NameToLayer("Monster"));
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Knife");        

        for (int i = 0; i < colliders.Length; i++)
        {
            //得到碰到撞到的对象上的怪物脚本 让其受伤
            MonsterObject monster = colliders[i].gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                monster.Wound(this.atk);
                break;
            }
        }
    }


    public void ShootEvent()
    {
        //进行射线检测
        //前提是需要开火点
        RaycastHit[] hits = Physics.RaycastAll(new Ray(gunPoint.position, this.transform.forward), 1000,
                                                1 << LayerMask.NameToLayer("Monster"));
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Gun");

        for (int i = 0; i < hits.Length; i++)
        {
            //得到对象上的怪物脚本 让其受伤
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                //进行打击特效的创建
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1f);

                monster.Wound(this.atk);
                break;
            }
        }
    }


    //4.钱变化的逻辑    
    public void UpdateMoney()
    {
        //间接地更新界面上 钱的数量
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    /// <summary>
    /// 提供给外部加钱的方法
    /// </summary>
    /// <param name="money"></param>
    public void AddMoney(int money)
    {
        //加钱
        this.money += money;
        UpdateMoney();
    }

    public void DrawRay()
    { 
        if (role.type == 2)
        {
            gunRole = true;
            if (line == null)
            {
                line = this.gameObject.AddComponent<LineRenderer>();
                line.positionCount = 2;
                line.startWidth = 0.05f;
                line.endWidth = 0.05f;

                line.startColor = Color.red;
                line.endColor = Color.red;
            }            
        }
        else
            gunRole = false;
    }
}
