using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {
	public int runningState;
	public int shootingState;
	public int taiyouState;
	public int hurtState;
	public int dyingState;
	public int limpingState;
	public int enemyWeaponTrigger;
	public int rollState;
	public int holdState;
	public int holdWeaponState;
	public int whistleState;
	public int comboState1;
	public int comboState2;
	public int comboState3;
	public int blockState;
	public int targetState;

	public int stepState;
	public int leapState;
	public int idleState;
	public int enemyHurtState;

	public int stillState;

	public int yesBool;
	public int idleBool;
	public int talkingBool;
	public int speedFloat;
	public int movingBool;
	public int taiyouBool;
	public int ankouBool;
	public int limpingBool;
	public int dyingTrigger;
	public int deadBool;
	public int shootingBool;
	public int rollingBool;
	public int pushingBool;
	public int holdGunBool;
	public int holdWeaponBool;
	public int shootingTrigger;
	public int slashingBool;
	public int targetingBool;
	public int attackingBool;
	public int whistleBool;
	public int comboInt;
	public int slashTrigger1;
	public int slashTrigger2;
	public int slashTrigger3;
	public int magicBool;

	public int walkingState;
	public int walkState;
	public int seekingState;
	public int alertState;
	public int attackState;
	public int pauseState;

	public int blockTrigger;
	public int playerDetectedBool;
	public int playerLostTrigger;
	public int attackBool;
	public int cautiousBool;
	public int hurtTrigger;
	public int pauseBool;
	public int chaseBool;
	public int stunnedBool;
	public int turningBool;
	public int frozenBool;
	public int spottedTrigger;
	public int whistleTrigger;

	public int slimeAttackTrigger;
	public int slimeHurtTrigger;
	public int slimeDeathTrigger;
	public int slimeDeadBool;

	public int slapTrigger;
	public int jumpTrigger;
	public int shootTrigger;
	public int rearBool;
	public int crashBool;

	public int unlockedBool;
	public int inRangeBool;

	public int tapTrigger;

	public int tipBool;
	public int appearTrigger;
	public int laughTrigger;
	public int tiltBool;
	public int raiseBool;
	public int shockTrigger;
	public int thinkingBool;
	public int tipTrigger;
	public int rollTrigger;

	//BOSS
	public int damageTakenBool;
	public int slitherBool;
	public int biteTrigger;
	public int circleBool;
	public int slitherState;


	void Awake () {

		//STATES FOR PLAYER
		runningState = Animator.StringToHash("Locomotion.Running");
		hurtState = Animator.StringToHash("Combat.Hurt");
		taiyouState = Animator.StringToHash("Charging.Taiyou");
		limpingBool = Animator.StringToHash("Combat.Limping");
		dyingState = Animator.StringToHash("Combat.Dying");
		shootingState = Animator.StringToHash("Combat.Shoot");
		rollState = Animator.StringToHash("Locomotion.Roll");
		holdState = Animator.StringToHash("Combat.HoldGun");
		holdWeaponState = Animator.StringToHash("Combat.HoldWeapon");
		whistleState = Animator.StringToHash("Combat.Whistle");
		comboState1 = Animator.StringToHash("Sword.Slash1");
		comboState2 = Animator.StringToHash("Sword.Slash2");
		comboState3 = Animator.StringToHash("Sword.Slash3");
		blockState = Animator.StringToHash("Targeting.Block");
		targetState = Animator.StringToHash("Targeting.Targeting");

		pauseState = Animator.StringToHash("Base Layer.Pause");
		stepState = Animator.StringToHash("Base Layer.Step");

		idleState = leapState = Animator.StringToHash("Base Layer.Idle");
		leapState = Animator.StringToHash("Base Layer.JumpLeap");
		stillState = Animator.StringToHash("Base Layer.Still");
		slitherState = Animator.StringToHash("Base Layer.Slither");
		
		//VARIABLES FOR PLAYER
		speedFloat = Animator.StringToHash("Speed");
		movingBool = Animator.StringToHash("Moving");
		taiyouBool = Animator.StringToHash("Taiyou");
		ankouBool = Animator.StringToHash("Ankou");
		limpingBool = Animator.StringToHash("Limping");
		dyingTrigger = Animator.StringToHash("Dying");
		deadBool = Animator.StringToHash("Dead");
		shootingBool = Animator.StringToHash("Shooting");
		slashingBool = Animator.StringToHash("Slashing");
		enemyWeaponTrigger = Animator.StringToHash("EnemyWeapon");
		rollingBool = Animator.StringToHash("Rolling");
		holdGunBool = Animator.StringToHash("HoldingGun");
		shootingTrigger = Animator.StringToHash("Shoot");
		targetingBool = Animator.StringToHash("Targeting");
		attackingBool = Animator.StringToHash("Attacking");
		pushingBool = Animator.StringToHash("Pushing");
		holdWeaponBool = Animator.StringToHash("HoldingWeapon");
		whistleBool = Animator.StringToHash("Whistling");
		comboInt = Animator.StringToHash("Combo");
		slashTrigger1 = Animator.StringToHash("ComboTrigger1");
		slashTrigger2 = Animator.StringToHash("ComboTrigger2");
		slashTrigger3 = Animator.StringToHash("ComboTrigger3");
		magicBool = Animator.StringToHash("Magic");
		blockTrigger = Animator.StringToHash("Block");

		//STATES FOR ENEMIES
		walkState = Animator.StringToHash("Base Layer.Walk");
		walkingState = Animator.StringToHash("Base Layer.Walking");
		seekingState = Animator.StringToHash("Base Layer.Seek");
		alertState = Animator.StringToHash("Base Layer.Alert");
		attackState = Animator.StringToHash("Base Layer.Attack");
		enemyHurtState = Animator.StringToHash("Base Layer.Hurt");

		//VARIABLES FOR ENEMIES
		attackBool = Animator.StringToHash("Attacking");
		cautiousBool = Animator.StringToHash("Cautious");
		playerDetectedBool = Animator.StringToHash("PlayerDetected");
		chaseBool = Animator.StringToHash("Chasing");
		movingBool = Animator.StringToHash("Moving");
		turningBool = Animator.StringToHash("Turning");
		frozenBool = Animator.StringToHash("Frozen");
		playerLostTrigger = Animator.StringToHash("PlayerLost");
		pauseBool = Animator.StringToHash("Pause");
		spottedTrigger = Animator.StringToHash("PlayerSpotted");
		rearBool = Animator.StringToHash("Rear");
		crashBool = Animator.StringToHash("Crashed");
		whistleTrigger = Animator.StringToHash("Whistle");
		stunnedBool = Animator.StringToHash("Stunned");
	
		slimeAttackTrigger = Animator.StringToHash("SlimeAttack");
		slimeDeathTrigger = Animator.StringToHash("SlimeDeath");
		slimeHurtTrigger = Animator.StringToHash("SlimeHurt");

		slapTrigger = Animator.StringToHash("Slam");
		shootTrigger = Animator.StringToHash("Shoot");
		jumpTrigger = Animator.StringToHash("Jump");

		//BOSS
		slitherBool = Animator.StringToHash("Moving");
		biteTrigger = Animator.StringToHash("Bite");
		hurtTrigger = Animator.StringToHash("Hurt");
		circleBool = Animator.StringToHash("InCircle");
		damageTakenBool = Animator.StringToHash("DamageTaken");

		//MISC
		unlockedBool = Animator.StringToHash("Unlocked");
		inRangeBool = Animator.StringToHash("PlayerInRange");
		tapTrigger = Animator.StringToHash("Tap");

		//CUTSCENEBOOLS;
		idleBool = Animator.StringToHash("Idle");
		talkingBool = Animator.StringToHash("Talking");
		tipTrigger = Animator.StringToHash("Tip");
		appearTrigger = Animator.StringToHash("AppearTrigger");
		tiltBool = Animator.StringToHash("TiltBool");
		laughTrigger = Animator.StringToHash("LaughTrigger");
		raiseBool = Animator.StringToHash("RaiseBool");
		shockTrigger = Animator.StringToHash("Shock");
		thinkingBool = Animator.StringToHash("ThinkingBool");
		rollTrigger = Animator.StringToHash("RollTrigger");
		yesBool = Animator.StringToHash("Yes");

	}
}
