using System.Collections;
using UnityEngine;
public class CharacterEffects : MonoBehaviour
{
    
    [SerializeField] private bool _isWeak;
    [SerializeField] private bool _isWeaknessApplied;
    [SerializeField] private bool _isPoisoned;
    [SerializeField] private bool _isPoisonApplied;
    [field:SerializeField] public bool _isStunned {  get; private set; }
    [SerializeField] private bool _isStunApplied;

    private Coroutine _effectCoroutine;
    public float _appliedEffectDuration {  get; private set; }
    private float _appliedWeaknessCoefficient;
    private float _appliedPoisonInterval;
    private float _appliedPoisonTickDamage;

    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    private void Update()
    {
        HandleEffects();
    }
    public void SetStun(float duration) //Stun
    {
        _isStunned = true;
        _appliedEffectDuration = duration;
    }
    public void SetWeakness(float duration, float coefficient) //weakness
    {
        _isWeak = true;
        _appliedEffectDuration = duration;
        _appliedWeaknessCoefficient = coefficient;
    }

    public void SetPoison(float duration, float interval, float tickDamage) //poison
    {
        if (_isPoisoned) { return; }
        _isPoisoned = true;
        _appliedEffectDuration = duration;
        _appliedPoisonInterval = interval;
        _appliedPoisonTickDamage = tickDamage;
    }
    protected virtual IEnumerator StunCo()
    {
        _character.EnableAttack(false);
        _character.EnableMovement(false);
        _character.CharAnimator._anim.SetBool("Stun", true);
        _character._characterUI.ShowEffect("ќглушение");
        yield return new WaitForSeconds(_appliedEffectDuration);
        _character._characterUI.HideEffect();
        _character.CharAnimator._anim.SetBool("Stun", false);
        _character.EnableAttack(true);
        _character.EnableMovement(true);
        _isStunned = false;
        _isStunApplied = false;
    }
    protected IEnumerator WeaknessCo()
    {
        float reducedDmg = _character.Stats._maxDamage * (1 - _appliedWeaknessCoefficient);
        _character.Stats.ChangeDamage(reducedDmg);
        _character._characterUI.ShowEffect("—лабость");
        yield return new WaitForSeconds(_appliedEffectDuration);
        _character._characterUI.HideEffect();
        _character.Stats.ChangeDamage(_character.Stats._maxDamage);
        _isWeak = false;
        _isWeaknessApplied = false;
    }
    protected IEnumerator PoisonCo()
    {
        float elapsed = 0f;
        _character._characterUI.ShowEffect("яд");
        while (elapsed < _appliedEffectDuration)
        {
            _character.TakeDamage(_appliedPoisonTickDamage);
            yield return new WaitForSeconds(_appliedPoisonInterval);
            elapsed += _appliedPoisonInterval;
        }
        _character._characterUI.HideEffect();
        _isPoisoned = false;
        _isPoisonApplied = false;
    }
    protected void HandleEffects()
    {
        if (_isStunned && !_character._isDead && _character._battleStarted && !_isStunApplied)
        {
            _isStunApplied = true;
            PlayEffectApplication(StunCo());
        }
        if (_isWeak && !_character._isDead && _character._battleStarted && !_isWeaknessApplied)
        {
            _isWeaknessApplied = true;
            PlayEffectApplication(WeaknessCo());
        }
        if (_isPoisoned && !_character._isDead && _character._battleStarted && !_isPoisonApplied)
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

        _isStunned = false;
        _isStunApplied = false;

        _appliedEffectDuration = 0f;
        _appliedWeaknessCoefficient = 0f;
        _appliedPoisonInterval = 0f;
        _appliedPoisonTickDamage = 0f;

        if (_character != null)
        {
            _character.EnableAttack(true);

            if (_character.CharAnimator != null && _character.CharAnimator._anim != null)
                _character.CharAnimator._anim.SetBool("Stun", false);

            if (_character.Stats != null)
                _character.Stats.ChangeDamage(_character.Stats._maxDamage);

            _character._characterUI?.HideEffect();
        }
    }
    protected void PlayEffectApplication(IEnumerator effect)
    {
        if (_effectCoroutine != null)
            StopCoroutine(_effectCoroutine);

        _effectCoroutine = StartCoroutine(effect);
    }
}
