using UnityEngine;
using System.Collections;

public class Veins : MonoBehaviour {
	private float delta = 0.0541642828373f;
	private float shrinkSpeed = 5f;
	public SkinnedMeshRenderer renderer;
	public Ectoplasm ecto;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
	}
	

	public IEnumerator toggleVeins(Transform target, bool detracting=false) {
		if(target!=null) {
			Vector3 pos = target.transform.position;
			pos.z = pos.z+1.5f;
			transform.LookAt(pos);
		}
		renderer.enabled = true;
		float scale = 0;
		if(!detracting) scale = Vector3.Distance(transform.position, target.transform.position)*delta;
		Debug.Log ("Scale is: "+scale);
		Vector3 newScale = new Vector3 (1, 1, scale);
		while(!Mathf.Approximately(transform.localScale.z,newScale.z)) {
			transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime*shrinkSpeed);
			yield return null;
		}
		if(detracting) renderer.enabled = false;
		yield return null;
	}
	
	// Update is called once per frame
	void Update () {
		if(ecto.state != Ectoplasm.ectoState.ATTACK) {
			StopAllCoroutines();
			StartCoroutine(toggleVeins(null,true));
		}
	}
}
