using UnityEngine;
using System.Collections;

public class LunaBug : MonoBehaviour {
	public enum bugType {GREEN,RED,YELLOW}
	public bugType color;
	private int heal = 5;
	public GameObject particles;
	public BoidFlocking flock;
	private GameObject playerPos;

	// Use this for initialization
	void Start () {
		playerPos = GameObject.FindGameObjectWithTag("PlayerSwapper");
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			useEffect();
			particles.SetActive(true);
			particles.transform.parent=null;
			Destroy(this.gameObject);
		}
	}

	void useEffect() {
		GameData d = GetUtil.getData();
		if (color == bugType.GREEN) {
			if(d.currentPlayer == GameData.player.Annie) d.annieCurrentLife+=heal;
			else if(d.currentPlayer == GameData.player.Emil) d.emilCurrentLife+=heal;
		}
		else if(color == bugType.RED) {
			if(d.currentPlayer == GameData.player.Annie) d.annieCurrentEnergy-=heal;
			else if(d.currentPlayer == GameData.player.Emil) d.emilCurrentEnergy+=heal;
		}
		else if(color == bugType.YELLOW) {
			if(d.currentPlayer == GameData.player.Annie) d.annieCurrentEnergy+=heal;
			else if(d.currentPlayer == GameData.player.Emil) d.emilCurrentEnergy-=heal;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Charge")&&playerPos!=null) {
			flock.enabled = false;
			float step = 20 * Time.deltaTime;
			Vector3 pos = playerPos.transform.position;
			pos.y+=1f;
			pos.x-=0.5f;
			transform.position = Vector3.MoveTowards(transform.position, pos, step);
		}
		else flock.enabled = true;
	}
}
