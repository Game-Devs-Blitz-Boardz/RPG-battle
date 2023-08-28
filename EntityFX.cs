using UnityEngine;
using System.Collections;

public class EntityFX : MonoBehaviour
{

    private SpriteRenderer sr;
    
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX() {
        sr.material = hitMat;
        yield return new WaitForSeconds(0.2f);
        sr.material = originalMat;
    }
}
