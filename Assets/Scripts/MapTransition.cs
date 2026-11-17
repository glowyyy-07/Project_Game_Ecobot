using System.Collections;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [Header("Tujuan & Kamera")]
    public Transform titikTujuanPlayer;      // Isi dengan Spawn_Di_Map2
    public Vector3 posisiKameraMap2;         // Koordinat X, Y, Z kamera Map 2

    [Header("Animasi Layar Gelap")]
    public CanvasGroup layarHitam;           // Drag objek LayarHitam ke sini
    public float durasiFade = 0.4f;

    private bool sedangTransisi = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !sedangTransisi)
        {
            StartCoroutine(ProsesPindahMap(other.transform));
        }
    }

    IEnumerator ProsesPindahMap(Transform player)
    {
        sedangTransisi = true;

        // 1. Layar perlahan menghitam (Fade Out)
        float waktu = 0f;
        while (waktu < durasiFade)
        {
            waktu += Time.deltaTime;
            layarHitam.alpha = Mathf.Lerp(0f, 1f, waktu / durasiFade);
            yield return null;
        }
        layarHitam.alpha = 1f;

        // 2. Pindahkan posisi robot dan kamera ke Map 2 saat layar gelap
        player.position = titikTujuanPlayer.position;
        Camera.main.transform.position = posisiKameraMap2;

        yield return new WaitForSeconds(0.15f);

        // 3. Layar perlahan terang kembali di Map 2 (Fade In)
        waktu = 0f;
        while (waktu < durasiFade)
        {
            waktu += Time.deltaTime;
            layarHitam.alpha = Mathf.Lerp(1f, 0f, waktu / durasiFade);
            yield return null;
        }
        layarHitam.alpha = 0f;

        sedangTransisi = false;
    }
}