using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃を受けた時に呼ばれるインターフェース
/// </summary>
public interface IHit
{
    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    void OnHit();
}
