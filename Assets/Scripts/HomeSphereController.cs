using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSphereController : MonoBehaviour
{
    private Vector3 defaultPos;
    private Rigidbody rigid;
    public float speed;

    // explosion particle
    public GameObject explosionParticle;
    [SerializeField]
    public GameObject sphere;


    // Start is called before the first frame update
    void Start()
    {
        rigid = sphere.gameObject.GetComponent<Rigidbody>();
        InitSphere();
    }
    public void InitSphere()
    {
        sphere.SetActive(true);
        explosionParticle.SetActive(false);
        sphere.transform.position = new Vector3(695, 350, 80);
        defaultPos = sphere.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 delta = new Vector3(0, 1, 0) * Mathf.Repeat(Time.time * speed, 60);
        rigid.MovePosition(defaultPos + delta);
        explosionParticle.transform.position = sphere.transform.position;
        // if (sphere.transform.position.y >= 390)
        // {
        //     Explosion();
        // }
    }

    public void Explosion()
    {
        explosionParticle.SetActive(true);//爆発をBombの場所にBombの向きで呼び出す
        StartCoroutine(destroySphereTimer());//Bombを0.1秒後に消す
        StartCoroutine(destroyExplosionTimer());//Explosionを3秒後に消す
        // StartCoroutine(initTimer());

    }

    IEnumerator destroySphereTimer()
    {
        yield return new WaitForSeconds(0.1f);
        sphere.SetActive(false);
    }

    IEnumerator destroyExplosionTimer()
    {
        yield return new WaitForSeconds(3);
        explosionParticle.SetActive(false);

    }
    IEnumerator initTimer()
    {
        yield return new WaitForSeconds(3);
        InitSphere();
    }
}
