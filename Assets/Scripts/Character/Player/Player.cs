using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // public
    public static readonly float AbsorbGroundedInputTime = 0.1f;
    public float SpeedAccelerationOnGround;
    public float SpeedAccelerationInAir;
    public float MaxHealth = 100f;
    public Transform GunShootPoint;

    public GameObject HurtEffect;
    public GameObject DeadEffect;

    public GameObject AddHPEffect;
    public GameObject AddGoldEffect;
    public GameObject OnFireEffect;

    [HideInInspector]
    public BaseGun CurrentWeapon;  // 当前枪
    [HideInInspector]
    public BaseGun OwnGun;
    public GameObject Gun_Mo;
    public SkinnedMeshRenderer[] MeshRenderers;
    public Animator Animator;

    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsHurt { get; set; }
    public bool IsControl { get; set; }
    public bool HasKey { get; set; }
    public bool IsMovable { get; set; }
    public bool IsAdsorb { get; set; }
    public CharacterController2D Controller
    {
        get
        {
            return controller;
        }
    }
    [HideInInspector]
    public bool isFacingRight;
    [HideInInspector]
    public float normalizedHorizontalSpeed;

    // protect
    protected CharacterController2D controller;
    protected BoxCollider2D boxCollider2D;
    protected float absorbGroundedInputTimer = 0.0f;
    protected bool isAttacking = false;
    protected int horizontal = 0;
    protected bool isPressGunAttack = false;
    protected bool isPressJumping = false;
    protected bool isSlowDown = false;
    protected int gold;
    protected int savedHostage = 0;
    protected float movementFactor;
    protected int jumpCount = 0;
    protected float attackRate;
    protected Vector2 touchPosition = Vector2.zero;
    protected bool isAutoShooting = false;

    void Awake()
    {
        HasKey = false;
        controller = GetComponent<CharacterController2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        isFacingRight = Animator.transform.localScale.x > 0;
        savedHostage = 0;

        CurrentHealth = MaxHealth;

        IsDead = false;
        IsHurt = false;
        IsControl = true;
        IsMovable = true;
        IsAdsorb = false;

        jumpCount = 2;

        //string path = "Weapon/" + DataManager.Instance.PInfo.RWeapon.Name;
        string path = "Weapon/MP5";
        OwnGun = Resources.Load<BaseGun>(path);
        Gun_Mo.GetComponent<SkinnedMeshRenderer>().material = OwnGun.GunMaterial;
        CurrentWeapon = OwnGun;
        CurrentWeapon.Init(GunShootPoint);
        attackRate = 0;
    }

    void Start()
    {
        SoundManager.Instance.PlayerShotSound = CurrentWeapon.ShootSound;
    }

    void Update()
    {
        // reset state
        Animator.speed = 1.0f;
        // for when the player is moving by the platform then hit the platform or item(dropingblock or other collision)
        if (controller.IsHittingWall)
        {
            controller.HandlePlatforms = false;
        }
        else
        {
            controller.HandlePlatforms = true;
        }

        if (IsControl)
        {
            // handle horizontal input
            handleHorizontalInput();
            // handle vertical input
            handleJumpInput();
            // handle attack input
            handleAttackInput();

            handleGestureInput();
        }
    }

    public void HitBack(float x, float y)
    {
        HitBack(x, y, 0.1f);
    }

    public void HitBack(float x, float y, float time)
    {
        if (CurrentHealth > 0)
        {
            IsControl = false;
            boxCollider2D.enabled = false;
            controller.SetForce(new Vector2(x, y));
            StartCoroutine(hitBackCo(time));
        }
    }

    private IEnumerator hitBackCo(float time)
    {

        yield return new WaitForSeconds(time);
        boxCollider2D.enabled = true;
        controller.SetHorizontalForce(0.0f);
        //yield return new WaitForSeconds(0.3f);
        IsControl = true;
    }

    void LateUpdate()
    {

        if (controller.IsGrounded && Animator.GetBool("Jumping") == true && Controller.Velocity.y <= 0)
        {
            Animator.SetBool("Jumping", false);

        }
    }

    private void handleHorizontalInput()
    {

        this.absorbGroundedInputTimer -= Time.deltaTime;
        this.absorbGroundedInputTimer = Mathf.Max(0.0f, this.absorbGroundedInputTimer);
        //float input = 0;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //input = Input.GetAxisRaw("Horizontal");
        normalizedHorizontalSpeed = Input.GetAxisRaw("Horizontal");

#elif UNITY_ANDROID || UNITY_IOS
        normalizedHorizontalSpeed = horizontal;
#endif

        if (normalizedHorizontalSpeed != 0)
        {
            if (Flip(normalizedHorizontalSpeed))
            {
                normalizedHorizontalSpeed = 0;
                absorbGroundedInputTimer = AbsorbGroundedInputTime;
            }
        }

        if (this.controller.IsGrounded && this.absorbGroundedInputTimer > 0)
        {
            controller.SetHorizontalForce(0.0f);
        }

        if (Mathf.Abs(normalizedHorizontalSpeed) > 0)
        {
            Animator.SetFloat("Speed", 1);
        }
        else if (Mathf.Abs(normalizedHorizontalSpeed) == 0)
        {
            Animator.SetFloat("Speed", 0);
        }

        //movementFactor = controller.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        movementFactor = controller.IsGrounded ? controller.Parameters.MaxVelocity.x : controller.Parameters.MaxVelocity.x * 0.95f;

        if (!IsMovable)
        {
            movementFactor = 0.0f;
        }

        if (IsAdsorb)
        {
            movementFactor = 0.5f;
        }

        float force = normalizedHorizontalSpeed * movementFactor;
        controller.SetHorizontalForce(force);
    }

    private void handleSlidingWalls()
    {

        /*  bool wallSliding = false;
          int wallDirX = controller.collisionState.Left ? -1 : 1;

          if (controller.IsHittingWall && !controller.IsGrounded && controller.Velocity.y < 0)
          {

              wallSliding = true;

              if (controller.Velocity.y < -SpeedWallSlide)
              {
                  controller.SetVerticalForce(-SpeedWallSlide);

              }

              /*if (unStickTime > 0)
              {
                  controller.SetVerticalForce(0.0f);

                  if (normalizedHorizontalSpeed != wallDirX && normalizedHorizontalSpeed != 0)
                  {
                      unStickTime -= Time.deltaTime;
                  }
                  else
                  {
                      unStickTime = WallStickTime;
                  }
              }
              else
              {
                  unStickTime = WallStickTime;
              }
          }

          bool status = false;
  #if  UNITY_EDITOR || UNITY_STANDALONE_WIN
          status = Input.GetButtonDown("Jump");

  #elif UNITY_ANDROID || UNITY_IOS
          status = joystickJumping;
  #endif


          if (status)
          {
              if (wallSliding)
              {
                  if (wallDirX == normalizedHorizontalSpeed)
                  {
                      Debug.Log("same direction");
                      //controller.SetForce(new Vector2(-wallDirX * SpeedAccelerationInAir * 4, controller.Parameters.JumpMagnitude));
                      controller.SetVerticalForce(controller.Parameters.JumpMagnitude);
                  }
                  else if (normalizedHorizontalSpeed == 0)
                  {
                      Debug.Log("0");
                      controller.SetForce(new Vector2(-wallDirX * SpeedAccelerationInAir * 1, controller.Parameters.JumpMagnitude));
                  }
                  else
                  {
                      Debug.Log("different direction");
                      controller.SetForce(new Vector2(-wallDirX * SpeedAccelerationInAir * 1, controller.Parameters.JumpMagnitude));
                  }
  #if  UNITY_EDITOR || UNITY_STANDALONE_WIN


  #elif UNITY_ANDROID || UNITY_IOS
          joystickJumping = false;
  #endif
              }
          }*/

    }

    private void handleJumpInput()
    {
#if  UNITY_EDITOR || UNITY_STANDALONE_WIN

        if (controller.IsGrounded)
        {

            jumpCount = 2;
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("A"))
            {

                jumpCount -= 1;
                Animator.SetBool("Jumping", true);
                controller.Jump();
            }
        }
        else
        {

            if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("A")) && jumpCount == 1)
            {

                jumpCount -= 1;
                Animator.Play("Base Layer.JumpTree", 0, 0);
                controller.Jump();
            }
        }

#elif UNITY_ANDROID || UNITY_IOS
    
#endif
    }

    private void handleGestureInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("B"))
        {
            this.Controller.Fall();

        }


#elif UNITY_ANDROID || UNITY_IOS
         if (Input.touchCount == 1 && Input.GetTouch(0).position.x > Screen.width / 2)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchPosition = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (touchPosition != Vector2.zero)
                {

                    if ((touchPosition.y - Input.GetTouch(0).position.y) > 20f)
                    {
                       
                        this.Controller.Fall();
                        touchPosition = Vector2.zero;
                    }
                }
            }
        }
#endif
    }

    private void handleOtherGun()
    {
        if (((Input.GetButtonDown("Fire1") || Input.GetButtonDown("X")) || ((Input.GetButton("Fire1") || Input.GetButton("X")) && (CurrentWeapon.IsAutomatic == 0))) && attackRate <= 0)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("X")))
            {
                isAutoShooting = false;
            }
            else if (((Input.GetButton("Fire1") || Input.GetButton("X")) && (CurrentWeapon.IsAutomatic == 0)))
            {
                isAutoShooting = true;
            }

            attackRate = CurrentWeapon.Rate;
            if (!isAttacking)
            {
                isAttacking = true;
                AnimatorStateInfo asi = Animator.GetCurrentAnimatorStateInfo(1);
                if (asi.IsName("Attack Layer.attack"))
                {
                    Animator.Play("Attack Layer.attack", 1, 0);
                }
                else
                {

                    Animator.SetBool("Attacking", true);
                }
            }
        }
    }

    private void handleAttackInput()
    {
        attackRate -= Time.deltaTime;
#if  UNITY_EDITOR || UNITY_STANDALONE_WIN

        handleOtherGun();

#elif UNITY_ANDROID || UNITY_IOS
        
        
        if ( isPressGunAttack&& CurrentWeapon.IsAutomatic == 0 && attackRate <= 0)
        {
                attackRate = CurrentWeapon.Rate;
         
                if (!isAttacking)
                {
                    isAttacking = true;
                 
                    AnimatorStateInfo asi = Animator.GetCurrentAnimatorStateInfo(1);
                    if (asi.IsName("Attack Layer.attack"))
                    {
                        Animator.Play("Attack Layer.attack", 1, 0);
                    }
                    else
                    {
                        Animator.SetBool("Attacking", true);
                    }
                    
                }
            }
                  
        
     
#endif
    }

    private bool Flip(float dx)
    {

        Vector3 scale = Animator.transform.localScale;
        if (Mathf.Sign(dx) != Mathf.Sign(scale.x))
        {
            isFacingRight = scale.x > 0 ? false : true;
            Animator.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            return true;
        }

        return false;
    }

    public void Hurt(float damage)
    {
        this.Hurt(damage, true);
    }

    public void InstantKill()
    {
        CurrentHealth = 0;
        IsDead = true;
        IsControl = false;
        controller.SetHorizontalForce(0.0f);
        StartSlowMotion();
        addDeadEffect(transform.position);
        Animator.SetBool("Dead", true);

    }

    public void Hurt(float damage, bool isFacingRight)
    {

        if (IsDead)
            return;

        IsHurt = true;
        StartCoroutine(HurtEndCo());
        CurrentHealth -= damage;
        Director.Instance.HUD.UpdateHp(CurrentHealth);
        StartCoroutine(HurtColorChange());

        if (CurrentHealth > 0)
        {
            addHurtEffect(transform.position, isFacingRight);
        }
        else
        {
            IsDead = true;
            IsControl = false;
            controller.SetHorizontalForce(0.0f);
            this.boxCollider2D.enabled = false;
            StartSlowMotion();
            addDeadEffect(transform.position);
            Animator.SetBool("Dead", true);
            StartCoroutine(showRevive());
        }
    }

    private IEnumerator showRevive()
    {
        yield return new WaitForSeconds(1.5f);
        Director.Instance.ShowRevive();
    }

    private IEnumerator HurtEndCo()
    {
        yield return new WaitForSeconds(1.0f);
        IsHurt = false;
    }

    private IEnumerator HurtColorChange()
    {
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetFloat("_FlashAmount", 0.5f);
            MeshRenderers[i].materials[0].SetColor("_FlashColor", Color.red);
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetFloat("_FlashAmount", 0.3f);
        }
        yield return new WaitForSeconds(0.03f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetFloat("_FlashAmount", 0.15f);
        }
        yield return new WaitForSeconds(0.03f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetFloat("_FlashAmount", 0f);
        }
    }

    public void Revive()
    {
        Animator.SetBool("Dead", false);
        Animator.Play("Base Layer.GunMovement", 0, 0);
        IsDead = false;
        isAttacking = false;
        IsControl = true;
        this.boxCollider2D.enabled = true;
        CurrentHealth = MaxHealth;
        Director.Instance.HUD.UpdateHp(CurrentHealth);
        transform.position = Director.Instance.GetLastCheckPoint().position;
    }

    private void addDeadEffect(Vector3 pos)
    {
        if (DeadEffect != null)
        {
            GameObject Effect = Instantiate(DeadEffect, pos, Quaternion.identity) as GameObject;
            Effect.transform.parent = this.transform;
        }

    }

    private void StartSlowMotion()
    {
        Time.timeScale = 0.3f;
        StartCoroutine(slowMotionCo());
    }

    private IEnumerator slowMotionCo()
    {
        yield return new WaitForSeconds(0.8f);
        Time.timeScale = 1.0f;
    }

    public void PickupHp(float hp)
    {
        CurrentHealth += hp;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        Director.Instance.HUD.UpdateHp(CurrentHealth);
        if (AddHPEffect != null)
        {
            GameObject Effect = Instantiate(AddHPEffect, transform.position, Quaternion.identity) as GameObject;
            Effect.transform.parent = this.transform;
        }

    }

    public void PickUpKey(int count)
    {
        HasKey = true;
        BaseUI.Instance.AddKey();
    }

    public void PickupGold(int count)
    {
        gold += count;
        Director.Instance.HUD.UpdateGold(gold);
        if (AddGoldEffect != null)
        {
            GameObject Effect = Instantiate(AddGoldEffect, transform.position, Quaternion.identity) as GameObject;
            Effect.transform.parent = this.transform;
        }
    }

    public void PickupGun(BaseGun gun)
    {
        isAttacking = false;
        CurrentWeapon = gun;
        attackRate = 0;
        // 更新枪的贴图
        Gun_Mo.GetComponent<SkinnedMeshRenderer>().material = gun.GunMaterial;

        // 更新枪的UI
        BaseUI.Instance.UpdateUICurrentWeaponPicture(gun.Name);

        // 更新枪的射击声音
        SoundManager.Instance.PlayerShotSound = CurrentWeapon.ShootSound;
    }

    public void PressJump()
    {

        if (IsControl)
        {
            isPressJumping = true;
            if (controller.IsGrounded)
            {
                jumpCount = 2;
                jumpCount -= 1;
                controller.Jump();
                Animator.SetBool("Jumping", true);
            }
            else
            {

                if (jumpCount == 1)
                {
                    jumpCount -= 1;
                    controller.Jump();
                    Animator.Play("Base Layer.JumpTree", 0, 0);
                }
            }
        }
    }

    public void ReleaseJump()
    {
        isPressJumping = false;
    }

    public void PressAttack()
    {
        isPressGunAttack = true;


        if (isAttacking == false && CurrentWeapon.IsAutomatic == 1)
        {
            if (attackRate <= 0)
            {
                attackRate = CurrentWeapon.Rate;
                isAttacking = true;

                AnimatorStateInfo asi = Animator.GetCurrentAnimatorStateInfo(1);
                if (asi.IsName("Attack Layer.attack"))
                {
                    Animator.Play("Attack Layer.attack", 1, 0);
                }
                else
                {
                    Animator.SetBool("Attacking", true);
                }

            }
        }

    }

    public void ReleaseAttack()
    {
        isPressGunAttack = false;
        if (CurrentWeapon.Name == "FlameThrower")
        {
            isAttacking = false;
            Animator.SetBool("Flame", false);

        }
    }

    public void MoveForward()
    {
        horizontal = 1;
    }

    public void MoveBackward()
    {
        horizontal = -1;
    }

    public void Stop()
    {
        horizontal = 0;
    }

    private void slowDown()
    {
        if (!isSlowDown)
        {
            isSlowDown = true;
            StartCoroutine(stopSlowDown());
        }
    }

    IEnumerator stopSlowDown()
    {
        yield return new WaitForSeconds(0.4f);
        isSlowDown = false;
    }

    #region ANIMATION FRAME EVENT

    public void ShootBullet()
    {
        CurrentWeapon.ShootBullet(this.transform, isFacingRight, GunShootPoint, Util.ObjectOwner.PLAYER, true, controller.DeltaMovementX, isAutoShooting);
        StartCoroutine(CurrentWeapon.Reload());
    }

    public void ResetShootState()
    {

        isAttacking = false;
        Animator.SetBool("Attacking", false);
    }

    #endregion

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!IsHurt)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                BaseBullet bullet = collider.gameObject.GetComponent<BaseBullet>();
                if (bullet.Owner == Util.ObjectOwner.Zombie || bullet.Owner == Util.ObjectOwner.Soldier)
                {
                    Vector3 pos = bullet.transform.position;
                    bullet.DestoryWithEffect();
                    Hurt(bullet.Damage, bullet.IsFacingRight);

                }
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {

                BaseEntity entity = collider.gameObject.transform.GetComponent<BaseEntity>();
                if (entity != null && entity.CurrentHealth > 0)
                {
                    Hurt(entity.Damage);
                    if (entity.Type == BaseEntity.EnemyType.Crasher)
                    {
                        HitBack(entity.isFacingRight == true ? 10.0f : -10.0f, 10.0f, 0.5f);
                    }
                }
            }
        }


    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!IsHurt)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {

                BaseEntity entity = collider.gameObject.transform.GetComponent<BaseEntity>();
                if (entity != null && entity.CurrentHealth > 0)
                {
                    Hurt(entity.Damage);
                }
            }
        }


    }

    public void addHurtEffect(Vector3 pos, bool isFacingRight)
    {
        if (HurtEffect != null)
        {

            Quaternion randomRotation = Quaternion.Euler(0f, 0f, isFacingRight == true ? -180f : 0f);
            GameObject Effect = Instantiate(HurtEffect, pos, randomRotation) as GameObject;
            Effect.transform.parent = this.transform;
        }

    }

    public void MoveToRight()
    {
        Animator.SetFloat("Speed", 1);
        controller.SetHorizontalForce(SpeedAccelerationOnGround);

    }

    public void MoveToLeft()
    {
        Animator.SetFloat("Speed", 1);
        controller.SetHorizontalForce(-SpeedAccelerationOnGround);
    }

    public void StopMoving()
    {

        Animator.SetFloat("Speed", 0);
        controller.SetHorizontalForce(0.0f);
    }

    public void SaveHostage()
    {
        savedHostage += 1;
        Director.Instance.HUD.UpdateSavedHostage(savedHostage);
    }
}
