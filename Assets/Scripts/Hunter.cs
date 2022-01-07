using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Hunter : CharaBase, IStun
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
        if (Input.GetButtonDown("Fire1") && CanAttack) StartCoroutine(nameof(Attack));
        if (Input.GetButtonDown("Jump") && CanUseItem) GetComponent<Item>().UseItem();
    }
    private void FixedUpdate()
    {
        if (!m_view || !m_view.IsMine) return;      // 自分が生成したものだけ処理する
        if (CanMove)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
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

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector3(h,0, v).normalized * Speed * m_speedUpRate;
        m_anim.SetBool("IsWalk", (h == 0 && v == 0) ? false : true);
        m_anim.SetBool("IsRight", h > 0 ?true : false);
        
    }

    public void SetDirection(float h, float v)
    {
        if (h == 0 && v == 0) return;
        m_attackObject.transform.localPosition = new Vector3(h * 1.5f, 0, v * 1.5f);
    }

    bool CanAttack = true;
    IEnumerator Attack()
    {
        CanAttack = false;
        m_attackObject.SetActive(true);
        PhotonNetwork.Instantiate(m_attackEffect,transform.position + new Vector3(m_attackObject.transform.localPosition.x/2,0, 0),
            (m_attackObject.transform.localPosition.x < 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.Euler(0, 0, 0)));
        m_anim.SetTrigger("Attack");
        yield return new WaitForSeconds(m_attackTime);
        m_attackObject.SetActive(false);
        yield return new WaitForSeconds(m_waitAttackTime);
        CanAttack = true;
    }

    public void PlayStun()
    {
        StartCoroutine(nameof(Stun));
    }

    IEnumerator Stun()
    {
        float timer = 0;
        CanMove = false;
        m_stunEffectObject.SetActive(true);
        yield return new WaitForSeconds(m_stunTime);
        m_stunEffectObject.SetActive(false);
        CanMove = true;
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
        Enforcevisibility,
        Enforcespeed,
    }

    public HaveItemType GetHaveItem => itemType;

    public void SetItem(HaveItemType haveItem)
    {
        itemType = haveItem;
        GetComponent<ItemDisplay>().ChangeItem((int)haveItem);

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
            case HaveItemType.Enforcevisibility:
                gameObject.AddComponent<EnhancedVisibility>();
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
            var item = collision.GetComponent<ItemTypeGetter>();
            SetItem(item.ItemType);
            ItemManager.Instance.ResetItem(item.ID);
            ItemManager.Instance.SpawnItem(1);
            collision.gameObject.GetComponent<PhotonView>().TransferOwnership(m_view.OwnerActorNr);
            PhotonNetwork.Destroy(collision.gameObject);
        }
    }
}
