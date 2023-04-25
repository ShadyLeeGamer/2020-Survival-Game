using UnityEngine;

public class Shooter : MonoBehaviour
{
    public bool pump;
    public bool isShooting;
    public bool upMode;

    public int currentIntensity;
    public int selectedSoap;
    public int currentType;
    public int[] currentAmmo;
    public int[] intensity;
    public int[] maxAmmo;
    public int[] type;

    public float currentRecoil;
    public float[] recoil;
    public float currentFireRate;
    public float[] fireRate;
    float nextTimeToFire;
    float nextTimeToAnim;

    public GameObject currentProjectile;
    public GameObject[] projectile;

    public Transform currnetFirePoint;
    public Transform[] firePoint;

    public SpriteRenderer[] soapAnim;
    public SpriteRenderer[] handAnim;
    public SpriteRenderer[] armAnim;

    public Sprite[] soapSprite1;
    public Sprite[] soapSprite2;
    public Sprite[] handUp;
    public Sprite[] handDown;
    public Sprite[] armSprite;

    public PlayerMovement player;
    Database database;

    public Vector2 currentMobility;
    public Vector2[] mobility;

    void Start()
    {
        database = FindObjectOfType<Database>();

        for (int i = 0; i < maxAmmo.Length; i++)
            currentAmmo[i] = maxAmmo[i];
    }

    void Update()
    {
        SetStats();

        if (player.canMove)
        {
            if (Input.GetKey(KeyCode.P) && currentAmmo[currentType] >= currentIntensity)
                Shoot();
            else
            {
                pump = false;

                NoShoot();
            }

            if (Input.GetKeyDown(KeyCode.O))
                SwitchSoap();
        }

        for (int i = 0; i < armAnim.Length; i++)
            armAnim[i].sprite = armSprite[database.skin];

        currentType = type[selectedSoap];

        if (isShooting && currentAmmo[currentType] >= currentIntensity)
        {
            if (Time.time >= nextTimeToFire)
            {
                if (!upMode)
                    database.pumps[currentType]++;

                for (int i = 0; i < currentIntensity; i++)
                {
                    currnetFirePoint.localEulerAngles = new Vector3(0, currnetFirePoint.rotation.y, Random.Range(-currentRecoil / 2, currentRecoil));
                    Instantiate(currentProjectile, currnetFirePoint.position, currnetFirePoint.rotation);

                    if (selectedSoap != 0)
                        currentAmmo[currentType]--;
                }

                nextTimeToFire = Time.time + 1f / currentFireRate;

                pump = true;

                for (int i = 0; i < handAnim.Length; i++)
                    handAnim[i].sprite = handDown[database.skin];
                for (int i = 0; i < soapAnim.Length; i++)
                    soapAnim[i].sprite = soapSprite2[currentType];
            }
            else if (Time.time > nextTimeToAnim)
            {
                nextTimeToAnim = Time.time + 1f / currentFireRate;

                pump = false;

                for (int i = 0; i < handAnim.Length; i++)
                    handAnim[i].sprite = handUp[database.skin];
                for (int i = 0; i < soapAnim.Length; i++)
                    soapAnim[i].sprite = soapSprite1[currentType];
            }
        }
        else
        {
            for (int i = 0; i < handAnim.Length; i++)
                handAnim[i].sprite = handUp[database.skin];
            for (int i = 0; i < soapAnim.Length; i++)
                soapAnim[i].sprite = soapSprite1[currentType];
        }
        if (upMode)
        {
            if (currentAmmo[4] < 1)
            {
                selectedSoap = 0;
                upMode = false;
            }
        }
    }

    void SetStats()
    {
        currentProjectile = projectile[currentType];
        currentIntensity = intensity[currentType];
        currentFireRate = fireRate[currentType];
        currentMobility = mobility[currentType];
        currentRecoil = recoil[currentType];

        if (!player.isHugged)
        {
            player.speed = currentMobility.x;
            player.jumpForce = currentMobility.y;
        }

        for (int i = 0; i < soapAnim.Length; i++)
            soapAnim[i].sprite = soapSprite1[currentType];
    }

    public void Shoot()
    {
        isShooting = true;
        player.animator.SetBool("Shooting", true);
    }

    public void NoShoot()
    {
        isShooting = false;
        player.animator.SetBool("Shooting", false);
    }

    public void SwitchSoap()
    {
        if (selectedSoap == 0)
        {
            if (upMode)
                selectedSoap = 2;
            else if (type[1] != type[0])
                selectedSoap = 1;
        }
        else
            selectedSoap = 0;
    }
}
