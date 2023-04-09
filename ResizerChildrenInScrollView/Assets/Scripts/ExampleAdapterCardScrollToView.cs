using TMPro;
using UnityEngine;

public class ExampleAdapterCardScrollToView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NameText_Text;
    [SerializeField] private TextMeshProUGUI Strength_Text;
    [SerializeField] private TextMeshProUGUI Description_Text;

    public void SetCharacterParametres(string newName, int newStrength, string newDescription)
    {
        NameText_Text.text = newName;
        Strength_Text.text = newStrength + "";
        Description_Text.text = newDescription;
    }  
}
