using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airSpeed = 3f;
    public float jumpImpulse = 10f;

    private TouchingDirections touchingDirections;
    public float currentMoveSpeed{
        get{
            if(canMove){
                if(isMoving && !touchingDirections.IsOnWall){
                    if(touchingDirections.IsGrounded){
                        if (isRunning){
                            return runSpeed;
                        }
                        else{
                            return walkSpeed;
                        }
                    }
                    else{
                        return airSpeed;
                    }
                    
                }else{
                    //Speed on the air
                    return 0;
                }
            }
            else{
                //Movement Lock
                return 0;
            }
    }}

    private bool _isFacingRight = true;
    public bool isFacingRight { 
        get{
        return _isFacingRight;
    } 
        set{
        if(_isFacingRight != value){
            transform.localScale *= new Vector2(-1, 1);
        }

        _isFacingRight = value;
    }}
    
    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving {
        get{
            return _isMoving;
    } 
        set{
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving,_isMoving);
    }}

    [SerializeField]
    private bool _isRunning = false;
    public bool isRunning {
        get{
            return _isRunning;
    } 
        set{
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning,_isRunning);
    }}

    public bool canMove {
        get{
            return animator.GetBool(AnimationStrings.canMove);
    }}

    public bool IsAlive{
        get{
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Damageable damageable;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate(){
        if(!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void onMove(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();

        if(IsAlive){
            isMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else{
            isMoving = false;
        }
    }

    public void SetFacingDirection(Vector2 moveInput){
        if(moveInput.x > 0 && !isFacingRight){
            //Facing Right
            isFacingRight = true;
        }
        else if(moveInput.x < 0 && isFacingRight){
            //Facing Left
            isFacingRight = false;
        }
    }

    public void onRun(InputAction.CallbackContext context){
        if(context.started){
            isRunning = true;
        } else if(context.canceled){
            isRunning = false;
        }
    }

    public void onJump(InputAction.CallbackContext context){
        //do with dead as well
        if(context.started && touchingDirections.IsGrounded && canMove){
            animator.SetTrigger(AnimationStrings.jumpTrigger);

            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void onAttack(InputAction.CallbackContext context){
        if(context.started){
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void onRangedAttack(InputAction.CallbackContext context){
        if(context.started && touchingDirections.IsGrounded){
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void onHit(int damage, Vector2 knockBack){
        rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
    }
}
