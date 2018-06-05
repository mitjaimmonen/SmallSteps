using UnityEngine;
using XInputDotNetPure;
using System.Collections;

public class PlayerInputController : MonoBehaviour {

    public PlayerInput Current;
    float moveAxisV;
    float moveAxisH;
    float rotAxisV;
    float rotAxisH;
    [Range(0.01f, 0.99f), Tooltip("Deadzone for joystick movement (Left stick).")]
    public float movementDeadzone = 0.05f;
    [Range(0.01f, 0.99f), Tooltip("Deadzone for joystick rotation (Right stick).")]
    public float rotationDeadzone = 0.05f;
	// Use this for initialization
	void Start () {
        Current = new PlayerInput();
	}


    public void HandleInput(GamePadState state, GamePadState prevState)
    {

        HandleMove(state);
        HandleRotating(state); //Also basic attack

        // Retrieve our current WASD or Arrow Key input
        // Using GetAxisRaw removes any kind of gravity or filtering being applied to the input
        // Ensuring that we are getting either -1, 0 or 1
        Vector3 moveInput = HandleMove(state);
        // Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector2 rotInput = HandleRotating(state);
        // Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        bool attackInput = HandleAttack(state);

        bool jumpInput = HandleJump(state);

        Current = new PlayerInput()
        {
            MoveInput = moveInput,
            RotInput = rotInput,
            JumpInput = jumpInput,
            AttackInput = attackInput
        };

    }

    bool HandleAttack(GamePadState state)
    {
        return (state.Buttons.RightShoulder == ButtonState.Pressed);
        
    }

    bool HandleJump(GamePadState state)
    {
        return (state.Buttons.LeftShoulder == ButtonState.Pressed);
        
    }
    Vector3 HandleMove(GamePadState state)
    {
        moveAxisH = state.ThumbSticks.Left.X;
        moveAxisV = state.ThumbSticks.Left.Y;
        Vector3 dir = new Vector3(moveAxisH,0,moveAxisV);
        if (dir.magnitude < movementDeadzone)
            return Vector3.zero;
        return dir;
        
    }
    Vector2 HandleRotating(GamePadState state)
    {
        rotAxisH = state.ThumbSticks.Right.X;
        rotAxisV = state.ThumbSticks.Right.Y;
        Vector2 rot = new Vector3(rotAxisH,rotAxisV);
        if (rot.magnitude < rotationDeadzone)
            return Vector3.zero;
        return rot;
    }

	void Update () {
        

	}
}

public struct PlayerInput
{
    public Vector3 MoveInput;
    public Vector2 RotInput;
    public bool JumpInput;
    public bool AttackInput;    
}
