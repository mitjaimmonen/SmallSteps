using UnityEngine;
using System.Collections;

/*
 * Example implementation of the SuperStateMachine and SuperCharacterController
 */
[RequireComponent(typeof(SuperCharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMachine : SuperStateMachine {

    [FMODUnity.EventRef] public string chargeSound, swingSound, burstSound, floatSound, walkSound;
    FMOD.Studio.EventInstance floatSoundEI, walkSoundEI;

    public ParticleSystem jetpackParticles;
    public ParticleSystem dirtParticles;
    public Transform AnimatedMesh;
    public PlayerCamera playerCamera;
    public Animator anim;
    public Transform planet;

    public float WalkSpeed = 4.0f;
    public float RotSpeed = 5f;
    public float WalkAcceleration = 1000.0f;
    public float moveFriction = 1000f;
    public float JumpAcceleration = 100.0f;
    public float JumpHeight = 3.0f;
    public float Gravity = 25.0f;

    bool doubleJumped = false, prevJumpInput, attacking = false;

    // Add more states by comma separating them
    enum PlayerStates { Idle, Walk, Jump, Fall }

    private SuperCharacterController controller;
    private NetCatcher netCatcher;

    // current velocity
    private Vector3 moveDirection;
    // current direction our character's art is facing
    public Vector3 lookDirection { get; private set; }

    private Vector3 lastPos, currentPos;

    private PlayerInputController input;


    [FMODUnity.EventRef] public string deathSound, damageSound;
    bool destroying;
    Health health;
    public float maxHealth;
    public GameObject takeDamageParticles;


    public bool IsAttacking
    {
        get {return attacking; }
        set
        {
            attacking = value;
            netCatcher.isAttacking = attacking;

        }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
    }
    public float CurrentHealth
    {
        get { return health.CurrentHealth; }
    }
    public bool IsAlive
    {
        get { return health.isAlive(); }
    }

    
    void Awake()
    {
        health = new Health(maxHealth);
        
    }

	void Start () {
	    // Put any code here you want to run ONCE, when the object is initialized

        input = gameObject.GetComponent<PlayerInputController>();

        // Grab the controller object from our object
        controller = gameObject.GetComponent<SuperCharacterController>();
		
		// Our character's current facing direction, planar to the ground
        lookDirection = transform.forward;

        // Set our currentState to idle on startup
        currentState = PlayerStates.Idle;
        netCatcher = GetComponentInChildren<NetCatcher>();
        if(!planet)
        {
            var temp = GetComponentInChildren<Gravity>();
            planet = temp.planet;
        }
	}


    void PlayCharge()
    {
        //play charge sound
    }
    void PlaySwing()
    {
        //Play swing sound
    }
    public void AttackEnd()
    {
        IsAttacking = false;
    }

    public void GetHit(float dmgValue)
    {

        if (health.isAlive())
        {
            FMODUnity.RuntimeManager.PlayOneShot(damageSound, transform.position);
        }

        health.TakeDamage(dmgValue);
        Debug.LogWarning("THE PLAYER IS GETTING HIT OUCH PAIN! Health is:" + health.CurrentHealth);

        if (takeDamageParticles)
        {
            Quaternion rot = transform.rotation;
            rot.y = Random.Range(0, 360);
            GameObject temp = Instantiate(takeDamageParticles, transform.position, rot);
            Destroy(temp, 2f);
        }




        if (!health.isAlive())
            DIE();

    }
    void DIE()
	{
		//Play dead animation
		if (!destroying)
		{
			if (deathSound != "")
				FMODUnity.RuntimeManager.PlayOneShot(deathSound, transform.position);
			destroying = true;
		}

	}


    protected override void EarlyGlobalSuperUpdate()
    {
		// Rotate out facing direction horizontally based on mouse input
        lookDirection = Quaternion.AngleAxis(input.Current.RotInput.x * RotSpeed * 100f * Time.deltaTime, controller.up) * lookDirection;
        // Put any code in here you want to run BEFORE the state's update function.
        // This is run regardless of what state you're in

        if (anim)
        {
            if (input.Current.AttackInput && !attacking)
            {
                if (!anim.GetBool("charge"))
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached(chargeSound, this.gameObject);
                    anim.SetBool("charge", true);
                    
                }
            }
            else if (!input.Current.AttackInput && anim.GetBool("charge"))
            {
                anim.SetBool("charge", false);
                
                if (!IsAttacking)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached(swingSound, this.gameObject);
                    IsAttacking = true;
                }

            }
        }

    }

    protected override void LateGlobalSuperUpdate()
    {
        // Put any code in here you want to run AFTER the state's update function.
        // This is run regardless of what state you're in

        // Move the player by our velocity every frame
        lastPos = transform.position;
        transform.position += moveDirection * controller.deltaTime;
        currentPos = transform.position;

        // Rotate our mesh to face where we are "looking"
        AnimatedMesh.rotation = Quaternion.LookRotation(lookDirection, controller.up);
    }

    private bool AcquiringGround()
    {
        return controller.currentGround.IsGrounded(false, 0.01f);
    }

    private bool MaintainingGround()
    {
        return controller.currentGround.IsGrounded(true, 0.5f);
    }

    public void RotateGravity(Vector3 up)
    {
        lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
    }

    /// <summary>
    /// Constructs a vector representing our movement local to our lookDirection, which is
    /// controlled by the camera
    /// </summary>
    private Vector3 LocalMovement()
    {
        Vector3 right = Vector3.Cross(controller.up, lookDirection);

        Vector3 local = Vector3.zero;

        if (input.Current.MoveInput.x != 0)
        {
            local += right * input.Current.MoveInput.x;
        }

        if (input.Current.MoveInput.z != 0)
        {
            local += lookDirection * input.Current.MoveInput.z;
        }

        return local;
    }

    // Calculate the initial velocity of a jump based off gravity and desired maximum height attained
    private float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

	/*void Update () {
	 * Update is normally run once on every frame update. We won't be using it
     * in this case, since the SuperCharacterController component sends a callback Update 
     * called SuperUpdate. SuperUpdate is recieved by the SuperStateMachine, and then fires
     * further callbacks depending on the state
	}*/

    // Below are the three state functions. Each one is called based on the name of the state,
    // so when currentState = Idle, we call Idle_EnterState. If currentState = Jump, we call
    // Jump_SuperUpdate()
    void Idle_EnterState()
    {
        controller.EnableSlopeLimit();
        controller.EnableClamping();
        if (!dirtParticles.isEmitting)
            dirtParticles.Play();
                
    }

    void Idle_SuperUpdate()
    {
        // Run every frame we are in the idle state
        floatSoundEI.setParameterValue("isFloating", 0);
        
        if (jetpackParticles.isPlaying)
            jetpackParticles.Stop();

        if (dirtParticles.isEmitting)
            dirtParticles.Stop();

        if (!input.Current.JumpInput)
            prevJumpInput = false;
        

        if (input.Current.JumpInput && !prevJumpInput)
        {
            currentState = PlayerStates.Jump;
            return;
        }

        if (!MaintainingGround())
        {
            currentState = PlayerStates.Fall;
            return;
        }

        if (input.Current.MoveInput != Vector3.zero)
        {
            currentState = PlayerStates.Walk;
            return;
        }

        // Apply friction to slow us to a halt
        moveDirection = Vector3.MoveTowards(moveDirection, Vector3.zero, moveFriction * controller.deltaTime);
    }

    void Idle_ExitState()
    {
        // Run once when we exit the idle state
        if (!dirtParticles.isEmitting)
            dirtParticles.Play();
                
    }

    void Walk_SuperUpdate()
    {
        floatSoundEI.setParameterValue("isFloating", 0);

        if (jetpackParticles.isPlaying)
            jetpackParticles.Stop();

        FMOD.Studio.PLAYBACK_STATE playbackState;
        walkSoundEI.getPlaybackState(out playbackState);
        if (!walkSoundEI.isValid() || playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            walkSoundEI = FMODUnity.RuntimeManager.CreateInstance(walkSound);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkSoundEI, transform, GetComponent<Rigidbody>());
            walkSoundEI.start();
        }
        if (!dirtParticles.isEmitting)
            dirtParticles.Play();

        if (!input.Current.JumpInput)
        {
            prevJumpInput = false;            
        }
            
        if (input.Current.JumpInput && !prevJumpInput)
        {
            currentState = PlayerStates.Jump;
            return;
        }

        if (!MaintainingGround())
        {
            currentState = PlayerStates.Fall;
            return;
        }

        if (input.Current.MoveInput != Vector3.zero)
        {
            moveDirection = Vector3.MoveTowards(moveDirection, LocalMovement() * WalkSpeed, WalkAcceleration * controller.deltaTime);
        }
        else
        {
            currentState = PlayerStates.Idle;
            return;
        }
    }

    void Jump_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();
        doubleJumped = false;
        prevJumpInput = true;

        moveDirection += controller.up * CalculateJumpSpeed(JumpHeight, Gravity);
    }

    void Jump_SuperUpdate()
    {
        if (dirtParticles.isEmitting)
            dirtParticles.Stop();
        if (input.Current.JumpInput)
        {
            if (input.Current.JumpInput && !prevJumpInput)
            {
                if (!doubleJumped)
                {
                    //Do doublejump burst???
                    Debug.Log("Double jump?");
                    FMODUnity.RuntimeManager.PlayOneShotAttached(burstSound, this.gameObject);
                    doubleJumped = true;
                    // prevJumpInput = true;
                    if (!jetpackParticles.isPlaying)
                        jetpackParticles.Play();
                    moveDirection += controller.up * CalculateJumpSpeed(JumpHeight, Gravity);
                }
                // Debug.Log("Double jump hover thingy?");
                //Just hover maybe?!?!?!?

                FMOD.Studio.PLAYBACK_STATE playbackState;
                floatSoundEI.getPlaybackState(out playbackState);
                if (!floatSoundEI.isValid() || playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
                {
                    Debug.Log(playbackState + ", isValid: " + floatSoundEI.isValid());
		            floatSoundEI = FMODUnity.RuntimeManager.CreateInstance(floatSound);
                    floatSoundEI.setParameterValue("isFloating", 1);
		            floatSoundEI.start();

 			        FMODUnity.RuntimeManager.AttachInstanceToGameObject(floatSoundEI, GetComponent<Transform>(), GetComponent<Rigidbody>());
                }
                else
                    floatSoundEI.setParameterValue("isFloating", 1);

                if (!jetpackParticles.isPlaying)
                    jetpackParticles.Play();
                var vertical =(moveDirection - (Math3d.ProjectVectorOnPlane(controller.up, moveDirection))).magnitude;
                float lastMagnitude = (lastPos - planet.position).magnitude;
                float currentMagnitude = (currentPos - planet.position).magnitude;
                if (lastMagnitude > currentMagnitude && vertical >3f)
                    moveDirection += controller.up * CalculateJumpSpeed(Time.deltaTime*0.25f, Gravity);
                     
            }
            
        }
        Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);
        Vector3 verticalMoveDirection = moveDirection - planarMoveDirection;

        if (Vector3.Angle(verticalMoveDirection, controller.up) > 90 && AcquiringGround())
        {
            moveDirection = planarMoveDirection;
            currentState = PlayerStates.Idle;
            prevJumpInput = true;
            
            return;            
        }
        if (!input.Current.JumpInput)
        {
            floatSoundEI.setParameterValue("isFloating", 0);
            jetpackParticles.Stop();
            prevJumpInput = false;
            
        }
       

        planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, LocalMovement() * WalkSpeed, JumpAcceleration * controller.deltaTime);
        verticalMoveDirection -= controller.up * Gravity * controller.deltaTime;

        moveDirection = planarMoveDirection + verticalMoveDirection;
        
    }

    void Fall_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();
        floatSoundEI.setParameterValue("isFloating", 0);
        prevJumpInput = true;

        // moveDirection = trueVelocity;
    }

    void Fall_SuperUpdate()
    {
        if (AcquiringGround())
        {
            if (!dirtParticles.isEmitting)
                dirtParticles.Play();

            moveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);
            currentState = PlayerStates.Idle;
            prevJumpInput = true;
            return;
        }

        moveDirection -= controller.up * Gravity * controller.deltaTime;
    }
}
