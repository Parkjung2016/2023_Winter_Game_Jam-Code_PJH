using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FIMSpace;
using FIMSpace.FProceduralAnimation;
using FIMSpace.FTail;
using Parkjung2016;
using UnityEngine;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public abstract partial class CharacterBase : MonoBehaviour, IApplyDamage
{
    public List<InteractableObject> CanInteractableList = new();
    public List<InteractableObject> interactableTarget = new();
    private FieldOfView fovInteract;
    internal Animator animator;

    protected Transform model;
    public bool IsInteracting;
    protected bool canBuff = true, buffing;


    public bool CanBuff => canBuff;
    public bool Buffing => buffing;
    protected LeaningAnimator leaningAnimator;
    protected TailAnimator2 tailAnimator;
    protected LegsAnimator legsAnimator;
    protected WeaponCollider weaponCollider;

    protected DinoControll dinoControll;

    private Sequence tailAnimatorSequence;

    private float shortFovDis_Interact;
    internal bool isMoving;

    private Meat currentMeat;
    protected CharacterSoundManager soundManager;
    [SerializeField] private float maxHP;

    protected float currentMoveSpeed = 1;
    [SerializeField] protected float buffTime, buffCoolTime;
    protected float originPower, originAttackSpeed, originSpeed;
    [SerializeField] protected float increaseBuffPower, buffAttackSpeed, buffSpeed;

    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
            OnChangedMaxHp?.Invoke(value);
        }
    }

    private float currentAtacckSpeed = 1;
    private float currentHP;

    public float CurrentHp
    {
        get => currentHP;
        set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);

            if (value <= 0)
            {
                Death();
            }

            OnChangedCurrentHp?.Invoke(currentHP);
        }
    }

    public event Action<float> OnChangedCurrentHp, OnChangedMaxHp;

    protected bool death;

    protected Material mat;

    protected virtual void Awake()
    {
        soundManager = GetComponent<CharacterSoundManager>();
        weaponCollider = GetComponentInChildren<WeaponCollider>();
        animator = GetComponent<Animator>();
        model = transform.Find("Model");
        mat = model.GetComponentInChildren<SkinnedMeshRenderer>().material;
        fovInteract = model.Find("FOV_Interact").GetComponent<FieldOfView>();
        leaningAnimator = model.GetComponent<LeaningAnimator>();
        legsAnimator = model.GetComponent<LegsAnimator>();
        tailAnimator = model.GetComponent<TailAnimator2>();
        CurrentHp = maxHP;
        dinoControll = model.GetComponent<DinoControll>();
    }


    public virtual void Interact(EInteractType type)
    {
        InteractableObject interactable = interactableTarget.Find(x => x.InteractType == type);
        interactable.Interact(this);
    }

    protected virtual void Update()
    {
        FindInteractTarget();

        animator.SetFloat(attackSpeedHash, currentAtacckSpeed);
    }

    public void Death()
    {
        death = true;
        dinoControll.SwitchEyeShape(6);
        Destroy(leaningAnimator);
        Destroy(legsAnimator);
        Destroy(tailAnimator);
        EnableAttackCollider(0);
        StopMove(true);
        CrossFadeAnimation("Death", .1f);
    }

    public virtual void DeathVariables()
    {
        Component[] components = model.GetComponents<Component>();
        // for (int i = 0; i < components.Length; i++)
        // {
        //     if (components[i] is Transform or Animator) continue;
        //     Destroy(components[i]);
        // }
        components = GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] is Transform or Animator) continue;
            Destroy(components[i]);
        }
    }

    protected void FindInteractTarget()
    {
        if (IsInteracting) return;

        for (int i = 0; i < CanInteractableList.Count; i++)
        {
            if (CanInteractableList[i] == null) CanInteractableList.RemoveAt(i);
        }

        if (fovInteract.visibleTargets.Count > 0)
        {
            if (fovInteract.visibleTargets[0] == null) return;
            // ClearInteractableList();
            shortFovDis_Interact =
                Vector3.Distance(gameObject.transform.position,
                    fovInteract.visibleTargets[0].transform.position);
            if (interactableTarget.Count == 0)
            {
                AddInteractableList(fovInteract.visibleTargets[0].GetComponentsInParent<InteractableObject>());
            }

            foreach (Transform found in fovInteract.visibleTargets)
            {
                float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

                if (Distance < shortFovDis_Interact)
                {
                    if (interactableTarget.Count >= 3) break;
                    AddInteractableList(found.GetComponentsInParent<InteractableObject>());
                    shortFovDis_Interact = Distance;
                }
            }
        }
        else
        {
            ClearInteractableList();
        }

        for (int i = 0; i < interactableTarget.Count; i++)
        {
            if (interactableTarget[i] == null || !interactableTarget[i].CanInteract ||
                !CanInteractableList.Contains(interactableTarget[i]))
                RemoveInteractableList(interactableTarget[i]);
        }
    }

    public void CrossFadeAnimation(string animationName, float duration = .1f)
    {
        animator.CrossFadeInFixedTime(animationName, duration);
    }

    public void EnableLeaningAnimator(bool enable)
    {
        leaningAnimator.enabled = enable;
    }

    public void EnableLegsAnimator(bool enable, float duration = 1)
    {
        if (enable)
            legsAnimator.User_FadeEnabled(duration);
        else
            legsAnimator.User_FadeToDisabled(duration);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) StartCoroutine(Damage());
    }

    public void EnableTailAnimator(bool enable, float duration)
    {
        if (tailAnimatorSequence != null && tailAnimatorSequence.IsActive()) tailAnimatorSequence.Kill();
        tailAnimatorSequence = DOTween.Sequence();
        tailAnimatorSequence.Append(DOTween.To(() => tailAnimator.TailAnimatorAmount,
            x => tailAnimator.TailAnimatorAmount = x, enable ? 1 : 0, duration));
    }

    public void Eat(Meat meat)
    {
        IsInteracting = true;
        CrossFadeAnimation("Eat", .1f);
        StopMove(true);
        currentMeat = meat;
        EnableLeaningAnimator(false);
    }

    public virtual void ApplyDamage(float damage)
    {
        dinoControll.SwitchEyeShape(5);
        CurrentHp -= damage;
        string hitAnimationName = $"Hit{Random.Range(1, 3)}";
        CrossFadeAnimation(hitAnimationName);

        if (damageCor != null) StopCoroutine(damageCor);
        damageCor = StartCoroutine(Damage());
    }

    private Coroutine damageCor;

    private IEnumerator Damage()
    {
        mat.SetColor(EmissionColor, new Color(1, 0, 0) * 2);
        yield return YieldCache.WaitForSeconds(.3f);
        // mat.SetColor("_Emission Color", Color.black);
    }

    public abstract void StopMove(bool stopMove);

    public void EndEat()
    {
        StopMove(false);
        if (currentMeat != null)
            currentMeat.BeEaten(transform);
        IsInteracting = false;
    }

    public void EnableAttackCollider(int enable) => weaponCollider.EnableCollider(Convert.ToBoolean(enable));

    public void DestroyMeatChild()
    {
        currentMeat.DestroyRandomChild();
    }

    public void BuffSkill()
    {
        dinoControll.SwitchEyeShape(2);

        buffing = true;
        CrossFadeAnimation("BuffSkill", .1f);
    }

    public void Buff()
    {
        print(2);
        StartCoroutine(StatBuff());
    }

    IEnumerator StatBuff()
    {
        originPower = weaponCollider.Power;
        originAttackSpeed = currentAtacckSpeed;
        originSpeed = currentMoveSpeed;

        weaponCollider.Power *= increaseBuffPower;
        currentAtacckSpeed = buffAttackSpeed;
        currentMoveSpeed = buffSpeed;
        print(currentMoveSpeed);
        yield return YieldCache.WaitForSeconds(buffTime);
        OriginStat();
        StartCoroutine(CoolTimeBuff());
        buffing = false;
        OriginEye();
    }

    public void OriginStat()
    {
        weaponCollider.Power = originPower;
        currentMoveSpeed = originSpeed;
        currentAtacckSpeed = originAttackSpeed;
    }

    IEnumerator CoolTimeBuff()
    {
        canBuff = false;
        yield return YieldCache.WaitForSeconds(buffCoolTime);
        canBuff = true;
    }

    public void OriginEye() => dinoControll.SwitchEyeShape(0);

    private readonly int attackSpeedHash = Animator.StringToHash("AttackSpeed");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
}