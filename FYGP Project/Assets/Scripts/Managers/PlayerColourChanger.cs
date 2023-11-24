using UnityEngine.UI;
using UnityEngine;

public class PlayerColourChanger : MonoBehaviour
{
    [SerializeField] private Image characterImage;

    public void ChangeColour(Color newColour)
    {
        if (characterImage != null)
        {
            characterImage.color = newColour;
        }
    }
}
