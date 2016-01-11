using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleCloud : MonoBehaviour
{
	ParticleSystem system;
	ParticleSystem.Particle[] particleArray;

	void Start () {
		system = GetComponent<ParticleSystem>();
		system.startLifetime = Mathf.Infinity;
		particleArray = new ParticleSystem.Particle[system.maxParticles]; 
	}

	public void SetPositionArray (Vector3[] positionArray) 
	{
		system.Clear();
		system.Emit(positionArray.Length);
		system.GetParticles(particleArray);
		for (int i = 0; i < positionArray.Length; i++) {
			particleArray[i].velocity = Vector3.zero;
			particleArray[i].lifetime = 9000f;
			particleArray[i].startLifetime = 9000f;
			particleArray[i].position = positionArray[i];
			particleArray[i].rotation = Random.Range(0f, 360f);
		}
		system.SetParticles(particleArray, positionArray.Length);
	}
}