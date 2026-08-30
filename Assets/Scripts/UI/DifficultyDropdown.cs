using TMPro;
using UnityEngine;

public class DifficultyDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager was not found.", this);
            return;
        }

        // Display the currently selected difficulty without
        // triggering the dropdown event.
        _dropdown.SetValueWithoutNotify(
            (int)GameManager.Instance.Difficulty
        );

        _dropdown.RefreshShownValue();
    }

    public void SetDifficulty(int dropdownValue)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager was not found.", this);
            return;
        }

        GameDifficulty difficulty =
            (GameDifficulty)dropdownValue;

        GameManager.Instance.SetDifficulty(difficulty);
    }
}
