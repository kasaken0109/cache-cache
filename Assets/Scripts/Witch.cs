﻿using System.Collections;
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
    [Tooltip("MPがないときにチャージするスピード")]
    private float m_emptySpeed = 0.2f;

    [SerializeField]
    [Tooltip("向きの点")]
    Transform[] m_directionPoints = default;
    [SerializeField]
    [Tooltip("生成するアラート")]
    private GameObject[] m_alart = new GameObject[0];

    private RuntimeAnimatorController m_origin = default;
    [SerializeField]
    WitchCamera m_witchCamera = default;
    [SerializeField]
    float m_setTPTime;

    public float GetMpSpeed => mp < 1f? m_emptySpeed : m_chargeSpeed;
    public int Hp => m_hp;
    public bool IsDead { get; set; }
    HpDisplay hpDisplay = default;
    Collider m_change;
    SpriteRenderer m_sr;
    bool m_contactFlag = false;
    bool m_specter = false;
    bool m_useTask = false;
    public bool UseTask { set { m_useTask = value; } }
    GameObject m_camera = null;

    float m_time = 0;
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
        mp = IsChangerd ? (mp - m_mpSpeed > 0 ? mp - m_mpSpeed : 0) : (mp + m_mpSpeed < m_mp ? mp + m_mpSpeed : m_mp);
        if (mp <= 0.01f)
        {
            m_view.RPC("SetAnimator", RpcTarget.All, false);
            IsChangerd = false;
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
        if (v != 0)
        {
            m_time += Time.fixedDeltaTime;
            if (m_time > m_setTPTime)
            {
                m_time = 0;
                FindObjectOfType<TeleportManager>().HierarchyTP(v, transform);
            }
        }
        else
            m_time = 0;
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

    [PunRPC]
    void PunSetActive(bool value)
    {
        m_lightObject.SetActive(value);
    }

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector3(h, 0, v).normalized * Speed;
        //m_rb.constraints = m_rb.velocity == Vector2.zero ? RigidbodyConstraints2D.FreezePosition 
        //    | RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeRotation;
        m_anim.SetBool("IsWalk", h == 0 && v == 0 ? false : true);
    }

    public void SetDirection(float h, float v)
    {
        if (h == 0 && v == 0) return;
        m_cacheRangeObject.transform.localPosition = new Vector3(h * 1.5f, 0, v * 1.5f);
        m_attackObject.transform.localPosition = new Vector3(h * 1.5f, 0, v * 1.5f);
    }

    [PunRPC]
    public void OnHit()
    {
        if (!m_view.IsMine) return;
        m_hp--; //1ずつ減らす

        if (m_hp < 1 && !IsDead) //魔法使いの体力が1未満になったら呼び出す
        {
            m_hp = 0;
            Debug.Log("HPが0になった。");
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Die, null, raiseEventoptions, sendOptions);
            Spectating();
            IsDead = true;
        }
        hpDisplay.UpdateHp(m_hp);

    }

    /// <summary>
    /// 観戦に移る
    /// </summary>
    void Spectating()
    {
        // ここでanimationでspriteを消す

        gameObject.GetComponent<BoxCollider>().enabled = false;
        //m_witchCamera.WitchT = this.transform;
        m_specter = true;
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
            }
            else if (IsChangerd)
            {
                m_view.RPC("SetAnimator", RpcTarget.All, false);
                IsChangerd = false;
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
        float timer = 0;
        CanMove = false;
        m_stunEffectObject.SetActive(true);
        yield return new WaitForSeconds(m_stunTime);
        m_stunEffectObject.SetActive(false);
        CanMove = true;
    }
}
