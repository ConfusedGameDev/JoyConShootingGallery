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
    public Transform cannonTip;
    public GameObject bullet;
    public float coolOffTime;
    private bool canShoot;
    private bool startShoot;
    void Start()
    {
        ammo = maxAmmo;
        TrySetupController();
        canShoot = true;
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

     
    void Update()
    {
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
