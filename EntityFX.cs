using UnityEngine;
using System.Collections;

public class EntityFX : MonoBehaviour
{

    private SpriteRenderer sr;
    
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX() {
        sr.material = hitMat;

        Color curColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(0.2f);

        sr.color = curColor;
        sr.material = originalMat;
    }

    private void RedColorBlink() {
        if (sr.color != Color.white) {
            sr.color = Color.white;
        } else {
            sr.color = Color.red;
        }
    }

    private void CancelColorChange() {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFXFor(float _seconds) {
        InvokeRepeating("IgniteColorFX", 0 , .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFXFor(float _seconds) {
        InvokeRepeating("ChillColorFX", 0 , .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFXFor(float _seconds) {
        InvokeRepeating("ShockColorFX", 0 , .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFX() {
        if (sr.color != igniteColor[0]) {
            sr.color = igniteColor[0];
        } else {
            sr.color = igniteColor[1];
        }
    }

    private void ChillColorFX() {
        if (sr.color != chillColor[0]) {
            sr.color = chillColor[0];
        } else {
            sr.color = chillColor[1];
        }
    }

    private void ShockColorFX() {
        if (sr.color != shockColor[0]) {
            sr.color = shockColor[0];
        } else {
            sr.color = shockColor[1];
        }
    }


}
