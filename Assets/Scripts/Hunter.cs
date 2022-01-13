using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Hunter : CharaBase
{
    [SerializeField] Rigidbody m_rb;


    [SerializeField]
    [Tooltip("開始時に待つ時間")]
    float m_waitTime = 20f;

    [SerializeField]
    [Tooltip("攻撃の時間")]
    float m_waitAttackTime = 1f;

    [SerializeField]
    [Tooltip("開始時に待つ時間を表示するUI")]
    Text m_waitDisplay = default;

    [SerializeField]
    [Tooltip("スタンする時間")]
    float m_stunTime = 3f;

    [SerializeField]
    [Tooltip("スタン時に表示される魔法陣")]
    GameObject m_stunMagic = default;

    private float m_speedUpRate = 1f;

    public float SpeedUpRate { set { m_speedUpRate = value; } }

    [SerializeField]
    [Tooltip("攻撃のコライダー")]
    GameObject m_attackObject = default;

    

    [SerializeField]
    [Tooltip("攻撃のエフェクト")]
    string m_attackEffect = default;

    [SerializeField]
    [Tooltip("攻撃判定が発生する時間")]
    float m_attackTime = 0.2f;

    [SerializeField]
    [Tooltip("スタンエフェクト")]
    private GameObject m_stunEffectObject;

    [SerializeField, Tooltip("ハンターカメラ")]
    GameObject m_hunterCamera = default;
    HunterCamera m_camera = default;

    bool CanUseItem = true;
    SpriteRenderer m_sr;
    Animator m_anim;
    [SerializeField] PhotonView m_view;
    float h, v;
    private void Start()
    {
        m_anim = GetComponent<Animator>();
        StartCoroutine(CountDown());
    }
    IEnumerator CountDown()
    {
        CanMove = false;
        int time = 0;
        while (time <= m_waitTime)
        {
            m_waitDisplay.text = (m_waitTime - time).ToString();
            yield return new WaitForSeconds(1f);
            time++;
        }
        m_waitDisplay.text = "スタート！";
        CanMove = true;
        yield return new WaitForSeconds(1f);
        m_waitDisplay.text = null;

    }
    private void Update()
    {
        if (!m_view || !m_view.IsMine || !CanMove) return;
        if (Input.GetButtonDown("Fire1") && CanAttack)
        {
            Sounds.SoundMaster.Request(transform, 1, 1);
            if (m_attackObject.activeInHierarchy) m_attackObject.SetActive(false);
            if (h >= 0)
            {
                m_anim.SetTrigger("Attack_R");
            }
            else
            {
                m_anim.SetTrigger("Attack_L");
            }
        }
        if (Input.GetButtonDown("Jump") && CanUseItem)
        {
            GetComponent<Item>().UseItem();
            ScoreManager.RequestAddScore(ActionScore.UseItem);
        }
    }
    private void FixedUpdate()
    {
        if (!m_view || !m_view.IsMine) return;      // 自分が生成したものだけ処理する
        if (CanMove)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            Move(h, v);
            SetDirection(h, v);
            //m_camera.Rotation(h, v);
        }
        else
        {
            m_rb.velocity = Vector3.zero;
        }
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
    float _timer = 0;
    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector3(h,0, v).normalized * Speed * m_speedUpRate;
        if (m_rb.velocity != Vector3.zero)
        {
            _timer += Time.deltaTime;
            if (_timer > 0.25f)
            {
                Sounds.SoundMaster.Request(transform, 14, 1);
                _timer = 0;
            }
        }
        else
        {
            _timer = 0;
        }
        m_anim.SetBool("IsWalk", (h == 0 && v == 0) ? false : true);
        m_anim.SetBool("IsRight", h > 0 ?true : false);
        
    }

    public void SetDirection(float h, float v)
    {
        if (h == 0 && v == 0) return;
        m_attackObject.transform.localPosition = new Vector3(h * 1.5f, 0, 0);
    }

    bool CanAttack = true;
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(m_attackTime);
        m_attackObject.SetActive(false);
        yield return new WaitForSeconds(m_waitAttackTime);
        CanAttack = true;
    }

    public void AttackStart()
    {
        CanAttack = false;
        m_attackObject.SetActive(true);
    }

    public void AttackEnd()
    {
        StartCoroutine(Attack());
    }

    public void SpawnEffect()
    {
        PhotonNetwork.Instantiate(m_attackEffect + (m_attackObject.transform.localPosition.x < 0 ? "Left" : ""), transform.position + new Vector3(m_attackObject.transform.localPosition.x, 0, 0),
            (Quaternion.Euler(0, 0, 0)));
    }

    /// <summary>
    /// スタンさせる、trueの時は魔法陣を表示する
    /// </summary>
    /// <param name="isStun"></param>
    /// 
    public void PlayStun(bool isStun)
    {
        StartCoroutine(Stun(isStun));
    }

    IEnumerator Stun(bool isStun)
    {
        CanMove = false;
        m_view.RPC(nameof(SetEffect), RpcTarget.All,true ,isStun);
        yield return new WaitForSeconds(m_stunTime);
        m_view.RPC(nameof(SetEffect), RpcTarget.All, false, isStun);
        CanMove = true;
    }

    [PunRPC]
    private void SetEffect(bool isActive ,bool isStun)
    {
        m_stunMagic.SetActive(isActive ? isStun && isActive : false);
        m_stunEffectObject.SetActive(isActive);
    }

    public void PlayWaitCoolDown(float time)
    {
        StartCoroutine(CoolDown(time));
    }

    IEnumerator CoolDown(float time)
    {
        CanUseItem = false;
        yield return new WaitForSeconds(time);
        CanUseItem = true;
    }

    HaveItemType itemType = HaveItemType.None;
    

    public enum HaveItemType
    {
        None,
        Enforcealarm,
        Trap,
        Enforcespeed,
    }

    public HaveItemType GetHaveItem => itemType;

    public void SetItem(HaveItemType haveItem)
    {
        itemType = haveItem;
        GetComponent<ItemDisplay>().ChangeItem((int)haveItem);
        Sounds.SoundMaster.Request(transform, 11, 1);

        switch (haveItem)
        {
            case HaveItemType.None:
                break;
            case HaveItemType.Enforcealarm:
                gameObject.AddComponent<PowerAlert>();
                break;
            case HaveItemType.Trap:
                gameObject.AddComponent<Trap>();
                break;
            case HaveItemType.Enforcespeed:
                gameObject.AddComponent<SpeedUp>();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Item") && GetHaveItem == HaveItemType.None)
        {
            Sounds.SoundMaster.Request(transform, 10, 1);
            var item = collision.GetComponent<ItemTypeGetter>();
            SetItem(item.ItemType);
            ItemManager.Instance.ResetItem(item.ID);
            ItemManager.Instance.SpawnItem(1);
            Destroy(collision.gameObject);
        }
    }
}
