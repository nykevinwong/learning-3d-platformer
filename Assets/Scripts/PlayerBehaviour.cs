using UnityEngine;
using System.Collections;
using CnControls;

public class PlayerBehaviour : MonoBehaviour {
	// Force to apply when player jumps
	public Vector2 jumpForce = new Vector2(0, 450);
	
	// How fast we'll let the player move in the x axis
	public float maxSpeed = 3.0f;
	
	// A modifier to the force applied
	public float speed = 50.0f;
	
	// The force to apply that we will get for the player's movement
	private float xMove;
	
	// Set to true when the player can jump
	private bool shouldJump;

	Rigidbody rigidbody;

	private bool onGround;
	private float yPrevious;

	private bool collidingWall;


	void Start ()
	{
		rigidbody = this.GetComponent<Rigidbody> ();
		shouldJump = false;
		xMove = 0.0f;

		onGround = false;
		yPrevious = Mathf.Floor(transform.position.y);

		collidingWall = false;
	}

	void FixedUpdate () 
	{
		// Move the player left and right
		Movement();
		
		// Sets the camera to center on the player's position.
		// Keeping the camera's original depth
		Camera.main.transform.position = new Vector3(transform.position.x, 
		                                             transform.position.y, 
		                                             Camera.main.transform.position.z);
	}

	void Update()
	{

		// Check if we are on the ground
		CheckGrounded();

		// Have the player jump if they press the jump button
		Jumping();
	}

	void Jumping()
	{
		if(CnInputManager.GetButtonDown("Jump")) 
		{
			shouldJump = true;
		}
		
		// If the player should jump
		if(shouldJump && onGround) 
		{
			rigidbody.AddForce(jumpForce);  
			shouldJump = false;
		}
	}

	void Movement()
	{
		//Get the player's movement (-1 for left, 1 for right, 0 for // none)
		xMove = CnInputManager.GetAxis("Horizontal");


		if(collidingWall && !onGround)
		{
			xMove = 0;
		}

		if(xMove != 0)
		{
			// Setting player horizontal movement
			float xSpeed = Mathf.Abs(xMove * rigidbody.velocity.x);
			
			if (xSpeed < maxSpeed) 
			{
				Vector3 movementForce = new Vector3(1,0,0);
				movementForce *= xMove * speed;

				RaycastHit hit;
				if(!rigidbody.SweepTest(movementForce, out hit, 0.05f))
				{
				  rigidbody.AddForce(movementForce);        
				}
			}
			
			// Check speed limit
			if (Mathf.Abs(rigidbody.velocity.x) > maxSpeed) 
			{
				Vector2 newVelocity;
				
				newVelocity.x = Mathf.Sign(rigidbody.velocity.x) * maxSpeed;
				newVelocity.y = rigidbody.velocity.y;
				
				rigidbody.velocity = newVelocity;
			}
		}
		else
		{
			// If we're not moving, get slightly slower
			Vector2 newVelocity = rigidbody.velocity;
			
			// Reduce the current speed by 10%
			newVelocity.x *= 0.9f;
			rigidbody.velocity = newVelocity;
		}
	}

	void CheckGrounded()
	{
		// Check if the player is hitting something from 
		// the center of the object (origin) to slightly below the 
		// bottom of it (distance)
		float distance = (GetComponent<CapsuleCollider>().height/2 * 
		                  this.transform.localScale.y) + .01f;
		Vector3 floorDirection = transform.TransformDirection(-Vector3.up);
		Vector3 origin = transform.position;
		
		if(!onGround)
		{
			// Check if there is something directly below us
			if (Physics.Raycast (origin, floorDirection, distance)) 
			{
				onGround = true;
			}
		}
		// If we are currently grounded, are we falling down or jumping?
		else if((Mathf.Floor(transform.position.y) != yPrevious)) 
		{
			onGround = false;
		}
		
		// Our current position will be our previous next frame
		yPrevious = Mathf.Floor(transform.position.y);
	}

	void OnDrawGizmos() 
	{
		if(transform!=null && rigidbody!=null)
		Debug.DrawLine(transform.position, transform.position + 
		               rigidbody.velocity, Color.red);
	}


	// If we hit something and we're not grounded, it must be a wall or // a ceiling. 
	void OnCollisionEnter(Collision collision)
	{ 
		if (!onGround)
		{ 
			collidingWall = true;  
		}
	}
	
	void OnCollisionExit(Collision collision)
	{
		collidingWall = false;
	}
}
