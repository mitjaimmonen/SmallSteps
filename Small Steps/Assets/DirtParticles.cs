using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtParticles : MonoBehaviour {

     private ParticleSystem.Particle[] m_Particles;
     public ParticleSystem m_System;  
     public Transform target;
	 public float gravity;
     private Vector3 targetDir;
 
     private void Start()
     {
         InitializeIfNeeded();
         m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
     }
 
     private void FixedUpdate()
     {
         // GetParticles is allocation free because we reuse the m_Particles buffer between updates
         int numParticlesAlive = m_System.GetParticles(m_Particles);
 
         // Change only the particles that are alive
         for (int i = 0; i < numParticlesAlive; i++)
         {
             //obtain the inverse direction of each particle and apply velocity towards the object
             targetDir = m_Particles[i].position - target.transform.position;
 
             m_Particles[i].velocity += targetDir * gravity * Time.fixedDeltaTime;
         }
		 var dir = (transform.position - target.transform.position);
		 transform.up = dir;
 
         // Apply the particle changes to the particle system
         m_System.SetParticles(m_Particles, numParticlesAlive);
     }
 
     private void InitializeIfNeeded()
     {
         if (m_System == null)
             m_System = GetComponent<ParticleSystem>();
 
         if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
             m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
     }
}
