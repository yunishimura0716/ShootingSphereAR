using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyClass
{
    public class Sphere : MonoBehaviour
    {
        // how much it grows each time
        public float scaleFactor = 1.09f;

        // maximum scale
        public float maxScale = 0.7f;

        [SerializeField]
        public SphereController sphereController;

        // when sphere is attacked
        public void Attacked()
        {
            // increase the scale
            transform.localScale *= scaleFactor;

            // check if we've passed the maxScale
            if (transform.localScale.x >= maxScale) {
                sphereController.Explosion();
            }
        }
    }    
}
