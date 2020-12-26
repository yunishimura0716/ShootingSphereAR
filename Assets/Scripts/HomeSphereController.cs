using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSphereController : MonoBehaviour
{
    private Vector3 defaultPos;
    public float speed;

    // explosion particle
    [SerializeField]
    public GameObject explosionParticle;
    [SerializeField]
    public GameObject sphere;


    // Start is called before the first frame update
    void Start()
    {
        defaultPos = sphere.transform.position;
        InitSphere();
    }
    public void InitSphere()
    {
        sphere.SetActive(true);
        explosionParticle.SetActive(false);
        sphere.transform.position = defaultPos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = new Vector3(0, 1, 0) * Mathf.Repeat(Time.time * speed, 45);
        sphere.transform.position = defaultPos + delta;
        explosionParticle.transform.position = sphere.transform.position;
        if (sphere.transform.position.y >= 385)
        {
            Explosion();
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
        sphere.SetActive(false);
    }

    IEnumerator destroyExplosionTimer()
    {
        yield return new WaitForSeconds(1);
        InitSphere();
    }
}
