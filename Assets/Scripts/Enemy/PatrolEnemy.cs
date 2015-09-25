using UnityEngine;
using System.Collections;

public class PatrolEnemy : EnemyClass {

	//MOVEMENT VARIABLES
	public Transform[] wayPoints;
	private int currentPoint = 0;
	public float enemySpeed = 1f;
	public float enemyRotationSpeed = 4f;
	public float enemyWalkingSpeed = 0.8f;
	public float enemyRunningSpeed = 1f;

	//ACTION VARIABLES
	public int cautionTimer = 0;
	public int cautionWaitTime = 100;
	public int stunTimer = 0;
	public int stunWaitTime = 30;
	public int seekTimer = 0;
	public int seekWaitTime = 100;
	public int attackTimer = 0;
	public int attackWaitTime = 200;
	public int pauseTimer = 0;
	public int pauseWaitTime = 100;
	public int patrolTimer = 0;
	public int patrolWaitTime = 100;
	protected bool pausing = false;
	protected bool moving = false;
	public bool attacking = false;
	protected bool chasing = false;
	protected bool canMakeDecision = true;

	//VISUAL VARIABLES
	public Transform emotion;
	public ParticleSystem smoke;

	// Use this for initialization
	void Start () {
		transform.position = wayPoints[0].transform.position;
	}

	protected void updateAnimations() {
		//Put this at the top of the enemy's update function
		animator.SetBool(hash.pauseBool, pausing);
		animator.SetBool(hash.movingBool, moving);
		animator.SetBool(hash.attackBool, attacking);
		animator.SetBool(hash.chaseBool, chasing);
		animator.SetBool(hash.stunnedBool, stunned || frozen);
	}

	protected void manageMovement() {
		//Manage agent based on anim
		bool canMove = (animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.runningState ||
		                animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.seekingState ||
		                animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.walkingState ||
		                animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.walkState);
		if(canMove) {
			agent.Resume();
			agent.updateRotation = true;
		}
		else { 
			agent.Stop();
			agent.updateRotation = false;
		}
	}

	#region Detection
	void OnTriggerStay (Collider other) {
		
		if(other.gameObject.tag == "Player") {
			trackingPlayer = true;
			playerPos = other.transform.position;
			seePlayer(other); //DETECT PLAYER THROUGH SIGHT
			hearPlayer(other); //DETECT PLAYER THROUGH SOUND
		}
		
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player") trackingPlayer = false;
	}

	void hearPlayer(Collider other) {
		if(player == null) { //IN CASE PLAYER SWITCHES
			player = GameObject.FindWithTag("Player").transform;
			playerAnimator = player.GetComponent<Animator>();
		}
		
		if (playerAnimator.GetCurrentAnimatorStateInfo(0).nameHash == hash.whistleState) {
			if(animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.idleState ||
			   animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.walkState) animator.SetTrigger(hash.whistleTrigger);
			agent.Stop();
			playerLastSighting = playerPos;
		}
		
	}

	void seePlayer(Collider other) {
		// Create a vector from the enemy to the player and store the angle between it and forward.
		Vector3 direction = other.transform.position - transform.position;
		float angle = Vector3.Angle(direction, transform.forward);
		if (angle < fieldOfViewAngle * 0.5f) {
			RaycastHit hit;

			// ... and if a raycast towards the player hits something...
			if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
			{
				Debug.DrawLine(transform.position, hit.point, Color.red);
				// ... and if the raycast hits the player...
				if (hit.collider.gameObject.tag == "Player")
				{
					// ... the player is in sight.
					playerInSight = true;
					// Set the last global sighting is the players current position.
					if(player == null) { //IN CASE PLAYER SWITCHES
						player = GameObject.FindWithTag("Player").transform;
						playerAnimator = player.GetComponent<Animator>();
					}
					playerLastSighting = player.transform.position;
					cautionTimer = 0; //Reset caution timer since player's busted
				}
				else playerInSight = false;
			}
			else playerInSight = false;
		}
		else playerInSight = false;
	}
	
	void LoseSuspicion() {
		//Lose the player by resetting variables...
		playerLastSighting = resetPlayerPosition;
		agent.speed = enemySpeed;
		animator.SetTrigger(hash.playerLostTrigger);
		attacking = false;
		chasing = false;
	}
	#endregion

	#region Movement
	public bool ArrivedAtPoint() {
		if (agent.remainingDistance <= agent.stoppingDistance) {
			if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
				return true;
			}
			else return false;
		}
		else return false;
	}
	
	public void MoveTowardsTarget(Vector3 target) {
		//NAVMESH
		moving = true;
		agent.updateRotation = true;
		agent.SetDestination(target);
	}
	
	protected void Patrol()
	{
		//Debug.Log ("PATROL");
		// Set an appropriate speed for the NavMeshAgent.
		MoveTowardsTarget(wayPoints[currentPoint].position);
		agent.speed = enemyWalkingSpeed;
		// If near the next waypoint or there is no destination...
		if(agent.remainingDistance < agent.stoppingDistance)
		{
			moving = false; //Tell animator that we're pausing
			agent.updateRotation = false;
			// ... increment the timer.
			patrolTimer++;
			// If the timer exceeds the wait time...
			if(patrolTimer >= patrolWaitTime)
			{
				// ... increment the currentPoint.
				if(currentPoint == wayPoints.Length - 1)
					currentPoint = 0;
				else
					currentPoint++;
				// Reset the timer.
				patrolTimer = 0;
			}
		}
		// Set the destination to the patrolWayPoint.
		//else MoveTowardsTarget(wayPoints[currentPoint].position);
		
	}
	
	public void Seek() {
		//decide: attack or chase?
		//attack is followed by a pause
		//Chase goes on until player is spotted or player is lost
		//Debug.Log ("SEEKING YOU");
		chasing = true;
		if(playerInSight) Attack();
		else Chase();
	}

	public void Chase() {
		agent.speed = enemyRunningSpeed;
		moving = true;
		MoveTowardsTarget(playerLastSighting);
		if(ArrivedAtPoint()) {
			Caution();
			moving = false;
		}
	}
	
	public void Caution() {
		//Look backwards when arrived at playerLastSpotted sight
		//At least supposed to. Do this later
		//Debug.Log ("CAUTION");
		chasing = false;
		if (cautionTimer >= cautionWaitTime) {
			LoseSuspicion();
			cautionTimer = 0;
		}
		else cautionTimer++;
	}

	public void Pause() {
		//Idle and do nothing for pauseWaitTime seconds. FOR ATTACK ONLY!!!!
		attacking = false;
		if(pauseTimer >= pauseWaitTime) {
			pausing = false;
			//attacking = false;
			pauseTimer = 0;
		}
		else {
			pauseTimer++;
			moving = false;
			agent.Stop();
		}
	}
	#endregion

	#region Combat
	protected void Attack() {
		//To be overriten by Class if needed
		attacking = true;
		pausing = true;
		agent.Stop();
		moving = false;
		hitCounter = 0;
	}

	public void Stunned() {
		if (hitCounter % 4 == 0) {
			if(stunTimer >= stunWaitTime) {
				stunned = false;
				stunTimer = 0;
			}
			else {
				stunTimer++;
				stunned = true;
			}
		}
		else {
			stunned = false;
		}

	}

	public void Freeze() {
		//Handles Emil's shadow stunning;
		if(!attacking && animator.GetCurrentAnimatorStateInfo(0).nameHash != hash.attackState) {
			if(frozen == false) { 
				freezeWaitTime = lightLevels.darkness*15;
				shadowStunParticles.Play();
			}
			if(freezeTimer >= freezeWaitTime) {
				frozen = false;
				freezeTimer = 0;
				shadowStunEffect.active = false;
				shadowStunParticles.Stop ();
			}
			else {
				shadowStunEffect.active = true;
				freezeTimer++;
				frozen = true;
			}
		}
	}
	
	#endregion
	
	//void DisplayEmoticon() {
		//For Alert and Notice (?)
	//}

	public void handleBurning() {
		if(sunDetector.sunlight > 0) {
			burnCounterTime = 5 * 1/sunDetector.sunlight;
			takeSunDamage(burnRate);
			if(!smoke.isPlaying) smoke.Play();
			stunned = true;
		}
		else {
			if (smoke.isPlaying) {
				smoke.Stop();
				//stunned = false;
			}
		}
	}
	
	void takeSunDamage(float rate) {
		burnCounter++;
		if(burnCounter >= burnCounterTime) {
			currentLife -= rate;
			burnCounter = 0f;
		}
		
		if(currentLife <= 0) Die(); //KILL PLAYER IF GAME OVER.	
	}

	void DisplayEmoticon(GameObject emoticon) {
		emoticon = Instantiate(emoticon, emotion.transform.position, Quaternion.identity) as GameObject;
	}

}
