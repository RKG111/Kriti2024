using UnityEngine;

public class mynkEnemySound : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public AudioClip soundClip; // Sound clip to play
    public float maxDistance = 10f; // Maximum distance at which the sound is audible
    public float maxVolume = 1f; // Maximum volume when the player is within range

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    private void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within range to play the sound
        if (distanceToPlayer < maxDistance)
        {
            // Calculate volume based on distance
            float volume = 1f - (distanceToPlayer / maxDistance);
            volume = Mathf.Clamp(volume, 0f, maxVolume);

            // Set the volume of the audio source
            audioSource.volume = volume;

            // Play the sound if it's not already playing
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Stop the sound if the player is out of range
            audioSource.Stop();
        }
    }
}
