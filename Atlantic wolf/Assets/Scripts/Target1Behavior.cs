using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.Common.CoM;

public class Target1Behavior : MonoBehaviour
{
    private Rigidbody targetRb;
    public ParticleSystem explosion;
    public ParticleSystem smoke;
    public ParticleSystem smokeSecond;

    readonly float speed = 1f;
    [HideInInspector]
    public static float MaxSpeed = 5f;
    readonly float drownSpeed = 1.5f;
    float speedInfo;

    private SpawnManager spawnManager;
    private GameManager gameManager;

    private bool isGotHit;


    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameManager = GameObject.Find("Game manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        ParticleStop();
        Drowning();

    }
    void Move()
    {
        targetRb.AddRelativeForce(Vector3.right * speed,ForceMode.VelocityChange);
        var targetVel = targetRb.velocity;
        if (targetVel.magnitude > MaxSpeed)
        {
            targetRb.velocity = targetVel.normalized * MaxSpeed;
        }
        speedInfo = Mathf.Round(targetVel.magnitude);
        Debug.Log($"Target1 current speed {speedInfo} , Max speed is {MaxSpeed}");
        if (!gameManager.isGameActive)
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Torpedo"))
        {
            explosion.Play();
            gameManager.explosionSoundBox.PlayOneShot(gameManager.explosionSound, 0.3f);

            gameObject.GetComponent<VariableCenterOfMass>().centerOfMassOffset = new Vector3(12, -30, Random.Range(-11, 15));
            Invoke(nameof(MassChange), 5.5f);
        }
        else if (collision.gameObject.CompareTag("DestroyPanel"))
        {
            Destroy(gameObject);
            gameManager.targetsDestroyedCount++;

        }
        else if (collision.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }

    void MassChange()
    {
        gameObject.GetComponent<VariableCenterOfMass>().baseMass *= 5;
        isGotHit = true;
    }
    void ParticleStop()
    {
        if (gameObject.transform.position.y < 7.5f)
        {
            explosion.Stop();
            smoke.Stop();
            smokeSecond.Stop();

        }
    }
    void Drowning()
    {
        if (isGotHit)
        {
            targetRb.AddRelativeForce(Vector3.down * drownSpeed, ForceMode.VelocityChange);
        }
    }
}
