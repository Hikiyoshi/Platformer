using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, int> healthChanged;
    public UnityEvent damageableDeath;
    private Animator animator;

    [SerializeField]private int _maxHealth = 100;
    public int MaxHealth {
        get{
            return _maxHealth;
        }
        set{
            _maxHealth = value;
        }
    }

    [SerializeField]private int _health = 100;
    public int Health {
        get{
            return _health;
        }
        set{
            _health = value;

            //If health drops below 0, character is no longer alive
            if(_health <= 0){
                if(_health < 0)
                    _health = 0;
                IsAlive = false;
            }

            healthChanged?.Invoke(_health, MaxHealth);
        }
    }

    private bool _isAlive = true;
    [SerializeField]private bool isInvicible = false;
    [SerializeField]private float timeSinceHit = 0f;
    public float invicibilityTime = 0.25f;

    public bool IsAlive { get{
        return _isAlive;
    } private set{
        _isAlive = value;
        animator.SetBool(AnimationStrings.isAlive, value);

        if(value == false)
        {
            damageableDeath.Invoke();
        }
    } }

    public bool LockVelocity {
        get{
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        private set{
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Update(){
        if(isInvicible){
            if(timeSinceHit > invicibilityTime){
                //Remove invicibility
                isInvicible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockBack){
        if(IsAlive && !isInvicible){
            Health -= damage;
            isInvicible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);

            LockVelocity = true;

            damageableHit?.Invoke(damage, knockBack);

            CharacterEvents.characterDamaged(gameObject, damage);

            return true;
        }

        return false;
    }

    public bool Heal(int healthRestore){
        if(IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed(gameObject, actualHeal);

            return true;
        }

        return false;
    }
}
