using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Rotation
    readonly float rotateSens = 0.8f;

    //Raycast specs
    readonly float rayDistance = 750f;
    RaycastHit hit;
    public LayerMask IgnoreMe;


    //Periscope specs
    readonly float raiseSpeed = 0.03f;
    readonly float upperBorder = 24.2f;
    readonly float lowerBorder = 22f;

    private GameManager gameManager;
    private SpawnManager spawnManager;

    //For torpedo launch
    public GameObject firePoint;
    public Rigidbody projectile;
    public int projectileCount = 0;
    readonly float torpedoSpeed = 1850f;



    // Start is called before the first frame update
    void Awake()
    {
        transform.Rotate(0, 90, 0);
        gameManager = GameObject.Find("Game manager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();


    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            RotatePlayer();
            PeriscopeHeightControl();
            TorpedoLaunch();
            Zoom();
            RayView();
        }
        
        
    }
    void RotatePlayer()
    {
        //Debug.Log($"Actual value of y rotation {transform.eulerAngles.y}");
        transform.Rotate(rotateSens * new Vector3(0, y: Input.GetAxis("Mouse X"), 0));

        if (transform.eulerAngles.y > 120)
        {
            transform.eulerAngles = new Vector3(0, 120, 0);
        }
        else if (transform.eulerAngles.y < 60)
        {
            transform.eulerAngles = new Vector3(0, 60, 0);
        }
    }
    void PeriscopeHeightControl()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Translate(Vector3.up * raiseSpeed);
            if (transform.position.y >= upperBorder)
            {
                transform.position = new Vector3(transform.position.x, upperBorder, transform.position.z);
            }
        }
        else if (Input.GetKey(KeyCode.X))
        {
            transform.Translate(Vector3.down * raiseSpeed);
            if (transform.position.y <= lowerBorder)
            {
                transform.position = new Vector3(transform.position.x, lowerBorder, transform.position.z);
            }
        }
    }
    void TorpedoLaunch()
    {
        if (Input.GetKeyDown(KeyCode.Space) && projectileCount < 2 && gameManager.torpedoCount > 0)
        {
            gameManager.reloadingText.gameObject.SetActive(false);

            Debug.Log("Button hit");

            Rigidbody torpedo = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            torpedo.AddRelativeForce(Vector3.up * torpedoSpeed);
            projectileCount++;
            gameManager.torpedoCount--;
        }
        else if (projectileCount < 2 && gameManager.torpedoCount > 0)
        {
            gameManager.reloadingText.gameObject.SetActive(true);
        }
    }
    void Zoom()
    {
        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = 40;
        }
        else
            Camera.main.fieldOfView = 60;
    }
    void RayView()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green);

        if (Physics.Raycast(ray, out hit, 1000f, ~IgnoreMe))
        {
            //Debug.Log($"Игрок видит объект {hit.collider.name}");
            if (hit.collider.CompareTag("Left raycast marker"))
            {
                spawnManager.isLeftSpawnAllowed = true;
            }
            else if (hit.collider.CompareTag("Right raycast marker"))
            {
                spawnManager.isRightSpawnAllowed = true;
            }
        }
        else
        {
            spawnManager.isLeftSpawnAllowed = false;
            spawnManager.isRightSpawnAllowed = false;
            //Debug.Log($"Игрок не видит ничего");
        }
    }
}
