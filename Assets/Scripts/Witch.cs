using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Witch : CharaBase, IStun
{
    [SerializeField] Rigidbody m_rb;
    [SerializeField] PhotonView m_view;
    Animator m_anim;
    [SerializeField]
    private int m_hp = 3; //魔法使いの体力

    [SerializeField]
    [Tooltip("魔女が攻撃された時の加速スピード")]
    private float m_runSpeed = 9f;

    [SerializeField]
    [Tooltip("魔女が攻撃された時の効果時間")]
    private float m_godTime = 3f;

    [SerializeField]
    [Tooltip("魔女の魔力")]
    private float m_mp = 100;

    [SerializeField]
    [Tooltip("魔力が変動するスピード")]
    private float m_mpSpeed = 0.2f;

    [SerializeField]
    [Tooltip("魔力の表示UI")]
    private Image m_mpUI = default;

    [SerializeField]
    [Tooltip("変身範囲のコライダー")]
    GameObject m_cacheRangeObject = default;

    [SerializeField]
    [Tooltip("スタンエフェクト")]
    GameObject m_stunEffectObject = default;

    [SerializeField]
    [Tooltip("アタックコライダー")]
    GameObject m_attackObject = default;

    [SerializeField]
    [Tooltip("ライト範囲のコライダー")]
    GameObject m_lightObject = default;

    [SerializeField]
    [Tooltip("スタンする時間")]
    float m_stunTime = 3f;
    [SerializeField]
    [Tooltip("魔力を転送するスピード")]
    float m_chargeSpeed = 0.5f;

    [SerializeField]
    [Tooltip("攻撃ヒット時に減らすMP量")]
    float m_attackMPAmount = 30f;

    [SerializeField]
    [Tooltip("MPがないときにチャージまでにかかる時間")]
    private float m_emptyTime = 5f;

    [SerializeField]
    [Tooltip("向きの点")]
    Transform[] m_directionPoints = default;
    [SerializeField]
    [Tooltip("生成するアラート")]
    private GameObject[] m_alart = new GameObject[0];

    private RuntimeAnimatorController m_origin = default;
    [SerializeField]
    WitchCamera m_witchCamera = default;    

    public int Hp => m_hp;
    public bool IsDead { get; set; }
    HpDisplay hpDisplay = default;
    Collider m_change;
    SpriteRenderer m_sr;
    bool m_contactFlag = false;
    bool m_isGod = false;
    bool m_specter = false;
    bool m_useTask = false;
    public bool UseTask { set { m_useTask = value; } }
    GameObject m_camera = null;

    float mp;

    private void Start()
    {
        m_sr = GetComponent<SpriteRenderer>();
        m_anim = GetComponent<Animator>();
        m_change = GetComponentInChildren<Collider>();
        hpDisplay = GetComponent<HpDisplay>();
        m_origin = m_anim.runtimeAnimatorController;
        mp = m_mp;
        //StartCoroutine(CameraCreate());
    }

    IEnumerator CameraCreate()
    {
        yield return new WaitForSeconds(1);
        if (!m_view || !m_view.IsMine) yield break;
        Instantiate(m_witchCamera, transform);
    }

    private void Update()
    {
        if(!IsStopMp)mp = IsChangerd ? (mp - m_mpSpeed > 0 ? mp - m_mpSpeed : 0) : (mp + m_mpSpeed < m_mp ? mp + m_mpSpeed : m_mp);
        if (mp <= 0.01f)
        {
            m_view.RPC("SetAnimator", RpcTarget.All, false);
            IsChangerd = false;
            StartCoroutine(nameof(StopMp));
        }
        m_mpUI.fillAmount = mp / m_mp;
        m_view.RPC("ChangeSprite", RpcTarget.All);
    }
    private void FixedUpdate()
    {
        if (!m_view || !m_view.IsMine) return;      // 自分が生成したものだけ処理する
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Fire1"))
        {
            if (!IsChangerd && mp == m_mp) 
            {
                StartCoroutine("UseLight");
            }
            else if (IsChangerd && CanAttack)
            {
                StartCoroutine(nameof(Attack));
            }
        }
        
        //if (m_specter)
        //{
        //    m_witchCamera.CameraMove(h);
        //    return;
        //}
        if (CanMove) Move(h, v);
        SetDirection(h, v);
    }

    IEnumerator UseLight()
    {
        while (mp > 0)
        {
            m_view.RPC(nameof(PunSetActive),RpcTarget.All,true);
            mp -= m_mpSpeed * 2f;
            yield return null;
        }
        m_view.RPC(nameof(PunSetActive), RpcTarget.All, false);
    }


    bool IsStopMp = false;
    IEnumerator StopMp()
    {
        IsStopMp = true;
        yield return new WaitForSeconds(m_emptyTime);
        IsStopMp = false;
    }

    bool CanAttack = true;
    IEnumerator Attack()
    {
        CanAttack = false;
        m_attackObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_attackObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        CanAttack = true;
    }
    IEnumerator ChangeGod()
    {
        m_isGod = true;
        yield return new WaitForSeconds(m_godTime);
        m_isGod = false;
    }

    [PunRPC]
    void PunSetActive(bool value)
    {
        m_lightObject.SetActive(value);
    }

    bool IsSetPos = false;
    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector3(h, 0, v).normalized * (m_isGod ? m_runSpeed : Speed);
        if (IsChangerd && !IsSetPos)
        {
            transform.position += new Vector3(0, 0.75f, 0);
            IsSetPos = true;
        }
        if (!IsChangerd && IsSetPos)
        {
            transform.position += new Vector3(0, -0.75f, 0);
            IsSetPos = false;
        }
        m_anim.SetBool("IsWalk", h == 0 && v == 0 ? false : true);
        m_anim.SetBool("IsRight", h > 0 ? true : false);
    }

    public void SetDirection(float h, float v)
    {
        if (h == 0 && v == 0) return;
        m_cacheRangeObject.transform.localPosition = new Vector3(h * 1.5f, 0, v * 1.5f);
        m_attackObject.transform.localPosition = new Vector3(h * 1.5f, 0, v * 1.5f);
    }

    public void SetMp(float mpValue)
    {
        mp += mpValue;
    }

    [PunRPC]
    public void OnHit()
    {
        if (!m_view.IsMine || m_isGod) return;
        StartCoroutine("ChangeGod");
        m_hp--; //1ずつ減らす
        SetMp(-m_attackMPAmount);
        m_view.RPC("SetAnimator", RpcTarget.All, false);

        if (m_hp < 1 && !IsDead) //魔法使いの体力が1未満になったら呼び出す
        {
            m_hp = 0;
            Debug.Log("HPが0になった。");
            Spectating();
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Die, null, raiseEventoptions, sendOptions);
            IsDead = true;
        }
        hpDisplay.UpdateHp(m_hp);
    }

    /// <summary>
    /// 観戦に移る
    /// </summary>
    void Spectating()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        //m_view.RPC("SetAnimator", RpcTarget.All, false);
        m_anim.SetBool("IsDead", true);
        m_specter = true;
    }

    public void SetDisable()
    {
        m_sr.enabled = false;
    }

    bool IsChangerd = false;

    [PunRPC]
    void ChangeSprite()
    {
        if (!m_view.IsMine) return;
        if (Input.GetButtonDown("Use"))
        {
            if (m_contactFlag && m_change.gameObject.CompareTag("Animal") && !IsChangerd)
            {
                m_view.RPC("SetAnimator", RpcTarget.All, true);
                IsChangerd = true;
                gameObject.layer = 8;
            }
            else if (IsChangerd)
            {
                m_view.RPC("SetAnimator", RpcTarget.All, false);
                IsChangerd = false;
                gameObject.layer = 9;
            }
        }
    }

    [PunRPC]
    void SetAnimator(bool change)
    {
        m_anim.runtimeAnimatorController = change ? m_change.gameObject.GetComponentInParent<Animator>().runtimeAnimatorController : m_origin;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged")) return;
        m_change = other;
        m_contactFlag = true;
    }
    private void OnTriggerExit()
    {
        m_change = null;
        m_contactFlag = false;
    }

    [PunRPC]
    public void SpawnAlarm(int id)
        => Instantiate(m_alart[id], transform.position, transform.rotation,transform);

    void Dead()
    {

    }

    [PunRPC]
    public void PlayStun()
    {
        StartCoroutine(nameof(Stun));
    }

    IEnumerator Stun()
    {
        CanMove = false;
        m_stunEffectObject.SetActive(true);
        yield return new WaitForSeconds(m_stunTime);
        m_stunEffectObject.SetActive(false);
        CanMove = true;
    }
}
