using UnityEngine;

public class ThingSound : MonoBehaviour
{

    public void PlayPotion_PickUp()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Potion_PickUp, gameObject.transform);
    }
    public void PlayLubratacation_PickUp()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Lubratacation_PickUp, gameObject.transform);
    }
    public void PlayGear_PickUp()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Gear_PickUp, gameObject.transform);
    }
    public void PlayLevel_Success()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Level_Success, gameObject.transform);
    }
}
