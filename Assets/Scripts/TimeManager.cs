using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

// https://gametukurikata.com/program/time 参照
public class TimeManager : MonoBehaviour
{
	//　トータル制限時間
	private float m_totalTime;
	//　制限時間（分）
	[SerializeField] private int m_minute;
	//　制限時間（秒）
	[SerializeField] private float m_seconds;
	//　前回Update時の秒数
	private float m_oldSeconds;
	[SerializeField] Text m_timerText;

	bool m_cehck = false;
	bool m_powerAlert = false;

	int[] m_alertTime = { 5, 3, 1 };
	int m_next;

	void Start()
	{
		m_totalTime = m_minute * 60 + m_seconds;
		m_oldSeconds = 0f;
	}

	public void SetTimer(bool set) => m_cehck = set;
	// https://gametukurikata.com/program/time を参照
	void Update()
	{
		//if (!m_cehck) return;

		//　制限時間が0秒以下なら何もしない

		if (m_totalTime <= 0f)
		{
			Debug.Log("制限時間終了");

			RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
			raiseEventoptions.Receivers = ReceiverGroup.All;
			SendOptions sendOptions = new SendOptions();
			PhotonNetwork.RaiseEvent((byte)NetworkEvents.Win, null, raiseEventoptions, sendOptions);
		}

		if (m_totalTime <= 0f) return;
		//　一旦トータルの制限時間を計測；
		m_totalTime = m_minute * 60 + m_seconds;
		m_totalTime -= Time.deltaTime;

		//　再設定
		m_minute = (int)m_totalTime / 60;
		m_seconds = m_totalTime - m_minute * 60;

		//　タイマー表示用UIテキストに時間を表示する
		if ((int)m_seconds != (int)m_oldSeconds)
			m_timerText.text = m_minute.ToString("00") + ":" + ((int)m_seconds).ToString("00");

		if (m_alertTime[m_next] == m_minute && (int)m_seconds == 0)
		{
			m_next++;
			RingAlert();
		}

		m_oldSeconds = m_seconds;
		//　制限時間以下になったらコンソールに『制限時間終了』という文字列を表示する
	}

	void RingAlert()
	{
		var witches = new List<CharaBase>(FindObjectsOfType<CharaBase>()).
			Where(c =>
			{
				if (c.gameObject.CompareTag("Witch")) return c;
				else return false;
			});
		List<CharaBase> set = new List<CharaBase>(witches);
		// 自分探し
		set.ForEach(w =>
		{
			PhotonView view = w.gameObject.GetComponent<PhotonView>();
			if (view.IsMine)
			{
				if (!m_powerAlert) view.RPC("SpawnAlarm", RpcTarget.All, 0);
				else view.RPC("SpawnAlarm", RpcTarget.All, 1);
				return;
			}
		});
	}

	public void SetPowerAlert() => StartCoroutine(RingPowerAlert());
	IEnumerator RingPowerAlert()
	{
		m_powerAlert = true;

		float time = 0;
		bool check = true;
		while (check)
		{
			time += Time.deltaTime;
			if (time > 20)
			{
				check = false;
				m_powerAlert = false;
			}
			yield return null;
		}
	}
}