using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    private List<InputDevice> inputDevices = new List<InputDevice>();
    private InputDevice leftController;             

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputDevices.GetDevices(inputDevices);

        // Get the left-hand controller specifically
        var leftHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandedControllers);
        if (leftHandedControllers.Count > 0)
        {
            leftController = leftHandedControllers[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!leftController.isValid)
        {
            // Reacquire if disconnected
            var leftHandedControllers = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandedControllers);
            if (leftHandedControllers.Count > 0)
            {
                leftController = leftHandedControllers[0];
            }
        }

      if (IsSecondaryButtonPressed(leftController))
    {
       RestartAll();
    }
}

public void RestartAll()
{
    Scene currentScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(currentScene.name);
}
    private bool IsSecondaryButtonPressed(InputDevice device)
    {
        bool value;

        // Common VR inputs to check
        return device.TryGetFeatureValue(CommonUsages.secondaryButton, out value) && value;
    }
}