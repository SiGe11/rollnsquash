using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO don't duplicate everything. AbstractClass?
public class PlayerControllerr : MonoBehaviour
{

    public float speed;
    public Transform sparkle;
    public string horizontalKey;
    public string verticalKey;
    public string jumoKey;
    public AudioClip soundOnCollision;

    private Rigidbody rb;

    private int jumpCounter = 0;
    private int massCounter = 0;
    private ParticleSystem ps;
    private AudioSource audio;
    

    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        ps = sparkle.GetComponent<ParticleSystem>();
        ps.enableEmission = false;
        audio = GetComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.clip = soundOnCollision;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void FixedUpdate()
    {

        float moveHorizontal = 0;
        float moveVertical = 0;
        moveHorizontal = Input.GetAxis(horizontalKey);
        moveVertical = Input.GetAxis(verticalKey);
        bool jump = Input.GetKeyDown(jumoKey);
        float jumpvalue = 0.0f;
        if (jump && jumpCounter >= 60)
        {
            jumpvalue = 25;
            jumpCounter = 0;
        }
        Vector3 movement = new Vector3(moveHorizontal, jumpvalue, moveVertical);

        rb.AddForce(movement * speed);
        jumpCounter++;

        if (rb.mass == 1.0f && massCounter >= 600)
        {
            rb.mass = 0.3f;
        } else
        {
            massCounter++;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mass")) {
            Destroy(other.gameObject);
            rb.mass = 1.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            return;
        }

        if (collision.relativeVelocity.magnitude > 10)
        {
            audio.volume = 1 + (collision.relativeVelocity.magnitude * 10000);
            audio.Play();
            ps.enableEmission = true;
            StartCoroutine(stopSparkles());
        }
    }

    void OnGUI()
    {


        if (rb.position.y < -10) {
            GUI.Label(new Rect(20, 40, 80, 20), "Player loose");
        }
    }

    IEnumerator stopSparkles()
    {
        yield return new WaitForSeconds (.4f);

        ps.enableEmission = false;
    }
}
