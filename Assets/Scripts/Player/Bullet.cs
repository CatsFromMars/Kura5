using UnityEngine;
using System.Collections;
using XInputDotNetPure;

//A simple projectile that shoots in a straight direcrion.

public class Bullet : MonoBehaviour {
	//BULET STAT VARS
	public string element = "Sol";
	public int damage = 5;
	public float velocity = 100f;
	public float stunTime = 0.5f;
	public float size = 0.5f;
	//elem appearence
	public Material solarBulletMat;
	public Material fireBulletMat;
	public Material earthBulletMat;
	
	public Transform elec;
	private bool hit = false;
	public float aliveTime = 1f;
	public SphereCollider col;
	public TrailRenderer trail;
	Rigidbody rb;
	
	//ACTIVE BULLET TIME
	private float elapsedTime;
	//BE SURE TO CHANGE THIS LATER. MAKE IT NOT PUBLIC.
	public Transform hitEffect;
	public Transform maxHitEffect;
	playerWeaponClass wep;

	void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	void OnEnable() {
		element = GameData.annieWeaponConfig.element;
		if(element == "Sol") bulletElem(solarBulletMat,solarBulletMat);
		else if(element == "Fire") bulletElem(fireBulletMat,fireBulletMat);
		else if(element == "Earth") bulletElem(earthBulletMat,earthBulletMat);
		wep = GameData.annieWeaponConfig;
		damage = wep.damage;
		velocity = 30 * wep.speed;
		//transform.localScale = new Vector3 (s,s,s);
		//trail.startWidth = s;
		elapsedTime = 0;
		col.enabled = true;
		hit = false;
	}
	
	void bulletElem(Material bulletMat, Material trailMat) {
		renderer.material = bulletMat;
		trail.material = trailMat;
	}
	
	void Update() {
		//if(!GetComponent<MeshRenderer>().renderer.isVisible) Destroy(this.gameObject);
		if(Time.timeScale == 0) {
			transform.position += transform.forward * velocity * Time.unscaledDeltaTime;
			elapsedTime+=Time.unscaledDeltaTime;
			if(elapsedTime >= aliveTime) DespawnBullet();
		}
	}
	
	void FixedUpdate() {
		//Debug.Log (elapsedTime);
		//transform.position += transform.forward * velocity * Time.deltaTime;
		rb.MovePosition (transform.position + transform.forward * velocity * Time.deltaTime);
		elapsedTime+=(Time.deltaTime);
		if(elapsedTime >= aliveTime) DespawnBullet();
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag=="Invisible") col.enabled=false;
		else if(!hit) {
			hit = true;
			if(collision.collider.tag == "Wall") {
				Instantiate(Resources.Load("Effects/Sound") as GameObject, transform.position, Quaternion.identity);
			}
			
			SpawnEffect();
			if(elec!=null)elec.parent = null;
			DespawnBullet();
		}
	}
	
	void SpawnEffect() {
		ShakeScreenAnimEvent.LittleShake();
		if(GameData.annieWeaponConfig.power > 4) {
			Instantiate(maxHitEffect, transform.position, Quaternion.identity);
		}
		else Instantiate(hitEffect, transform.position, Quaternion.identity);
	}
	
	void DespawnBullet() {
		gameObject.Recycle();
	}	
}