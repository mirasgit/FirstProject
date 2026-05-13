using System;
using System.Collections;
using UnityEngine;
public class CharacterEffects : MonoBehaviour
{
    
    [SerializeField] private bool _isWeak;
    [SerializeField] private bool _isWeaknessApplied;
    [SerializeField] private bool _isPoisoned;
    [SerializeField] private bool _isPoisonApplied;
    [field:SerializeField] public bool IsStunned {  get; private set; }
    [SerializeField] private bool _isStunApplied;

    private Coroutine _effectCoroutine;
    public float AppliedEffectDuration {  get; private set; }
    private float _appliedWeaknessCoefficient;
    private float _appliedPoisonInterval;
    private float _appliedPoisonTickDamage;

    private Character _character;
    public event Action<string> EffectShown;
    public event Action EffectHidden;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    private void Update()
    {
        HandleEffects();
    }
    private void ShowEffect(string effectName)
    {
        EffectShown?.Invoke(effectName);
    }
    private void HideEffect()
    {
        EffectHidden?.Invoke();
    }
    public void SetStun(float duration) //Stun
    {
        IsStunned = true;
        AppliedEffectDuration = duration;
    }
    public void SetWeakness(float duration, float coefficient) //weakness
    {
        _isWeak = true;
        AppliedEffectDuration = duration;
        _appliedWeaknessCoefficient = coefficient;
    }

    public void SetPoison(float duration, float interval, float tickDamage) //poison
    {
        if (_isPoisoned) { return; }
        _isPoisoned = true;
        AppliedEffectDuration = duration;
        _appliedPoisonInterval = interval;
        _appliedPoisonTickDamage = tickDamage;
    }
    protected virtual IEnumerator StunCo()
    {
        _character.EnableAttack(false);
        _character.EnableMovement(false);
        _character.CharAnimator._anim.SetBool("Stun", true);
        ShowEffect("ќглушение");
        yield return new WaitForSeconds(AppliedEffectDuration);
        HideEffect();
        _character.CharAnimator._anim.SetBool("Stun", false);
        _character.EnableAttack(true);
        _character.EnableMovement(true);
        IsStunned = false;
        _isStunApplied = false;
    }
    protected IEnumerator WeaknessCo()
    {
        float reducedDmg = _character.Stats.MaxDamage * (1 - _appliedWeaknessCoefficient);
        _character.Stats.ChangeDamage(reducedDmg);
        ShowEffect("—лабость");
        yield return new WaitForSeconds(AppliedEffectDuration);
        HideEffect();
        _character.Stats.ChangeDamage(_character.Stats.MaxDamage);
        _isWeak = false;
        _isWeaknessApplied = false;
    }
    protected IEnumerator PoisonCo()
    {
        float elapsed = 0f;
        ShowEffect("яд");
        while (elapsed < AppliedEffectDuration)
        {
            _character.TakeDamage(_appliedPoisonTickDamage);
            yield return new WaitForSeconds(_appliedPoisonInterval);
            elapsed += _appliedPoisonInterval;
        }
        HideEffect();
        _isPoisoned = false;
        _isPoisonApplied = false;
    }
    protected void HandleEffects()
    {
        if (IsStunned && !_character.IsDead && _character.BattleStarted && !_isStunApplied)
        {
            _isStunApplied = true;
            PlayEffectApplication(StunCo());
        }
        if (_isWeak && !_character.IsDead && _character.BattleStarted && !_isWeaknessApplied)
        {
            _isWeaknessApplied = true;
            PlayEffectApplication(WeaknessCo());
        }
        if (_isPoisoned && !_character.IsDead && _character.BattleStarted && !_isPoisonApplied)
        {
            _isPoisonApplied = true;
            PlayEffectApplication(PoisonCo());
        }
    }
    public void ResetEffects()
    {
        if (_effectCoroutine != null)
        {
            StopCoroutine(_effectCoroutine);
            _effectCoroutine = null;
        }

        _isWeak = false;
        _isWeaknessApplied = false;

        _isPoisoned = false;
        _isPoisonApplied = false;

        IsStunned = false;
        _isStunApplied = false;

        AppliedEffectDuration = 0f;
        _appliedWeaknessCoefficient = 0f;
        _appliedPoisonInterval = 0f;
        _appliedPoisonTickDamage = 0f;

        if (_character != null)
        {
            _character.EnableAttack(true);

            if (_character.CharAnimator != null && _character.CharAnimator._anim != null)
                _character.CharAnimator._anim.SetBool("Stun", false);

            if (_character.Stats != null)
                _character.Stats.ChangeDamage(_character.Stats.MaxDamage);

            _character.CharacterUI?.HideEffect();
        }
    }
    protected void PlayEffectApplication(IEnumerator effect)
    {
        if (_effectCoroutine != null)
            StopCoroutine(_effectCoroutine);

        _effectCoroutine = StartCoroutine(effect);
    }
}
