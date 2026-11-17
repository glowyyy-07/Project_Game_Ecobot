
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [Header("Kategori Tempat Sampah")]
    public TrashCategory binCategory;


    // =========================================
    // MENDETEKSI ECO-BOT
    // =========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mencari InventoryManager pada Eco-Bot
        InventoryManager inventory =
            other.GetComponentInParent<InventoryManager>();

        // Jika bukan Eco-Bot, hentikan proses
        if (inventory == null)
            return;

        // Jika inventaris kosong, tidak ada yang disetorkan
        if (inventory.IsEmpty)
        {
            Debug.Log("Inventaris kosong. Tidak ada sampah untuk disetorkan.");
            return;
        }

        // Mencoba menyetorkan sampah sesuai kategori tong
        int depositedCount =
            inventory.DepositItems(binCategory);

        // Jika tidak ada sampah yang sesuai,
        // catat satu kesalahan penyortiran
        if (depositedCount == 0)
        {
            inventory.RegisterWrongSort();
        }
    }
}
