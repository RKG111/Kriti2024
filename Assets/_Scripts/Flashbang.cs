using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashWhiteScreen : MonoBehaviour
{
    public GameObject Light2D;
    public GameObject light2D;
    public Button FlashBang;

    private float originalIntensity;

    // void Start()
    // {
    //     Light2D=GameObject.Find("Light2D");
    //     light2D = Light2D.GetComponent<Light2D>();
    //     // Save the original intensity
    //     originalIntensity = light2D.intensity;

    //     // Add a listener to the button click event
    //     FlashBang.onClick.AddListener(OnClickButton);
    // }

    // void OnClickButton()
    // {
    //     // Increase the intensity to 100 for 3 seconds
    //     StartCoroutine(ChangeIntensityForDuration(100f, 3f));
    // }

    // IEnumerator ChangeIntensityForDuration(float targetIntensity, float duration)
    // {
    //     // Set the intensity to the target value
    //     light2D.intensity = targetIntensity;

    //     // Wait for the specified duration
    //     yield return new WaitForSeconds(duration);

    //     // Revert to the original intensity after the duration
    //     light2D.intensity = originalIntensity;
    // }
}
