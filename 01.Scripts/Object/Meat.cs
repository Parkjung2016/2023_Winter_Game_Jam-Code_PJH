using Invector.vCharacterController;
using System;
using System.Collections;
using Parkjung2016.Library;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meat : InteractableObject
{
    private ParticleSystem ps;

    public int EarnedXP;
    public int EarnedHP;
    private GameScene scene;

    protected override void Awake()
    {
        base.Awake();
        ps = GetComponentInChildren<ParticleSystem>();
        scene = (GameScene)SceneManagement.Instance.CurrentScene;
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return YieldCache.WaitUntil(() => scene.Player != null);
        EarnedHP = (int)MathLibrary.GetPercentageOfTheTotalValue(scene.Player.MaxHP, 5);
    }

    public override void Interact(CharacterBase target)
    {
        ps.Play();
        target.Eat(this);
    }

    public void DestroyRandomChild()
    {
        if (transform.childCount != 0) Destroy(transform.GetChild(Random.Range(0, transform.childCount)).gameObject);
    }

    public void BeEaten(Transform target)
    {
        if (target.GetComponent<vThirdPersonController>())
        {
            scene.Player.CurrentXP += EarnedXP;
        }

        scene.Player.CurrentHp += EarnedHP;


        Destroy(gameObject);
    }
}