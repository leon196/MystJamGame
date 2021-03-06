﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public World spawnWorld;
	public Gate gateSpawn;
	[HideInInspector] public World currentWorld;
	[HideInInspector] public bool started = false;

	RaycastHit hit;
	Interface ui;
	Vector3 rayDirection;
	
	MouseLook mouseLook;
	Transition transition;
	FX fx;
	Voice voice;
	Transform audioListener;

	void Start () 
	{
		currentWorld = spawnWorld;
		this.transform.position = currentWorld.transform.position;
		rayDirection = new Vector3(0.5f, 0.5f, 1f);

		ui = GetComponent<Interface>();
		fx = GameObject.FindObjectOfType<FX>();
		voice = GameObject.FindObjectOfType<Voice>();
		
		transition = GameObject.FindObjectOfType<Transition>();
		mouseLook = GameObject.FindObjectOfType<MouseLook>();
		audioListener = GameObject.FindObjectOfType<AudioListener>().transform;

		StartCoroutine(transition.Intro(gateSpawn));
	}
	
	void Update () 
	{
		if (started) {
			audioListener.transform.rotation = Camera.main.transform.rotation;
			Ray ray = Camera.main.ViewportPointToRay(rayDirection);

			if (transition.isInTransition) {
				ui.SetCursorType(Interface.CursorType.Load);

			} else if (Physics.Raycast(ray, out hit, 10f)) {
				Interaction interaction = hit.transform.GetComponent<Interaction>();

				if (interaction != null) {

					bool click = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);

					// Gate
					if (interaction.GetType() == typeof(Gate)) {
						Gate gate = (Gate)interaction;

						// Not close enough of gate
						if (gate.transitionType != Gate.TransitionType.Fade && Camera.main.fieldOfView > mouseLook.maxFOV / 2f) {
							ui.SetCursorType(Interface.CursorType.Plus);
							if (click) { 
								mouseLook.Zoom(40f); 
	        			voice.Curiosity();
							}

						// Close enough of gate
						} else if (gate.isEnabled && (gate.anotherGate != null || gate.anotherWorld != null)) {

							// Sphere transition
							switch (gate.transitionType) {
								case Gate.TransitionType.Fall : ui.SetCursorType(Interface.CursorType.Portal); break;
								case Gate.TransitionType.Sphere : ui.SetCursorType(Interface.CursorType.Lock); break;
								default : ui.SetCursorType(Interface.CursorType.Step); break;
							}

							// Click on Gate
							if (click) {
								gate.Interact();
								StartCoroutine(transition.Goto(gate));

								if (gate.GetComponent<End>() != null) {
									StartCoroutine(transition.Thanks());
								}

								switch (gate.transitionType) {
									case Gate.TransitionType.Sphere : {
										voice.Fear();
										voice.Wow(5f);
										fx.Sphering();
										break;
									}
									case Gate.TransitionType.Fade : {
										fx.Step(); 
										break;
									}
									case Gate.TransitionType.Fall : {
										voice.Fear();
										voice.Wow(5f);
										fx.Falling();
										break;
									}
								}
							}
						} 

					// Book
					} else if (interaction.GetType() == typeof(Book)) {
						// Close enough of book
						if (Camera.main.fieldOfView < mouseLook.maxFOV / 2f) {
							ui.SetCursorType(Interface.CursorType.Use);
							// Click on book
							if (click) {
								interaction.Interact();
								fx.PageSwipe();
							}
						// Not close enough of book
						} else {
							ui.SetCursorType(Interface.CursorType.Plus);
							// Zoom
							if (click) {
								mouseLook.Zoom(40f);
	        			voice.Curiosity();
							}
						}

					// Page
					} else if (interaction.GetType() == typeof(Page)) {
						// Close enough of page
						if (Camera.main.fieldOfView < mouseLook.maxFOV / 2f) {
							ui.SetCursorType(Interface.CursorType.Use);
							// Click on page
							if (click) {
								Page page = (Page)interaction;
								StartCoroutine(page.Attach(Camera.main.transform));
								fx.PageTake();
	        			voice.Satisfaction();
								mouseLook.Zoom(80f);
							}
						// Not close enough of page
						} else {
							ui.SetCursorType(Interface.CursorType.Plus);
							// Zoom
							if (click) {
								mouseLook.Zoom(40f);
	        			voice.Curiosity();
							}
						}

					// Statue
					} else if (interaction.GetType() == typeof(Statue)) {
						ui.SetCursorType(Interface.CursorType.Use);

						if (click) {
							interaction.Interact();
							fx.Catching();
						}

					// Default
					} else {
						ui.SetCursorType(Interface.CursorType.Use);

						if (click) {
							interaction.Interact();
						}
					}

				// No interaction
				} else {
					ui.SetCursorType(Interface.CursorType.Look);
				}

			// No interaction and no collision
			} else {
				ui.SetCursorType(Interface.CursorType.Look);
			}
		}
	}
}
