using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidManager : MonoBehaviour 
{
  List<Boid> boidList;

	void Start () 
  {
    boidList = new List<Boid>();
	}
  
  public void AddBoid (GameObject boidGameObject, Vector3 target)
  {
    Boid boid = boidGameObject.AddComponent<Boid>();
    
    boid.SetPosition(boidGameObject.transform.position);
    boid.target = target;
    
    boid.scaleCohesion = 0.0001f;
    boid.scaleSeparation = 0.008f;
    boid.scaleAlignment = 0.0001f;
    boid.scaleTarget = 0.01f;
    
    boid.friction = 0.1f;
    
    boidList.Add(boid);
  }
	
	void Update () 
  {
    for (int current = 0; current < boidList.Count; ++current)
    {
      Boid boid = boidList[current];
  
      Vector3 vectorCohesion = Vector3.zero;
      Vector3 vectorSeparation = Vector3.zero;
      Vector3 vectorAlignment = Vector3.zero;
      Vector3 vectorTarget = boid.target - boid.position;
      
      int countAlignment = 0;
    
      for (int other = 0; other < boidList.Count; ++other) 
      {
        if (current != other) 
        {
          Boid boidOther = boidList[other];
          
          float distance = Vector3.Distance(boid.position, boidOther.position) - (boid.size + boidOther.size);
          if (distance < 1f)
          {
            vectorSeparation += Vector3.Normalize(boid.position - boidOther.position) * distance;
          }
          
          if (distance < 1f)
          {
            vectorAlignment += boidOther.velocity;
            ++countAlignment;
          }
          
          vectorCohesion += boidOther.position - boid.position;
        }
      }
      
      vectorCohesion = vectorCohesion / Mathf.Max(boidList.Count - 1, 1f);

      if (countAlignment != 0) 
      {
        vectorAlignment = vectorAlignment / countAlignment;
      }
      
      boid.ApplyVelocity(
        vectorCohesion * boid.scaleCohesion
        + vectorSeparation * boid.scaleSeparation
        + vectorAlignment * boid.scaleAlignment
        + vectorTarget * boid.scaleTarget);
    }
	}
}
