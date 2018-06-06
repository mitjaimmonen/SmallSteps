using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class Player : MonoBehaviour {

 	public float movementSpeed;
    [Range(0.01f, 0.99f), Tooltip("Deadzone for joystick movement (Left stick).")]
    public float movementDeadzone = 0.05f;
    [Range(0.01f, 0.99f), Tooltip("Deadzone for joystick rotation (Right stick).")]
    public float rotationDeadzone = 0.05f;
    public CapsuleCollider playerCollider;

    float moveAxisV;
    float moveAxisH;
    float rotAxisV;
    float rotAxisH;


    private void Awake()
    {

    }

	private void Update()
	{
		//Do gravity shit.
	}


    public void HandleInput(GamePadState state, GamePadState prevState)
    {

		HandleMove(state);
		HandleRotating(state); //Also basic attack
		HandleJumping(state, prevState);

        
    }

    void HandleMove(GamePadState state)
    {
        moveAxisH = state.ThumbSticks.Left.X;
        moveAxisV = state.ThumbSticks.Left.Y;




        if (moveAxisH > movementDeadzone || moveAxisH < -movementDeadzone)
        {
            //Movement
            float magnitude = moveAxisH < 0 ? moveAxisH + movementDeadzone : moveAxisH - movementDeadzone;
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right * magnitude, Time.deltaTime * movementSpeed);

        }
        if (moveAxisV > movementDeadzone || moveAxisV < -movementDeadzone)
        {
            float magnitude = moveAxisV < 0 ? moveAxisV + movementDeadzone : moveAxisV - movementDeadzone;
            var cameraforw = Vector3.forward;

            cameraforw.y = 0;
            cameraforw.Normalize();
            transform.position = Vector3.Lerp(transform.position, transform.position + cameraforw * magnitude, Time.deltaTime * movementSpeed);


        }
    }
    void HandleRotating(GamePadState state)
    {
        rotAxisH = state.ThumbSticks.Right.X;
        rotAxisV = state.ThumbSticks.Right.Y;

        if (rotAxisH > rotationDeadzone || rotAxisH < -rotationDeadzone || rotAxisV > rotationDeadzone || rotAxisV < -rotationDeadzone)
        {
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(rotAxisH, rotAxisV) * 180 / Mathf.PI, 0);
        }
    }


	void HandleJumping(GamePadState state, GamePadState prevState)
	{

	}

}