using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSFXTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(
            () => AudioManager.PlaySound(_clickSound, 0.2f)
            );
    }
}
