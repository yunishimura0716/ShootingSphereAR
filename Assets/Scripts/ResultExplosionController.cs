using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultExplosionController : MonoBehaviour
{
    public GameObject explosionParticle;
    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Explosion();
    }
    public void Explosion()
    {
        explosionParticle.SetActive(true);//爆発をBombの場所にBombの向きで呼び出す
        StartCoroutine(destroyExplosionTimer());//Explosionを3秒後に消す
    }

    IEnumerator destroyExplosionTimer()
    {
        yield return new WaitForSeconds(3);
        explosionParticle.SetActive(false);

    }
}
