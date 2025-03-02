using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private float spinSpeed = 5f;
     // Update is called once per frame
    void Update()
    {
        // we rotate the coin around y axis
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // we check if if the colliding object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // if so we incremement the score
            gameManager.IncrementScore();
            //and destroy this game object
            Destroy(gameObject);
        }
    }
}
