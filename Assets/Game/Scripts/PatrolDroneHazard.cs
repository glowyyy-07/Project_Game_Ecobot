
using UnityEngine;

public class PatrolDroneHazard : MonoBehaviour
{
    [Header("Waktu Aktivasi")]
    [SerializeField] private float activationTime = 60f;

    [Header("Pengaturan Gerakan")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] private float destinationDistance = 0.2f;

    [Header("Pengaturan Kerusakan")]
    [SerializeField] private float batteryDamage = 25f;

    private Vector2 startingPosition;
    private Vector2 targetPosition;

    private bool isActive = false;

    private void Start()
    {
        startingPosition = transform.position;

        // Drone belum bergerak sebelum detik ke-60
        targetPosition = startingPosition;

        // Nonaktifkan visual dan collider sementara
        GetComponent<Collider2D>().enabled = false;

        // Tentukan titik tujuan pertama
        ChooseNewDestination();
    }

    private void Update()
    {
        // Aktif pada detik ke-60 sejak scene dimulai
        if (!isActive &&
            Time.timeSinceLevelLoad >= activationTime)
        {
            ActivateDrone();
        }

        if (!isActive)
            return;

        PatrolMovement();
    }

    private void ActivateDrone()
    {
        isActive = true;

        GetComponent<Collider2D>().enabled = true;

        Debug.Log("Patrol Drone mulai aktif!");
    }

    private void PatrolMovement()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(
            transform.position,
            targetPosition
        ) <= destinationDistance)
        {
            ChooseNewDestination();
        }
    }

    private void ChooseNewDestination()
    {
        Vector2 randomPosition = Random.insideUnitCircle
            * patrolRadius;

        targetPosition = startingPosition + randomPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        InventoryManager inventory =
            other.GetComponentInParent<InventoryManager>();

        if (inventory == null)
            return;

        inventory.battery = Mathf.Clamp(
            inventory.battery - batteryDamage,
            0f,
            100f
        );

        Debug.Log(
            "Terkena Patrol Drone! Baterai berkurang 25%. " +
            "Baterai sekarang: " + inventory.battery + "%"
        );
    }
}
