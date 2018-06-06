using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	    public float angle;
        public float distance;
        public Transform target;
		public float lerpSpeed = 10f;
		private Vector3 camPos, camDir;

        void LateUpdate()
        {
            float angle = Mathf.Deg2Rad * (this.angle);
            float horizontal = Mathf.Cos(angle) * distance;
            float vertical = Mathf.Sin(angle) * distance;

			camDir = -target.forward;
			Vector3 direction = new Vector3(camDir.x, 0, camDir.z);
			direction.Normalize();
			direction = direction * horizontal;

            camPos = target.position;			
			camPos += new Vector3(direction.x, vertical, direction.z);

            transform.position = Vector3.Lerp(transform.position, camPos, Time.deltaTime * lerpSpeed);
            transform.LookAt(target);
            var temp = Vector3.Cross(target.position-transform.position, transform.right);
            transform.rotation = Quaternion.LookRotation(target.position-transform.position, target.transform.up);

	}
}
