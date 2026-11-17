
using System.Collections;
using UnityEngine;

public class SteamVentHazard : MonoBehaviour
{
    [Header("Objek Visual")]
    [SerializeField] private GameObject steamSmoke;

    [Header("Pengaturan Waktu")]
    [SerializeField] private float interval = 5f;
    [SerializeField] private float activeDuration = 2f;

    [Header("Pengaturan Reposisi")]
    [SerializeField] private float repositionDistance = 2f;

    private Collider2D steamCollider;
    private bool isActive = false;
    private bool playerRepositioned = false;

    private void Awake()
    {
        steamCollider = GetComponent<Collider2D>();

        if (steamSmoke != null)
            steamSmoke.SetActive(false);

        if (steamCollider != null)
            steamCollider.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(SteamCycle());
    }

    private IEnumerator SteamCycle()
    {
        while (true)
        {
            // Menunggu sampai waktu semburan berikutnya
            yield return new WaitForSeconds(interval);

            // Mengaktifkan semburan
            ActivateSteam();

            // Menunggu durasi semburan
            yield return new WaitForSeconds(activeDuration);

            // Mematikan semburan
            DeactivateSteam();
        }
    }

    private void ActivateSteam()
    {
        isActive = true;
        playerRepositioned = false;

        if (steamSmoke != null)
            steamSmoke.SetActive(true);

        if (steamCollider != null)
            steamCollider.enabled = true;

        Debug.Log("Steam Vent aktif!");
    }

    private void DeactivateSteam()
    {
        isActive = false;

        if (steamSmoke != null)
            steamSmoke.SetActive(false);

        if (steamCollider != null)
            steamCollider.enabled = false;

        Debug.Log("Steam Vent tidak aktif.");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isActive || playerRepositioned)
            return;

        Rigidbody2D playerRb =
            other.GetComponentInParent<Rigidbody2D>();

        InventoryManager inventory =
            other.GetComponentInParent<InventoryManager>();

        // Memastikan objek yang terkena adalah Eco-Bot
        if (playerRb == null || inventory == null)
            return;

        // Menentukan arah menjauh dari sumber uap
        Vector2 direction =
            ((Vector2)playerRb.position -
            (Vector2)transform.position).normalized;

        // Jika tepat di tengah, gunakan arah default
        if (direction == Vector2.zero)
            direction = Vector2.up;

        // Memindahkan Eco-Bot menjauh dari Steam Vent
        playerRb.position += direction * repositionDistance;

        playerRepositioned = true;

        Debug.Log(
            "Eco-Bot terkena Steam Vent dan dipindahkan!"
        );
    }
}
