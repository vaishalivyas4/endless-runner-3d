using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float moveSpeed = 2f;

    Rigidbody rb;

    public float xLimit = 2.4f;

    public Transform cam;
    float curRotX = 0;
    Vector3 curCamRotation;
    public float maxRotAngle = 30f;
    public float rotSpeed = 5f;

    public float jumpForce = 300f;
    bool canJump = true;

    public Transform nozzle;
    public GameObject bulletPrefab;
    public float bulletSpeed = 800f;
    GameObject bullet;
    public float bulletExpTime = 2f;

    public GameObject reloadText;

    public int ammo = 10;
    public GameObject ammoText;

    public int lives = 3;
    public GameObject livesText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        #region Movement
        hmovement();

        //float h = Input.GetAxis("Horizontal");
        //Vector3 movement = transform.right * h * moveSpeed;
        //movement.y = rb.velocity.y;
        //rb.velocity = movement;

        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 curPos = transform.position;
        curPos.x = Mathf.Clamp(curPos.x, -xLimit, xLimit);
        transform.position = curPos;

        #endregion

        #region Camera Rotation
        curRotX += (mouseY * rotSpeed);
        curRotX = Mathf.Clamp(curRotX, -maxRotAngle, maxRotAngle);
        curCamRotation = cam.rotation.eulerAngles;
        curCamRotation.x = -curRotX;
        cam.rotation = Quaternion.Euler(curCamRotation);
        cam.transform.Rotate(Vector3.right * mouseY * rotSpeed);
        #endregion

        #region Bullet Fire
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammo <= 0)
                resetAmmo();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            bullet = Instantiate(bulletPrefab, nozzle.position, nozzle.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
            Destroy(bullet, bulletExpTime);

        if (ammo >=1)
            {
                ammo--;
                ammoText.GetComponent<Text>().text = "Ammo: " + ammo;
            }

        if(ammo <= 0 && !reloadText.activeInHierarchy)
            {
                reloadText.SetActive(true);
            }
        
        }
        ammoText.GetComponent<Text>().text = "Ammo: " + ammo;
        #endregion

        #region Jump
        if (Input.GetButtonDown("Jump") && canJump == true)
        {
            rb.AddForce(Vector3.up * jumpForce);
            //canJump = false;
        }
        #endregion

        livesText.GetComponent<Text>().text = "Lives: " + lives;
    }

    public void hmovement()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 iniPos = transform.position;
            iniPos.x -= 2;
            transform.position = iniPos;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 iniPos = transform.position;
            iniPos.x += 2;
            transform.position = iniPos;
        }
    }
    void resetAmmo()
    {
        ammo = 10;
        ammoText.GetComponent<Text>().text = "Ammo: " + ammo;
        reloadText.SetActive(false);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            WorldManager.instance.TakeDamage();
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "Ground")
        {
            canJump = true;
        }

        if (col.gameObject.tag == "PowerUp")
        {
            ammo *= 2;
        }

        if (col.gameObject.tag == "Life")
        {
            lives += 1;
            livesText.GetComponent<Text>().text = "Lives: " + lives;
        }
       
        

    }

}
