using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public class PowerAlert : Item
{
    public override void UseItem()
    {
        Set();
        base.UseItem();
    }

    public void Set()
    {
		TimeManager timeManager = FindObjectOfType<TimeManager>();
		timeManager.SetPowerAlert();

		var witches = new List<CharaBase>(FindObjectsOfType<CharaBase>()).
			Where(c =>
			{
				if (c.gameObject.CompareTag("Witch")) return c;
				else return false;
			});
		List<CharaBase> set = new List<CharaBase>(witches);
		set.ForEach(w =>
		{
			PhotonView view = w.gameObject.GetComponent<PhotonView>();
			view.RPC("SpawnAlarm", RpcTarget.All, 1);
		});
	}
}
