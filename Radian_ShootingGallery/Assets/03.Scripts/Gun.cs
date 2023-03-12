using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int maxAmmo;
    int ammo;
    private List<Joycon> joycons;

    // Values made available via Unity
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public int jc_ind = 0;
    public Quaternion orientation;
    public Quaternion correctedOrientation, rotationOffset;
    public Transform cannonTip, target;
    public GameObject bullet;
    public float coolOffTime;
    private bool canShoot;
    private bool startShoot;
    public LineRenderer BulletFX;
    void Start()
    {
        ammo = maxAmmo;
        TrySetupController();
        canShoot = true;
        if (BulletFX && cannonTip && target)
        {
            BulletFX.SetPosition(0, cannonTip.position);
            BulletFX.SetPosition(1, cannonTip.position);

        }
    }
    IEnumerator gunCoolOff()
    {
        canShoot = false;
        yield return new WaitForSeconds(coolOffTime);
        canShoot = true;
    }
    private void TrySetupController()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
        {
            Debug.Log("No Joycon detected");
            gameObject.SetActive(false);

        }
    }
    bool shoot;
    float delta = 0;
    public float shootSpeed = 3f;
    void Update()
    {
        if (BulletFX && cannonTip && target)
        {
            BulletFX.SetPosition(0, cannonTip.position);

            if(Input.GetMouseButtonDown(0) && !shoot)
            {
                shoot = true;
            }
            if (shoot && delta < 1)
            {
                BulletFX.SetPosition(1, Vector3.Lerp(cannonTip.position, target.position, delta));
                delta += Time.deltaTime*shootSpeed;
                shoot = delta < 1f;
            }
            if(!shoot && delta>0)
            {
                delta = 0;
                BulletFX.SetPosition(1, cannonTip.position);
            }
        }
        
        if (joycons.Count > 0)
        {
            Joycon j = joycons[jc_ind];
            if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 released");
                if (bullet && cannonTip && canShoot && startShoot  )
                {
                    if (ammo > 0)
                    {
                        ammo--;
                        Instantiate(bullet, cannonTip.position, cannonTip.rotation);
                        StartCoroutine(gunCoolOff());
                        startShoot = false;
                    }
                    else
                    {
                        j.SetRumble(160, 320, 0.6f, 200);
                        j.SetRumble(160, 320, 0.6f, 200);
                        j.SetRumble(160, 320, 0.6f, 200);
                    }
                }
            }
            if (j.GetButtonDown(Joycon.Button.PLUS))
            {
                j.SetRumble(160, 320, 0.6f, 200);
                j.Recenter();

            }
        }
    }
}
