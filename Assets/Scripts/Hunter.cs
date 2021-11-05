using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hunter : CharaBase,IStun
{
    Rigidbody2D m_rb;

    [SerializeField]
    [Tooltip("スタンする時間")]
    float m_stunTime = 3f;

    [SerializeField]
    [Tooltip("攻撃のコライダー")]
    GameObject m_attackObject = default;

    [SerializeField]
    [Tooltip("攻撃判定が発生する時間")]
    float m_attackTime = 0.2f;

    [SerializeField, Tooltip("ハンターカメラ")]
    GameObject m_hunterCamera = default;
    HunterCamera m_camera = default;

    bool CanUseItem = true;
    Animator m_anim;
    PhotonView m_view;

    void Start()
    {
        m_view = GetComponent<PhotonView>();
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_rb.gravityScale = 0;

        if (!m_view || !m_view.IsMine) return;
        m_camera = Instantiate(m_hunterCamera, this.transform).GetComponent<HunterCamera>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) StartCoroutine(nameof(Attack));
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
            m_camera.Rotation(h, v);
        }
        else
        {
            m_rb.velocity = Vector2.zero;
        }
        //if (Input.GetButtonDown("Fire1")) StartCoroutine(nameof(Attack));
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,-10);
    }

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector2(h, v).normalized * Speed;
        m_anim.SetBool("IsWalk", h + v != 0 ? true : false);
    }

    public void SetDirection(float h, float v)
    {
        if (h == 0 && v == 0) return;
        m_attackObject.transform.localPosition = new Vector3(h * 1.5f,v * 1.5f,0);
    }

    IEnumerator Attack()
    {
        m_attackObject.SetActive(true);
        m_anim.SetTrigger("Attack");
        yield return new WaitForSeconds(m_attackTime);
        m_attackObject.SetActive(false);
    }

    public void PlayStun()
    {
        StartCoroutine(nameof(Stun));
    }

    IEnumerator Stun()
    {
        float timer = 0;
        CanMove = false;
        m_rb.velocity = Vector3.zero;
        while (timer < m_stunTime)
        {
            //横に振動させる
            m_rb.AddForce(new Vector3(Mathf.Sin(timer * 180) * 100,0,0));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
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

        switch (haveItem)
        {
            case HaveItemType.None:
                break;
            case HaveItemType.Enforcealarm:
                break;
            case HaveItemType.Trap:
                gameObject.AddComponent<Trap>();
                break;
            case HaveItemType.Enforcevisibility:
                break;
            case HaveItemType.Enforcespeed:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            SetItem(collision.GetComponent<ItemTypeGetter>().ItemType);
            PhotonNetwork.Destroy(collision.gameObject);
        }
    }
}
