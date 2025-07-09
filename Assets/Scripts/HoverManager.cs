using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverManager : MonoBehaviour
{
    [System.Serializable]
    public class TextHoverPair
    {
        public TextMeshProUGUI textElement;  // The text element to hover over
        public GameObject hoverImage;        // The corresponding hover image
    }

    public TextHoverPair[] textHoverPairs; // Array of text-image pairs

    private void Start()
    {
        // Hide all hover images at the start
        foreach (var pair in textHoverPairs)
        {
            if (pair.hoverImage != null)
            {
                pair.hoverImage.SetActive(false);
            }

            // Add EventTriggers for each text element to listen for hover events
            EventTrigger trigger = pair.textElement.gameObject.AddComponent<EventTrigger>();

            // Pointer Enter event
            EventTrigger.Entry enterEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            enterEvent.callback.AddListener((data) => OnPointerEnter(pair)); // Add pointer enter logic
            trigger.triggers.Add(enterEvent);

            // Pointer Exit event
            EventTrigger.Entry exitEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            exitEvent.callback.AddListener((data) => OnPointerExit(pair)); // Add pointer exit logic
            trigger.triggers.Add(exitEvent);
        }
    }

    // Show the hover image when pointer enters the text
    private void OnPointerEnter(TextHoverPair pair)
    {
        // Hide all hover images first
        foreach (var textHoverPair in textHoverPairs)
        {
            if (textHoverPair.hoverImage != null)
            {
                textHoverPair.hoverImage.SetActive(false);
            }
        }

        // Show the hover image for the specific text element
        if (pair.hoverImage != null)
        {
            pair.hoverImage.SetActive(true);
        }
    }

    // Hide the hover image when pointer exits the text
    private void OnPointerExit(TextHoverPair pair)
    {
        // Hide the hover image when mouse exits
        if (pair.hoverImage != null)
        {
            pair.hoverImage.SetActive(false);
        }
    }
}
