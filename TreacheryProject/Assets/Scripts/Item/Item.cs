﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Item : NetworkBehaviour {
	
	public string itemName;
	public Transform holdPos;
	public float mass = 1, drag = 0, angularDrag = 0.05f;
	public bool useGravity = true, isHeld;

	[ServerCallback]
	public void ServerUse(GameObject holder) {
		gameObject.SendMessage ("Use", holder);
	}

	public void Use(GameObject holder) {
		// Do something
	}

	public void Interact (GameObject actor)
	{
		if (!isHeld) {
			Inventory bag = actor.GetComponent<Inventory> ();
			if (bag != null) {
				bag.AttemptPickup (this);
			}
		}
	}

	public void DisablePhysics()
	{
		if (GetComponent<Rigidbody> () != null) {
			Destroy (GetComponent<Rigidbody> ());
			Destroy (GetComponent<NetworkTransform> ());
		}
	}
	
	public void EnablePhysics()
	{
		gameObject.AddComponent<Rigidbody> ();
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		gameObject.AddComponent<NetworkTransform> ();
		gameObject.GetComponent<NetworkTransform> ().transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
		rigidbody.mass = mass;
		rigidbody.drag = drag;
		rigidbody.angularDrag = angularDrag;
		rigidbody.useGravity = useGravity;
	}
}