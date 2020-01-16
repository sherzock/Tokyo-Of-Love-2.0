using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro text;
    private const float DISAPPEAR_TIMER_MAX = 0.75f;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private static int sortingOrder;

    public static DamagePopup Create(Transform popup, Vector3 position, int damageAmount, bool isCriticalHit)
    {
       Transform damagePopupTransform = Instantiate(popup, position, Quaternion.identity);
       DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
       damagePopup.Setup(damageAmount, isCriticalHit);

        return damagePopup;
    }

    private void Awake()
    {
        text = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        text.SetText(damageAmount.ToString());
        if (isCriticalHit)
        {
            text.fontSize = 65;
            textColor = Color.red;
        }
        else
        {
            text.fontSize = 50;
            textColor = Color.yellow;
        }

        text.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        text.sortingOrder = sortingOrder;

        moveVector = new Vector3(0.7f, 1.0f) * 60.0f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8.0f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX / 2)
        {
            transform.localScale += Vector3.one * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= 3.0f * Time.deltaTime;
            text.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
