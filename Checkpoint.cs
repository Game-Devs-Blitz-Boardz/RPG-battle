using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private Animator anim;
    public string id;
    public bool activationStatus;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (activationStatus == true)
            anim.SetBool("active", activationStatus);
    }

    [ContextMenu("Generate Checkpoint Id")]
    private void GenerateId() {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() != null) {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint() {
        activationStatus = true;

        // anim.SetBool("active", true);
    }

}
