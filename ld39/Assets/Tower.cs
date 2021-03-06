﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
	Queue<GameObject> targetList = new Queue<GameObject> ();
	GameObject target = null;
	public float bulletSpeed = 6f;
	private bool alreadyFired = false;
	public int power;
	public int startpower;
	public int maxpower;
	//int powhold;
	//public GameObject HOLD;
	// Use this for initialization
	void Start () {
		power = startpower;
		// bullet.transform.position = transform.position;
	}
	IEnumerator waitForNextShoot() {
		yield return new WaitForSeconds(1f);
		alreadyFired = false;
		Debug.Log ("ready to fire");

	}
	// Update is called once per frame
	void Update () {
		if (power <= 0) {
			Debug.Log ("Reloading");
		}
		else{
			
				if (targetList.Count > 0) {
			var grunt = targetList.Peek ();
			try {
				if (!grunt.GetComponent<Grunt>().inRange) {
					targetList.Dequeue ();
				}
				else {
					if (targetList.Count > 0 && !alreadyFired) {
						target = targetList.Dequeue ();
						alreadyFired = true;
						StartCoroutine (waitForNextShoot ());
							power--;
						GameObject bullet = (GameObject)Instantiate(Resources.Load("prefab/bullet"), GetComponent<Transform>().position, GetComponent<Transform>().rotation) ;

						Debug.Log ("firing");

						bullet.GetComponent<Rigidbody2D> ().velocity = ShootUtil.firingVector (transform, target, bulletSpeed);

					}				
				}				
			}
			catch (MissingReferenceException e) {
				targetList.Dequeue ();
			}

		}
		}
		// (power > maxpower) {
			//powhold = power - maxpower;
			//GameObject.Find (HOLD).GetComponent<> ();
		//}

	}
	void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.tag == "Grunt") {
            Debug.Log("in range");
		    targetList.Enqueue (coll.gameObject);
			coll.gameObject.GetComponent<Grunt> ().inRange = true;
        }
		if (coll.gameObject.tag == "wind") {
			power++;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "Grunt") {
			Debug.Log("out of range");
			coll.gameObject.GetComponent<Grunt> ().inRange = true;
		}
	}

}
