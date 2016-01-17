using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
	public float size = 0.01f;
	public float speed = 1f;
	public float friction = 1f;

	public Vector3 position = Vector3.zero;
	public Vector3 target = Vector3.zero;
	public Vector3 velocity = Vector3.up;

	public float scaleCohesion = 1f;
	public float scaleSeparation = 1f;
	public float scaleAlignment = 1f;
	public float scaleTarget = 1f;

	Material material;

	void Start ()
	{
		material = GetComponent<Renderer>().material;
	}

	public void ApplyVelocity (Vector3 velocityChange)
	{
		velocity.x += velocityChange.x;
		velocity.y += velocityChange.y;
		velocity.z += velocityChange.z;

		position.x += velocity.x * speed;
		position.y += velocity.y * speed;
		position.z += velocity.z * speed;

		transform.position = position;

		// transform.LookAt(position + velocity);

		velocity.x *= friction;
		velocity.y *= friction;
		velocity.z *= friction;

		UpdateMaterial();
	}

	public void SetSize (float newSize)
	{
		size = newSize;
		transform.localScale = Vector3.one * size;
	}

	public void SetPosition (Vector3 newPosition)
	{
		position = newPosition;
		transform.position = position;
	}

	public void UpdateMaterial ()
	{
		material.SetFloat("_Range", Mathf.Min(1f, 0.1f * velocity.magnitude));
	}
}
