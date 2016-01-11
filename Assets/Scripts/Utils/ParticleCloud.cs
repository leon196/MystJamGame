using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleCloud : MonoBehaviour
{
	ParticleSystem system;
	ParticleSystem.Particle[] particleArray;

	void Start () {
		particleArray = new ParticleSystem.Particle[system.maxParticles]; 
		system = GetComponent<ParticleSystem>();
	}

	public void SetPointCloud (Vector3[] positionArray) 
	{
		system.Clear();
		system.Emit(positionArray.Length);
		system.GetParticles(particleArray);
		for (int i = 0; i < positionArray.Length; i++) {
			particleArray[i].position = positionArray[i];
		}
		system.SetParticles(particleArray, positionArray.Length);
	}
}