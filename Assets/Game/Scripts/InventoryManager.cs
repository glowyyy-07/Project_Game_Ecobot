
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string trashName;
    public TrashCategory category;

    public InventoryItem(string name, TrashCategory itemCategory)
    {
        trashName = name;
        category = itemCategory;
    }
}

public class InventoryManager : MonoBehaviour
{
    [Header("Pengaturan Inventaris")]
    public int maxCapacity = 2;

    private List<InventoryItem> inventory =
        new List<InventoryItem>();

    [Header("Status Permainan")]
    public int score = 0;
    public float battery = 100f;

    [Header("Kesalahan Penyortiran")]
    public int wrongSortCount = 0;
    public int maxWrongSort = 3;

    [Header("Statistik Sampah Berhasil Disetorkan")]
    public int organicDeposited = 0;
    public int nonOrganicDeposited = 0;
    public int b3Deposited = 0;

    // Properti inventaris
    public int CurrentCount => inventory.Count;

    public bool IsFull => inventory.Count >= maxCapacity;

    public bool IsEmpty => inventory.Count == 0;

    // Properti kondisi permainan
    public bool IsWrongSortLimitReached =>
        wrongSortCount >= maxWrongSort;

    public int TotalDeposited =>
        organicDeposited + nonOrganicDeposited + b3Deposited;


    // =========================================
    // 1. MENGAMBIL SAMPAH
    // =========================================

    public bool TryAddItem(TrashItem item)
    {
        if (item == null)
            return false;

        if (IsFull)
        {
            Debug.Log("Inventaris penuh! Tidak bisa mengambil sampah.");
            return false;
        }

        inventory.Add(
            new InventoryItem(item.trashName, item.category)
        );

        Debug.Log(
            item.trashName +
            " berhasil diambil. Inventaris: " +
            CurrentCount + "/" + maxCapacity
        );

        ShowInventory();

        return true;
    }


    // =========================================
    // 2. MENYETORKAN SAMPAH SESUAI KATEGORI
    // =========================================

    public int DepositItems(TrashCategory binCategory)
    {
        int depositedCount = 0;

        int gainedScore = 0;
        float gainedBattery = 0f;

        for (int i = inventory.Count - 1; i >= 0; i--)
        {
            InventoryItem item = inventory[i];

            // Lewati sampah yang tidak sesuai kategori tong
            if (item.category != binCategory)
                continue;

            switch (item.category)
            {
                case TrashCategory.Organic:

                    gainedScore += 10;
                    gainedBattery += 1f;
                    organicDeposited++;

                    break;

                case TrashCategory.NonOrganic:

                    gainedScore += 20;
                    gainedBattery += 2f;
                    nonOrganicDeposited++;

                    break;

                case TrashCategory.B3:

                    gainedScore += 50;
                    gainedBattery += 5f;
                    b3Deposited++;

                    break;
            }

            Debug.Log(
                item.trashName +
                " berhasil disetorkan ke tong " +
                binCategory
            );

            // Hapus hanya sampah yang berhasil disetorkan
            inventory.RemoveAt(i);

            depositedCount++;
        }

        if (depositedCount > 0)
        {
            // Skor maksimal sesuai GDD adalah 800
            score = Mathf.Min(score + gainedScore, 800);

            // Baterai maksimal 100%
            battery = Mathf.Clamp(
                battery + gainedBattery,
                0f,
                100f
            );

            Debug.Log("================================");
            Debug.Log("PENYETORAN BERHASIL");
            Debug.Log("Jumlah sampah: " + depositedCount);
            Debug.Log("Skor bertambah: +" + gainedScore);
            Debug.Log("Skor sekarang: " + score + "/800");
            Debug.Log("Baterai sekarang: " + battery + "%");
            Debug.Log("================================");

            ShowInventory();
            ShowDepositStatistics();
        }

        return depositedCount;
    }


    // =========================================
    // 3. MENCATAT KESALAHAN PENYORTIRAN
    // =========================================

    public void RegisterWrongSort()
    {
        wrongSortCount++;

        Debug.Log(
            "SALAH PILAH! Kesalahan: " +
            wrongSortCount + "/" + maxWrongSort
        );

        // Salah pilah tidak mengurangi baterai
        if (IsWrongSortLimitReached)
        {
            Debug.Log(
                "GAME OVER! Pemain sudah salah memilah sampah 3 kali."
            );
        }
    }


    // =========================================
    // 4. MENAMPILKAN ISI INVENTARIS
    // =========================================

    public void ShowInventory()
    {
        Debug.Log("===== ISI INVENTARIS =====");

        if (IsEmpty)
        {
            Debug.Log("Inventaris kosong.");
            return;
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            Debug.Log(
                "Slot " + (i + 1) + ": " +
                inventory[i].trashName +
                " (" + inventory[i].category + ")"
            );
        }
    }


    // =========================================
    // 5. MENAMPILKAN STATISTIK PENYETORAN
    // =========================================

    public void ShowDepositStatistics()
    {
        Debug.Log("===== STATISTIK SAMPAH =====");

        Debug.Log("Organic: " + organicDeposited);
        Debug.Log("NonOrganic: " + nonOrganicDeposited);
        Debug.Log("B3: " + b3Deposited);

        Debug.Log("Total disetorkan: " + TotalDeposited);
    }
}
