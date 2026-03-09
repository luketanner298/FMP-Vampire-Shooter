using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public Image HealthBar;
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = Mathf.Clamp(Health / MaxHealth, 0, 1);
    }
}