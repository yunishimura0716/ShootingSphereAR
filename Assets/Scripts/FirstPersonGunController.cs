using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyClass;

public class FirstPersonGunController : MonoBehaviour
{
    // fields for recoil effect
    [Header("Recoil_Transform")]
    public Transform RecoilPositionTranform;
    public Transform RecoilRotationTranform;
    [Space(10)]
    [Header("Recoil_Settings")]
    public float PositionDampTime;
    public float RotationDampTime;
    [Space(10)]
    public float Recoil1;
    public float Recoil2;
    public float Recoil3;
    public float Recoil4;
    [Space(10)]
    public Vector3 RecoilRotation;
    public Vector3 RecoilKickBack;

    public Vector3 RecoilRotation_Aim;
    public Vector3 RecoilKickBack_Aim;
    [Space(10)]
    public Vector3 CurrentRecoil1;
    public Vector3 CurrentRecoil2;
    public Vector3 CurrentRecoil3;
    public Vector3 CurrentRecoil4;
    [Space(10)]
    public Vector3 RotationOutput;

    public bool aim;

    // fields for shooting effect
    public enum ShootMode { AUTO, SEMIAUTO }
    public bool shootEnabled = true;
    [SerializeField]
    ShootMode shootMode = ShootMode.AUTO;
    [SerializeField]
    int maxAmmo = 30;
    [SerializeField]
    int maxSupplyValue = 100;
    [SerializeField]
    int damage = 1;
    [SerializeField]
    float shootInterval = 0.15f;
    [SerializeField]
    float shootRange = 50;
    [SerializeField]
    float supplyInterval = 0.2f;
    [SerializeField]
    Vector3 muzzleFlashScale;
    [SerializeField]
    GameObject muzzleFlashPrefab;
    [SerializeField]
    GameObject hitEffectPrefab;
    [SerializeField]
    Image ammoGauge;
    [SerializeField]
    Text ammoText;
    [SerializeField]
    Image supplyGauge;

    bool shooting = false;
    bool supplying = false;
    int ammo = 0;
    int supplyValue = 0;
    GameObject muzzleFlash;
    GameObject hitEffect;

    public int Ammo
    {
        set
        {
            ammo = Mathf.Clamp(value, 0, maxAmmo);

            //manipulate the representation of UI
            // text
            ammoText.text = ammo.ToString("D3");
            // guage
            float scaleX = (float) ammo / maxAmmo;
            ammoGauge.rectTransform.localScale = new Vector3(scaleX, 1, 1);
        }
        get
        {
            return ammo;
        }
    }
    public int SupplyValue
    {
        set
        {
            supplyValue = Mathf.Clamp(value, 0, maxSupplyValue);

            if (SupplyValue >= maxSupplyValue)
            {
                Ammo = maxAmmo;
                supplyValue = 0;
            }

            float scaleX = (float)supplyValue / maxSupplyValue;
            supplyGauge.rectTransform.localScale = new Vector3(scaleX, 1, 1);
        }
        get
        {
            return supplyValue;
        }
    }

    // countdown
    float countdown = 3f;
    int count;


    // Start is called before the first frame update
    void Start()
    {
        InitGun();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
            count = (int)countdown;
        }
        if (countdown <= 0)
        {
            if (shootEnabled && (ammo > 0) && GetInput())
            {
                StartCoroutine(ShootTimer());
            }
            if (shootEnabled)
            {
                StartCoroutine(SupplyTimer());
            }
        }
    }

    void InitGun()
    {
        Ammo = maxAmmo;
        SupplyValue = 0;
    }

    bool GetInput()
    {
        switch (shootMode)
        {
            case ShootMode.AUTO:
                //button values are 0 for left button, 1 for right button,
                //2 for the middle button. 
                //The return is true when the mouse button is pressed down, and false when released.
                return Input.GetMouseButton(0);      // long push shooting
            case ShootMode.SEMIAUTO:
                /*
                It will not return true until the user has released the mouse button and pressed it again.
                */
                return Input.GetMouseButtonDown(0);    // one push shooting
        }
        return false;
    }

    IEnumerator ShootTimer()
    {
        if (!shooting)
        {
            shooting = true;

            // muzzle flash on
            if (muzzleFlashPrefab != null)
            {
                if (muzzleFlash != null)
                {
                    muzzleFlash.SetActive(true);
                }
                else
                {
                    muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
                    muzzleFlash.transform.SetParent(gameObject.transform);
                    muzzleFlash.transform.localScale = muzzleFlashScale;
                }
            }

            Shoot();
            Fire();

            yield return new WaitForSeconds(shootInterval);

            // muzzle flash off
            if (muzzleFlash != null)
            {
                muzzleFlash.SetActive(false);
            }

            // hit effect off
            if (hitEffect != null)
            {
                if (hitEffect.activeSelf)
                {
                    hitEffect.SetActive(false);
                }
            }

            shooting = false;
        }
        else
        {
            yield return null;
        }
    }
    IEnumerator SupplyTimer()
    {
        if (!supplying)
        {
            supplying = true;

            SupplyValue++;

            yield return new WaitForSeconds(supplyInterval);

            supplying = false;
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // casting ray, then get hitting info of object
        if (Physics.Raycast(ray, out hit, shootRange))
        {
            // hit effect on
            if (hitEffectPrefab != null)
            {
                if (hitEffect != null)
                {
                    hitEffect.transform.position = hit.point;
                    hitEffect.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    hitEffect.SetActive(true);
                }
                else
                {
                    hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                }
            }
            string tagName = hit.collider.gameObject.tag;
            if(tagName == "Sphere")
            {
                Sphere sphere = hit.collider.gameObject.GetComponent<Sphere>();;
                sphere.Attacked();
            }
        }

        Ammo--;
    }

    // for recoil effect
    void FixedUpdate()
    {
        CurrentRecoil1 = Vector3.Lerp(CurrentRecoil1, Vector3.zero, Recoil1 * Time.deltaTime);
        CurrentRecoil2 = Vector3.Lerp(CurrentRecoil2, CurrentRecoil1, Recoil2 * Time.deltaTime);
        CurrentRecoil3 = Vector3.Lerp(CurrentRecoil3, Vector3.zero, Recoil3 * Time.deltaTime);
        CurrentRecoil4 = Vector3.Lerp(CurrentRecoil4, CurrentRecoil3, Recoil4 * Time.deltaTime);

        RecoilPositionTranform.localPosition = Vector3.Slerp(RecoilPositionTranform.localPosition, CurrentRecoil3, PositionDampTime * Time.fixedDeltaTime);
        RotationOutput = Vector3.Slerp(RotationOutput, CurrentRecoil1, RotationDampTime * Time.fixedDeltaTime);
        RecoilRotationTranform.localRotation = Quaternion.Euler(RotationOutput);
    }
    public void Fire()
    {
        if (aim == true)
        {
            CurrentRecoil1 += new Vector3(RecoilRotation_Aim.x, Random.Range(-RecoilRotation_Aim.y, RecoilRotation_Aim.y), Random.Range(-RecoilRotation_Aim.z, RecoilRotation_Aim.z));
            CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickBack_Aim.x, RecoilKickBack_Aim.x), Random.Range(-RecoilKickBack_Aim.y, RecoilKickBack_Aim.y), RecoilKickBack_Aim.z);
        }
        if (aim == false)
        {
            CurrentRecoil1 += new Vector3(RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
            CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);
        }
    }
}
