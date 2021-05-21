using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 - E - Usa as guard soul
 - Q - Dash
 - Q - No Ar aquele dashzão pra cima
 - Cima e Ataque - Bullet souls
 - Souls passivas sempre ativas
 - Slide - Baixo e Espaço quando tiver parado e fora das plataformas, da dash na direção que estiver olhando
 - Depois do double jump Baixo e Espaço pra chutar pra baixo
 */
/* A Resolver --- A fazer máximo sobre todos, polir tudo que econtrar, objetivo é tudo ficar o mais semelhante possivel
 - Polir o pulo, sla o personagem sobe igual um tiro, desce devagar, ta estranho (Acho que ta bom)
 - Resolver Deixar pra baixo apertado entre a queda das plataformas
 - Adcionar edge radius e semelhantes nos colliders do cenario
 - Polir o Crouch Attack, semi feito, talvez deixar mais rapido
 - Padronizar os Lenght de alguma forma, se não fizer isso vai ser um inferno adcionar armas, dar algum jeito de facilitar isso
 - Terminar de fazer os movimentos (Dash, Dash agachado e as viadagens tipo virar morcego e andar supersonico)
 - Adcionar alguns dos sons
 - Após isso se não encontrar mais essencial pra por nem polir
 - Seguir o tutorial do Brackeys pra Start menu, options menu e o outro que ta na playlist
 - Adcionar mais armas, especialmente armas de tipo diferentes (GreatSwords, fists, handgunds)
 */
public class playerController : MonoBehaviour
{
    [HideInInspector]
    public static playerController instance;
    NormalizeSlope normalizeSlopeScript;
    plataformScript thePlataformScript;
    //Movement variables
    public float moveSpeed;
    bool walked;
    bool onSlope;

    public float yVelocity;

    //Jump variables
    [HideInInspector]
    public bool isGrounded;
    public float jumpHeight;
    public bool canDoubleJump;
    bool DoubleJump;
    bool jumped;

    //Ground Check Variables
    public LayerMask GroundLayer;
    public LayerMask slopeLayer;
    public Transform groundCheckObject;
    public float checkRadius = 0.035f;
    [HideInInspector]
    public bool onPlatform;

    //Axis, Inputs and Bool Variables related to that
    float Xaxis;
    float Yaxis;
    [HideInInspector]
    public bool isJumpPressed;
    [HideInInspector]
    public bool isAttackPressed;
    [HideInInspector]
    public bool facingRight;

    //Attack Variables
    [HideInInspector]
    public bool isAttacking;
    attackController attackScript;
    //Attack Variables Time
    public float LightIdleAtk;
    public float CrouchLightAtk;
    public float JumpLightAtk;

    //Crouch
    [HideInInspector]
    public bool isCrouched;
    public bool canCrouchDash;


    //Miscelaneous
    Rigidbody2D myRB;
    Animator myAnim;
    string currentAnimation;

    //Animation States
    //Reset
    const string reset = "reset";
    //Idle
    const string idle = "idle";
    //Walking
    const string walkInit = "comecoAndar";
    const string walk = "walk";
    const string walkToIdle = "walkToIdleTransition";
    //VerticalJump
    const string verticalJumpInit = "verticalJumpInit";
    const string verticalJumpTransition = "verticalJumpFallTransition";
    const string verticalJumpFall = "verticalJumpFall";
    const string verticalJumpLand = "verticalJumpLand";
    //SideJump
    const string sideJumpInit = "sideJumpInit";
    const string sideJumpTransition = "sideJumpFallTransition";
    const string sideJumpFall = "sideJumpFall";
    //Crouch
    const string crounching = "crounching";
    const string crouched = "crouched";
    const string crouchAttack = "crouchAttack";
    const string crouchToIdle = "crouchToIdle";
    //Attack
    const string attack = "attack";
    const string jumpAttack = "jumpAttack";


    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        normalizeSlopeScript = GetComponent<NormalizeSlope>();
        attackScript = GetComponentInChildren<attackController>();
        facingRight = true;
        instance = this;
        DoubleJump = canDoubleJump;
    }

    void Update()
    {
        //Check the X and Y values of the input
        Xaxis = Input.GetAxisRaw("Horizontal");
        Yaxis = Input.GetAxisRaw("Vertical");

        //Check the Jump, Attack and Crouch Inputs
        if (Input.GetButtonDown("Jump"))
        {
            isJumpPressed = true;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            isAttackPressed = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        yVelocity = myRB.velocity.y;
        //Check if the player isGrounded
        isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, checkRadius, GroundLayer);
        //Check if the player is on a slope
        onSlope = Physics2D.OverlapCircle(groundCheckObject.position, checkRadius, slopeLayer);

        //Check if need to move
        if(Xaxis != 0 && !isCrouched && !isAttacking && isGrounded)
        {
            //myRB.velocity = new Vector2(moveSpeed * Xaxis, myRB.velocity.y);
            normalizeSlopeScript.MoveAndNormalizeSlope(Xaxis, moveSpeed);
        }else if(Xaxis != 0 && !isCrouched && !isGrounded)
        {
            myRB.velocity = new Vector2(moveSpeed * Xaxis, myRB.velocity.y);
        }
        else
        {
            myRB.velocity = new Vector2(0, myRB.velocity.y);
        }
        //Set the idle or the Run animations
        if (isGrounded && !isAttacking && !isCrouched && (myRB.velocity.y > -0.1f || onSlope))
        {
            if(Xaxis != 0 && !isCrouched)
            {
                ChangeAnimations(walkInit);
                walked = true;
            }
            else if(Xaxis == 0 && walked)
            {
                ChangeAnimations(walkToIdle);
                walked = false;
            }
            else
            {
                if(GetCurrentAnimInAnimator() != verticalJumpLand && GetCurrentAnimInAnimator() != walkToIdle
                    && GetCurrentAnimInAnimator() != crouchToIdle && !IsInputing()) { 
                    ChangeAnimations(idle);
                }
            }
            
        }
        //Jump Land Check
        if (isGrounded)
        {
            //Reset DoubleJump
            DoubleJump = canDoubleJump;

            if (jumped && Xaxis == 0 && !onPlatform)
            {
                ChangeAnimations(verticalJumpLand);
            }
            //Check if player hits ground and cancel the jump anim
            if(GetCurrentAnimInAnimator().Contains("jump") || GetCurrentAnimInAnimator().Contains("Jump"))
            {
                //ChangeAnimations(verticalJumpLand);
                DestroyWeapon();
                
            }
            //If player is standind still on a slope, this block of code prevents him from sliding down the fucking slope
            if(onSlope && !IsInputing())
            {
                FreezeConstraints();
            }
            else if(onSlope && (isCrouched || isAttacking))
            {
                FreezeConstraints();
            }
            else
            {
                UnFreezeConstraints();
            }
            jumped = false;
        }

        //Jump Check
        if (isJumpPressed)
        {
            if(isCrouched && onPlatform && !isAttacking)
            {
                isCrouched = false;
                ChangeAnimations(verticalJumpFall);
                isJumpPressed = false;
                thePlataformScript.Rotate();

                
            }
            else if(isGrounded && !isAttacking && Xaxis != 0)
            {
                myRB.velocity = Vector2.zero;
                myRB.AddForce(Vector2.up * jumpHeight);
                isJumpPressed = false;
                jumped = true;
                ChangeAnimations(sideJumpInit);
            }
            else if(isGrounded && !isAttacking && Xaxis == 0)
            {
                myRB.velocity = Vector2.zero;
                myRB.AddForce(Vector2.up * jumpHeight);
                isJumpPressed = false;
                jumped = true;
                ChangeAnimations(verticalJumpInit);
            }
            else
            {
                if(DoubleJump && !isAttacking && Xaxis != 0)
                {
                    myRB.velocity = Vector2.zero;
                    myRB.AddForce(Vector2.up * jumpHeight);
                    isJumpPressed = false;
                    jumped = true;
                    DoubleJump = false;
                    ChangeAnimations(sideJumpInit);
                }
                else if(DoubleJump && !isAttacking && Xaxis == 0)
                {
                    myRB.velocity = Vector2.zero;
                    myRB.AddForce(Vector2.up * jumpHeight);
                    isJumpPressed = false;
                    jumped = true;
                    DoubleJump = false;
                    ChangeAnimations(verticalJumpInit);
                }else if (!DoubleJump)
                {
                    isJumpPressed = false;
                }
            }
        }

        //Check if the player is just falling (Example, fallin of a ledge)
        if(myRB.velocity.y < 0 && !isAttacking && !isCrouched && !isGrounded && !jumped)
        {
            ChangeAnimations(verticalJumpFall);
        }

        //Crouch
        if (Yaxis < -0.1f)
        {
            if (isGrounded && !isAttacking && GetCurrentAnimInAnimator() != crouched && myRB.velocity.y == 0)
            {
                isCrouched = true;
                ChangeAnimations(crounching);
            }
        }
        else if(Yaxis > -0.1f && isCrouched && !isAttacking)
        {
            ChangeAnimations(crouchToIdle);
            isCrouched = false;
        }

        //Attack
        if (isAttackPressed)
        {
            isAttackPressed = false;
            if (!isAttacking)
            {
                isAttacking = true;
                if (isGrounded && !isCrouched && myRB.velocity.y == 0)
                {
                    AnimNeedReset(attack);
                    attackScript.LightAttack();
                    Invoke("AttackEnd", LightIdleAtk);        
                }
                else if (isGrounded && (Yaxis < 0f || GetCurrentAnimInAnimator() == crounching) && myRB.velocity.y == 0)
                {
                    AnimNeedReset(crouchAttack);
                    attackScript.CrouchedLightAttack();
                    Invoke("AttackEnd", CrouchLightAtk);
                }
                else if (!isGrounded)
                {
                    AnimNeedReset(jumpAttack);
                    attackScript.LightAttack();
                    Invoke("AttackEnd", JumpLightAtk);
                }
                else
                {
                    AttackEnd();
                }
            }
            
        }

        //Flip
        if(Xaxis > 0 && !facingRight && !isAttacking){
            Flip();
        }else if(Xaxis < 0 && facingRight && !isAttacking){
            Flip();
        }
    }
    void Flip(){
        facingRight = !facingRight;
        Vector3 flipScale = transform.localScale;
        flipScale.x *= -1;
        transform.localScale = flipScale;
    }
    
    void DestroyWeapon()
    {
        GameObject instantiatedWeapon = attackScript.instantiatedWeapon;
        if (instantiatedWeapon != null)
        {
            Destroy(instantiatedWeapon.gameObject);
        }
    }

    void AttackEnd()
    {
        DestroyWeapon();
        if (isCrouched)
        {
            ChangeAnimations(crouched);
        }
        if(GetCurrentAnimInAnimator() == jumpAttack)
        {
            ChangeAnimations(verticalJumpTransition);
        }
        isAttacking = false;
    }

    void ChangeAnimations(string newAnimation)
    {
        //Sai da função se animação for a mesma
        if (currentAnimation == newAnimation) return;
        //Caso não for toca a nova animação e seta animação atual como a nova
        myAnim.Play(newAnimation);
        currentAnimation = newAnimation;
    }
    void ResetAnim(string newAnimation)
    {
        myAnim.Play(newAnimation, -1, 0f);
    }
    void AnimNeedReset(string animationToAnalize)
    {
        if (GetCurrentAnimInAnimator() == animationToAnalize)
        {
            ResetAnim(animationToAnalize);
        }
        else
        {
            ChangeAnimations(animationToAnalize);
        }
    }
    public string GetCurrentAnimInAnimator()
    {
        AnimatorClipInfo animatorStateInfo = myAnim.GetCurrentAnimatorClipInfo(0)[0];
        return animatorStateInfo.clip.name;
    }
    public float GetCurrentAnimLength()
    {
        AnimatorClipInfo animatorStateInfo = myAnim.GetCurrentAnimatorClipInfo(0)[0];
        return animatorStateInfo.clip.length;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);
        Gizmos.DrawWireSphere(groundCheckObject.position, checkRadius);
    }

    void FreezeConstraints()
    {
        myRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    void UnFreezeConstraints()
    {
        myRB.constraints = RigidbodyConstraints2D.None;
        myRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    bool IsInputing()
    {
        if(Xaxis == 0 && Yaxis == 0 && !isJumpPressed && !isAttackPressed)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Platform")
        {
            thePlataformScript = other.transform.GetComponent<plataformScript>();
        }
    }
}
