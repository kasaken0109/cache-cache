using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Witch : CharaBase, IStun
{
    [SerializeField] Rigidbody2D m_rb;
    [SerializeField] PhotonView m_view;
    Animator m_anim;
    [SerializeField]
    private int m_hp = 3; //魔法使いの体力

    [SerializeField]
    [Tooltip("変身範囲のコライダー")]
    GameObject m_cacheRangeObject = default;

    [SerializeField]
    [Tooltip("スタンする時間")]
    float m_stunTime = 3f;
    [SerializeField]
    [Tooltip("向きの点")]
    Transform[] m_directionPoints = default;
    [SerializeField]
    [Tooltip("生成するアラート")]
    private GameObject[] m_alart = new GameObject[0];
    [SerializeField]
    WitchCamera m_witchCamera = default;

    public int Hp => m_hp;
    HpDisplay hpDisplay = default;
    Collider2D m_change;
    SpriteRenderer m_sr;
    bool m_contactFlag = false;
    bool m_specter = false;
    GameObject m_camera = null;

    private void Start()
    {
        m_sr = GetComponent<SpriteRenderer>();
        m_anim = GetComponent<Animator>();
        m_change = GetComponentInChildren<Collider2D>();
        //m_view = GetComponent<PhotonView>();
        //m_rb = GetComponent<Rigidbody2D>();
        hpDisplay = GetComponent<HpDisplay>();
        m_rb.gravityScale = 0;
        StartCoroutine(a());
    }
    IEnumerator a()
    {
        yield return new WaitForSeconds(1);
        if (!m_view || !m_view.IsMine)yield break;
        Debug.Log("9999");
        Instantiate(m_witchCamera, transform);
    }
    public void SetUp()
    {
        
    }

    private void Update()
    {
        m_view.RPC("ChangeSprite",RpcTarget.All);
    }
    private void FixedUpdate()
    {
        if (!m_view || !m_view.IsMine) return;      // 自分が生成したものだけ処理する
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (m_specter)
        {
            m_witchCamera.CameraMove(h, v);
            return;
        }

        if (CanMove) Move(h, v);
        SetDirection(h, v);
    }

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector2(h, v).normalized * Speed;
        m_anim = GetComponent<Animator>();
        m_anim.SetBool("IsWalk", h == 0 && v == 0 ? false : true);
    }
    int direction = 2;
    public int SetDirection(float h, float v)
    {
        if (h == 0 && v == 0) return direction;
        else if (h < 0 && v == 0) direction = 1;
        else if (h > 0 && v == 0) direction = 2;
        else if (h == 0 && v > 0) direction = 3;
        else if (h == 0 && v < 0) direction = 4;
        m_cacheRangeObject.transform.SetParent(m_directionPoints[direction - 1]);
        m_cacheRangeObject.transform.localPosition = Vector3.zero;
        return direction;
    }

    [PunRPC]
    public void OnHit()
    {
        m_hp--; //1ずつ減らす

        if (m_hp < 1) //魔法使いの体力が1未満になったら呼び出す
        {
            m_hp = 0;
            Debug.Log("HPが0になった。");
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Die, null, raiseEventoptions, sendOptions);
            Spectating();
        }
        hpDisplay.UpdateHp(m_hp);

    }

    /// <summary>
    /// 観戦に移る
    /// </summary>
    void Spectating()
    {
        // ここでanimationでspriteを消す

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        m_witchCamera.WitchT = this.transform;
        m_specter = true;
    }

    [PunRPC]
    void ChangeSprite()
    {
        if (m_contactFlag)
        {
            if (Input.GetButtonDown("Use") && m_view.IsMine)
            {
                if (m_change.gameObject.CompareTag("Animal"))
                {
                    m_view.RPC("SetAnimator", RpcTarget.All);
                    Debug.Log(m_sr.sprite);
                }
            }
        }
    }

    [PunRPC]
    void SetAnimator()
    {
        m_anim.runtimeAnimatorController = m_change.gameObject.GetComponent<Animator>().runtimeAnimatorController;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        m_change = other;
        m_contactFlag = true;
    }
    private void OnTriggerExit2D()//Collider2D other)
    {
        m_change = null;
        m_contactFlag = false;
    }

    [PunRPC]
    public void SpawnAlarm(int id)
        => Instantiate(m_alart[id], transform.position, transform.rotation);

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
        m_rb.velocity = Vector3.zero;
        while (timer < m_stunTime)
        {
            //横に振動させる
            m_rb.AddForce(new Vector3(Mathf.Sin(timer * 180) * 100, 0, 0));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        CanMove = true;
    }
}
