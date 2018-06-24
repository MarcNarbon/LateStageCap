using System .Collections;
using System .Collections .Generic;
using UnityEngine;
using UnityEngine .UI;
using UnityEngine .Audio;
public class shooter : MonoBehaviour
{
    public List<AudioClip> audioFX;
    public AudioSource audioSource;

    Text ammoText;
    SpriteRenderer ammoDisplay;
    public Sprite shotgunAmmoSpr, rifleAmmoSpr, pistolAmmoSpr;

    public npcBehaviour npcBehavScript;
    inventory playerInventory;

    public Transform aimLeft, aimRight;

    public GameObject pistolBullet;
    public GameObject rifleBullet;
    public GameObject shellBullet;
    GameObject bullet;

    public Transform pistolMuzzle;
    public Transform heavyPistolMuzzle;
    public Transform shotGunMuzzle;
    Transform muzzle;

    public GameObject player;
    public GameObject crossHair;
    public screenShake shakeScript;

    public Sprite softPistol;
    public Sprite heavyPistol;
    public Sprite shotGun;

    SpriteRenderer weaponSprite;

    private float lastShot = 0.0f;
    public float heavyPistolFRate, softPistolFRate, shotgunFRate;
    public bool aiming;
    public bool canFire = true;
    public bool enemyAiming;

    public enum weaponType { softPistol, heavyPistol, shotGun, empty };
    public weaponType thisWeapon;
    public weaponType enemyWeapon;


    void Start()
    {
        playerInventory = GameObject .Find("Inventory") .GetComponent<inventory>();
        enemyAiming = false;

        weaponSprite = GetComponent<SpriteRenderer>();
        aiming = false;
        crossHair = GameObject .Find("Crosshair");
        shakeScript = GameObject .Find("cameraTarget") .GetComponent<screenShake>();
        //weapon = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        enemyWeapon = (weaponType)Random .Range(0 , 3);

    }

    void setWeaponProp( weaponType weapon )
    {

        if ( thisWeapon == weaponType .heavyPistol )
        {
            weaponSprite .sprite = heavyPistol;
            muzzle = heavyPistolMuzzle;
            bullet = rifleBullet;

        }
        if ( thisWeapon == weaponType .softPistol )
        {
            weaponSprite .sprite = softPistol;
            muzzle = pistolMuzzle;
            bullet = pistolBullet;

        }
        if ( thisWeapon == weaponType .shotGun )
        {
            weaponSprite .sprite = shotGun;
            muzzle = shotGunMuzzle;
            bullet = shellBullet;

        }
        if ( thisWeapon == weaponType .empty )
        {
            weaponSprite .sprite = null;
            muzzle = null;
            bullet = null;

        }

    }

    void Update()
    {
        audioSource = GetComponent<AudioSource>();


        if ( ammoText == null )
            ammoText = GameObject .Find("AmmoText") .GetComponentInChildren<Text>();

        if ( ammoDisplay == null )
            ammoDisplay = GameObject .Find("AmmoDisplay") .GetComponent<SpriteRenderer>();

        // transform.position = player.transform.localPosition;

        if ( playerInventory == null )
            playerInventory = GameObject .Find("Inventory") .GetComponent<inventory>();

        if ( crossHair == null )
        {
            crossHair = GameObject .Find("Crosshair");
        }

        if ( Input .GetMouseButton(1) )
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }

        crossHair .GetComponent<SpriteRenderer>() .enabled = aiming;
        ammoText .enabled = aiming;
        ammoDisplay .enabled = aiming;
        ammoUiDisplay();
        setWeaponProp(thisWeapon);

        if ( !enemyAiming )
            weaponSprite .enabled = aiming;

        if ( !enemyAiming )
        {
            if ( aiming )
            {
                //GameObject.Find("cameraTarget").transform.position = new Vector3(-Input.mousePosition.x + transform.position.x, -Input.mousePosition.y + transform.position.y, transform.position.z);

                if ( Input .mousePosition .x < Screen .width / 2 )
                {
                    transform .position = aimLeft .position;
                    GetComponent<SpriteRenderer>() .flipY = true;
                }
                else
                {
                    transform .position = aimRight .position;
                    GetComponent<SpriteRenderer>() .flipY = false;

                }

                Vector3 mousePos = Input .mousePosition;
                mousePos .z = 5.23f;

                Vector3 objectPos = Camera .main .WorldToScreenPoint(transform .position);

                mousePos .x = mousePos .x - objectPos .x;
                mousePos .y = mousePos .y - objectPos .y;

                float angle = Mathf .Atan2(mousePos .y , mousePos .x) * Mathf .Rad2Deg;
                transform .localRotation = Quaternion .Euler(new Vector3(0 , 0 , angle));

                if ( Input .GetMouseButtonDown(0) )
                {
                    shootBullet();
                }
                else
                {
                    shotGunMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    shotGunMuzzle .gameObject .GetComponent<Light>() .enabled = false;
                    heavyPistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    heavyPistolMuzzle .gameObject .GetComponent<Light>() .enabled = false;
                    pistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    pistolMuzzle .gameObject .GetComponent<Light>() .enabled = false;

                }

                if ( Input .GetMouseButton(0) )
                {
                    if ( thisWeapon == weaponType .softPistol )//En vez de semiautomatica sera automatica
                        shootBullet();
                }
                else
                {

                    pistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    pistolMuzzle .gameObject .GetComponent<Light>() .enabled = false;

                }

            }
        }
        else
        {

        }
    }


    public void EnemyAiming( bool left , Transform playerPos )//Enemy aiming gun
    {

        if ( left )
        {
            thisWeapon = enemyWeapon;

            transform .position = aimLeft .position;
            GetComponent<SpriteRenderer>() .flipY = true;
            weaponSprite .enabled = true;

            if ( playerPos .localPosition .x > transform .localPosition .x )
            {
                transform .localRotation = Quaternion .Euler(new Vector3(0 , 0 , 0));
            }
            else
            {
                Vector2 aimPosition = playerPos .position - transform .position;
                float angle = Mathf .Atan2(aimPosition .y , aimPosition .x) * Mathf .Rad2Deg;
                transform .localRotation = Quaternion .Euler(new Vector3(0 , 0 , angle));
            }
        }
        else
        if ( !left )
        {
            thisWeapon = enemyWeapon;

            transform .position = aimRight .position;
            GetComponent<SpriteRenderer>() .flipY = false;
            weaponSprite .enabled = true;

            if ( playerPos .localPosition .x > transform .localPosition .x )
            {
                transform .localRotation = Quaternion .Euler(new Vector3(0 , 0 , 0));
            }
            else
            {
                Vector2 aimPosition = playerPos .position - transform .position;
                float angle = Mathf .Atan2(aimPosition .y , aimPosition .x) * Mathf .Rad2Deg;
                transform .localRotation = Quaternion .Euler(new Vector3(0 , 0 , angle));
            }
        }

    }

    void ammoUiDisplay()
    {

        if ( thisWeapon == weaponType .shotGun )
        {
            ammoDisplay .sprite = shotgunAmmoSpr;
            ammoText .text = playerInventory .ammo .x .ToString();
        }
        else if ( thisWeapon == weaponType .heavyPistol )
        {
            ammoDisplay .sprite = rifleAmmoSpr;
            ammoText .text = playerInventory .ammo .y .ToString();
        }
        else if ( thisWeapon == weaponType .softPistol )
        {
            ammoDisplay .sprite = pistolAmmoSpr;
            ammoText .text = playerInventory .ammo .z .ToString();
        }

    }

    public void shootBullet()//maxGaugeAmmo, maxRifleAmmo, maxPistolAmmo
    {
        if ( thisWeapon == weaponType .shotGun )
        {
            if ( !enemyAiming )
            {
                if ( playerInventory .ammo .x != 0 )// y = shotgunAmmo
                {
                    if ( Time .time > shotgunFRate + lastShot )
                    {
                        for ( int i = 0 ; i < 10 ; i++ )
                            Instantiate(bullet , muzzle .transform .position , transform .rotation , player .transform);

                        playerInventory .ammo .x--;
                        lastShot = Time .time;

                        shakeScript .shakers = screenShake .shakersPrefabs .hardShot;
                        shakeScript .enabled = true;

                        shotGunMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = true;
                        shotGunMuzzle .gameObject .GetComponent<Light>() .enabled = true;

                        audioSource .PlayOneShot(audioFX [5]);
                    }
                }
                else
                {
                    audioSource .PlayOneShot(audioFX [0]);
                    shotGunMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    shotGunMuzzle .gameObject .GetComponent<Light>() .enabled = false;
                }

            }
            else
            {
                if ( Time .time > shotgunFRate + lastShot )
                {
                    for ( int i = 0 ; i < 10 ; i++ )
                        Instantiate(bullet , muzzle .transform .position , transform .rotation , player .transform);

                    lastShot = Time .time;

                    shotGunMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = true;
                    shotGunMuzzle .gameObject .GetComponent<Light>() .enabled = true;

                    audioSource .PlayOneShot(audioFX [5]);
                }
                else
                {
                    shotGunMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    shotGunMuzzle .gameObject .GetComponent<Light>() .enabled = false;

                }
            }


        }

        //bulledFired.transform.parent = gameObject.transform;  
        if ( thisWeapon == weaponType .heavyPistol )
        {
            if ( !enemyAiming )
            {
                if ( playerInventory .ammo .y != 0 )// y = RifleAmmo
                {
                    if ( Time .time > heavyPistolFRate + lastShot )
                    {
                        playerInventory .ammo .y--;

                        Instantiate(bullet , muzzle .transform .position , transform .rotation , player .transform);
                        lastShot = Time .time;

                        shakeScript .shakers = screenShake .shakersPrefabs .medShot;
                        shakeScript .enabled = true;

                        heavyPistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = true;
                        heavyPistolMuzzle .gameObject .GetComponent<Light>() .enabled = true;

                        audioSource .PlayOneShot(audioFX [6]);

                    }
                }
                else
                {
                    audioSource .PlayOneShot(audioFX [0]);
                    heavyPistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    heavyPistolMuzzle .gameObject .GetComponent<Light>() .enabled = false;
                }
            }
            else
            {
                if ( Time .time > heavyPistolFRate + lastShot )
                {

                    Instantiate(bullet , muzzle .transform .position , transform .rotation , player .transform);
                    lastShot = Time .time;

                    heavyPistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = true;
                    heavyPistolMuzzle .gameObject .GetComponent<Light>() .enabled = true;

                    audioSource .PlayOneShot(audioFX [6]);
                }
                else
                {

                    heavyPistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    heavyPistolMuzzle .gameObject .GetComponent<Light>() .enabled = false;


                }

            }
        }
        if ( thisWeapon == weaponType .softPistol )
        {
            if ( !enemyAiming )
            {
                if ( playerInventory .ammo .z != 0 )// y = PistolAmmo
                {
                    if ( Time .time > softPistolFRate + lastShot )
                    {
                        playerInventory .ammo .z--;

                        Instantiate(bullet , muzzle .transform .position , transform .rotation , player .transform);
                        lastShot = Time .time;

                        shakeScript .shakers = screenShake .shakersPrefabs .softShot;
                        shakeScript .enabled = true;

                        pistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = true;
                        pistolMuzzle .gameObject .GetComponent<Light>() .enabled = true;

                        audioSource .PlayOneShot(audioFX [2]);

                    }
                    else
                    {

                        pistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                        pistolMuzzle .gameObject .GetComponent<Light>() .enabled = false;

                    }

                }
                else
                {
                    audioSource .PlayOneShot(audioFX [0]);
                }
            }
            else
            {
                if ( Time .time > softPistolFRate*2 + lastShot )
                {

                    Instantiate(bullet , muzzle .transform .position , transform .rotation , player .transform);
                    lastShot = Time .time;

                    pistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = true;
                    pistolMuzzle .gameObject .GetComponent<Light>() .enabled = true;

                    audioSource .PlayOneShot(audioFX [2]);
                }
                else
                {

                    pistolMuzzle .gameObject .GetComponent<SpriteRenderer>() .enabled = false;
                    pistolMuzzle .gameObject .GetComponent<Light>() .enabled = false;

                }

            }

        }
    }
}
