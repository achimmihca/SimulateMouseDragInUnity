using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class SampleSceneControl : MonoBehaviour
{
    private bool isSimulatingMouseDrag;
    private bool wasSimulatingMouseDrag;

    // Debugging UI
    private UIDocument uiDocument;
    private Label mouseButtonStateLabel;
    private Button clickCountButton;

    private void Start()
    {
        InitUI();
    }

    private void Update()
    {
        // Simulate mouse drag with H key.
        isSimulatingMouseDrag = Keyboard.current.hKey.isPressed;

        UpdateMouseDragSimulation();
        UpdateUI();
    }

    private void UpdateMouseDragSimulation()
    {
        // TODO: Simulating drag does not seem to work, although the button is triggered as expected.
        //   See https://forum.unity.com/threads/how-to-simulate-drag-event-with-new-inputsystem.1525885/
        //   See https://unity3d.atlassian.net/servicedesk/customer/portal/2/IN-64088

        Mouse mouse = Mouse.current;
        if (isSimulatingMouseDrag)
        {
            // Keep writing a 1 to the InputControl
            Debug.Log("Writing 1 to leftMouseButton");
            using (StateEvent.From(mouse, out InputEventPtr eventPtr))
            {
                mouse.leftButton.WriteValueIntoEvent(1f, eventPtr);
                InputSystem.QueueEvent(eventPtr);
            }
        }
        else if (!isSimulatingMouseDrag && wasSimulatingMouseDrag)
        {
            // Write a 0 to the InputControl once
            Debug.Log("Writing 0 to leftMouseButton");
            using (StateEvent.From(mouse, out InputEventPtr eventPtr))
            {
                mouse.leftButton.WriteValueIntoEvent(0f, eventPtr);
                InputSystem.QueueEvent(eventPtr);
            }
        }

        wasSimulatingMouseDrag = isSimulatingMouseDrag;
    }

    private void InitUI()
    {
        uiDocument = FindObjectOfType<UIDocument>();
        mouseButtonStateLabel = uiDocument.rootVisualElement.Q<Label>("mouseButtonStateLabel");
        clickCountButton = uiDocument.rootVisualElement.Q<Button>("clickCountButton");
        int clickCount = 0;
        clickCountButton.RegisterCallback<ClickEvent>(evt =>
        {
            clickCount++;
            clickCountButton.text = $"Click count: {clickCount}";
        });
    }

    private void UpdateUI()
    {
        mouseButtonStateLabel.text = $"isDragging: {isSimulatingMouseDrag} | wasDragging: {wasSimulatingMouseDrag}";
    }
}
