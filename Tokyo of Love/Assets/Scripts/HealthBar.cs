using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    private HealthSystem healthSystem;

    private void Start()
    {
        bar = transform.Find("Bar");
    }

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        bar.localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }

    public void SetColor(Color color)
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }

    public void SetSize(float health)
    {
        bar.localScale = new Vector3(health, 1);
    }

}
