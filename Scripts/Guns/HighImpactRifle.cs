using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighImpactRifle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private iDamageable damageable;



    private float timeSinceLastShot;
    public Rigidbody playerRb;


    [Header("Graphics")]
    public ParticleSystem[] muzzleFlash;
    public TrailRenderer tracerEffect;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (gunData.fireRate / 60f);



    public void Shoot()
    {

        if (CanShoot())
        {
            OnGunShot();

            // Creating Tracers
            var tracer = Instantiate(tracerEffect, attackPoint.position, Quaternion.identity);
            tracer.AddPosition(attackPoint.position);

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitInfo;
            // Check if ray hits something
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hitInfo, gunData.maxDistance))
            {
                targetPoint = hitInfo.point;
                tracer.transform.position = targetPoint;
                Debug.Log(hitInfo.transform.name);
                damageable = hitInfo.transform.GetComponent<iDamageable>();
                damageable?.TakeDamage(gunData.damage);
                

            }
            else
            {
                //targetPoint = hitInfo.point;
                //tracer.transform.position = targetPoint;
                targetPoint = ray.GetPoint(1000); // A point away from the player

            }

            // Calculate Direction of attackpoint to targetpoint
            Vector3 direction = targetPoint - attackPoint.position;

            // Add bullet Recoil to playeer

            playerRb.AddForce(-direction.normalized * gunData.recoilForce, ForceMode.Impulse);

            //rayBarrel.origin = attackPoint.position;
            //rayBarrel.direction = attackPoint.forward;
            //if (Physics.Raycast(rayBarrel, out hitInfoBarrel, gunData.maxDistance))
            //{
            //    Debug.DrawLine(rayBarrel.origin, hitInfoBarrel.point, Color.red, 3.0f);
            //}


            //OnGunShot();
            //if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance))
            //{
            //    Debug.Log(hitInfo.transform.name);
            //}

            timeSinceLastShot = 0;
        }


    }

    // Effects
    private void OnGunShot()
    {
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }
        
    }

}