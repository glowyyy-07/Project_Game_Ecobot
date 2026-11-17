
using UnityEngine;

public enum PowerUpType
{
    Timer,
    Speed,
    Magnet
}

public class PowerUpItem : MonoBehaviour
{
    [Header("Jenis Power-Up")]
    public PowerUpType powerUpType;

    [Header("Nilai Efek")]
    public float minValue;
    public float maxValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerUpManager manager =
            other.GetComponentInParent<PowerUpManager>();

        if (manager == null)
            return;

        manager.ActivatePowerUp(this);

        Destroy(gameObject);
    }
}
