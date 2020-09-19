using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  MyClass
{
    public class SphereController : MonoBehaviour
    {
        private Rigidbody rigid;
        private Vector3 defaultPos;

        [SerializeField]
        public float speed = 0.5f;
        [SerializeField]
        public int y_max = 2;
        [SerializeField]
        public float initInterval = 5;
        // [SerializeField]
        // public int sphere_num;

        // explosion particle
        [SerializeField]
        public GameObject explosionParticle;
        [SerializeField]
        public GameObject sphere;
        [SerializeField]
        GameManager GM;

        // Start is called before the first frame update
        void Start()
        {
            sphere.SetActive(false);
            explosionParticle.SetActive(false);
            rigid = sphere.gameObject.GetComponent<Rigidbody>();
        }

        public void InitSphere()
        {
            sphere.SetActive(true);
            explosionParticle.SetActive(false);
            sphere.transform.position = new Vector3(sphere.transform.position.x, -2, sphere.transform.position.z);
            sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            defaultPos = sphere.transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 delta = new Vector3(0, 1, 0) * Mathf.Repeat(Time.time * speed, 4);
            rigid.MovePosition(defaultPos + delta);
            explosionParticle.transform.position = sphere.transform.position;
            isOnPlane(sphere.transform.position);
        }

        void isOnPlane(Vector3 pos)
        {
            if (pos.y < -1) {
                sphere.gameObject.GetComponent<Renderer>().enabled = false;
            } else {
                sphere.gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
        
        public void Explosion()
        {
            explosionParticle.SetActive(true);//爆発をBombの場所にBombの向きで呼び出す
            StartCoroutine(destroySphereTimer());//Bombを0.1秒後に消す
            StartCoroutine(destroyExplosionTimer());//Explosionを3秒後に消す

        }

        IEnumerator destroySphereTimer()
        {
            yield return new WaitForSeconds(0.1f);

            sphere.gameObject.SetActive(false);
        }

        IEnumerator destroyExplosionTimer()
        {
            yield return new WaitForSeconds(2);

            explosionParticle.SetActive(false);

            GM.removeSphere(gameObject);
        }
    }
}
