using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelA;  // Reference to Panel A
    public GameObject panelB;  // Reference to Panel B
    public Button switchButton; // Reference to the button that will trigger the switch

    private bool isPanelAActive = true; // Boolean to track the currently active panel

    void Start()
    {
        // Initialize panels visibility
        panelA.SetActive(true);
        panelB.SetActive(false);

        // Add listener to the button click event
        switchButton.onClick.AddListener(SwitchPanel);
    }

    // Switch between the two panels
    void SwitchPanel()
    {
        if (isPanelAActive)
        {
            // Hide Panel A and show Panel B
            panelA.SetActive(false);
            panelB.SetActive(true);
        }
        else
        {
            // Hide Panel B and show Panel A
            panelA.SetActive(true);
            panelB.SetActive(false);
        }

        // Toggle the panel tracking flag
        isPanelAActive = !isPanelAActive;
    }
}