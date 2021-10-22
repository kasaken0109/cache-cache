using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WitchTransform : MonoBehaviour
{
    SpriteRenderer MainSpriteRenderer;

    public enum TransformState
    {
        Witch = 0,
        Sheep = 1,
    }

    public Sprite Sheep;
    public Sprite Witch;

    [SerializeField]
    private TransformState m_transformState = TransformState.Witch;

    void Start()
    {
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public TransformState transformState
    {
        get => m_transformState;
        set
        {
            m_transformState = value;
        }
    }

    void StateSet(TransformState transformState)
    {
        if (transformState == TransformState.Witch)
        {
            MainSpriteRenderer.sprite = Witch;
        }
        if (transformState == TransformState.Sheep)
        {
            MainSpriteRenderer.sprite = Sheep;
        }
    }

    void Update()
    {
        StateSet(transformState);
    }
}
