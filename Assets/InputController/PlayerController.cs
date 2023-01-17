//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputController/PlayerController.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerController : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerController"",
    ""maps"": [
        {
            ""name"": ""Gamepad"",
            ""id"": ""e10a52d5-4c0a-4d02-a6df-b70d9caf2c41"",
            ""actions"": [
                {
                    ""name"": ""FireButton"",
                    ""type"": ""Button"",
                    ""id"": ""baa6c1e9-798e-4322-87f7-2cc4fa19d336"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""30af2d94-10b3-489a-aa3b-a6c8a214e360"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""c73e73df-dae9-4812-8bbf-cef80f712ff6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump1"",
                    ""type"": ""Button"",
                    ""id"": ""d991ee90-6681-49d4-872a-0315840de486"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FireButton1"",
                    ""type"": ""Button"",
                    ""id"": ""5db8b14c-3481-409f-bb51-d4c16175e76e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3077e68b-d717-443d-8c33-6a15b221d504"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3bb631ce-314f-4b07-813d-9b424cc752a0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Joystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6db72c2-6a7b-4c96-9277-43acbe526e47"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireButton1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf275c66-1d44-4477-9118-e7314a314c45"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a81908f0-57c8-4b2f-b32b-ca2fcbd07942"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gamepad
        m_Gamepad = asset.FindActionMap("Gamepad", throwIfNotFound: true);
        m_Gamepad_FireButton = m_Gamepad.FindAction("FireButton", throwIfNotFound: true);
        m_Gamepad_Joystick = m_Gamepad.FindAction("Joystick", throwIfNotFound: true);
        m_Gamepad_Jump = m_Gamepad.FindAction("Jump", throwIfNotFound: true);
        m_Gamepad_Jump1 = m_Gamepad.FindAction("Jump1", throwIfNotFound: true);
        m_Gamepad_FireButton1 = m_Gamepad.FindAction("FireButton1", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gamepad
    private readonly InputActionMap m_Gamepad;
    private IGamepadActions m_GamepadActionsCallbackInterface;
    private readonly InputAction m_Gamepad_FireButton;
    private readonly InputAction m_Gamepad_Joystick;
    private readonly InputAction m_Gamepad_Jump;
    private readonly InputAction m_Gamepad_Jump1;
    private readonly InputAction m_Gamepad_FireButton1;
    public struct GamepadActions
    {
        private @PlayerController m_Wrapper;
        public GamepadActions(@PlayerController wrapper) { m_Wrapper = wrapper; }
        public InputAction @FireButton => m_Wrapper.m_Gamepad_FireButton;
        public InputAction @Joystick => m_Wrapper.m_Gamepad_Joystick;
        public InputAction @Jump => m_Wrapper.m_Gamepad_Jump;
        public InputAction @Jump1 => m_Wrapper.m_Gamepad_Jump1;
        public InputAction @FireButton1 => m_Wrapper.m_Gamepad_FireButton1;
        public InputActionMap Get() { return m_Wrapper.m_Gamepad; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamepadActions set) { return set.Get(); }
        public void SetCallbacks(IGamepadActions instance)
        {
            if (m_Wrapper.m_GamepadActionsCallbackInterface != null)
            {
                @FireButton.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnFireButton;
                @FireButton.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnFireButton;
                @FireButton.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnFireButton;
                @Joystick.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJoystick;
                @Joystick.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJoystick;
                @Joystick.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJoystick;
                @Jump.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJump;
                @Jump1.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJump1;
                @Jump1.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJump1;
                @Jump1.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnJump1;
                @FireButton1.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnFireButton1;
                @FireButton1.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnFireButton1;
                @FireButton1.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnFireButton1;
            }
            m_Wrapper.m_GamepadActionsCallbackInterface = instance;
            if (instance != null)
            {
                @FireButton.started += instance.OnFireButton;
                @FireButton.performed += instance.OnFireButton;
                @FireButton.canceled += instance.OnFireButton;
                @Joystick.started += instance.OnJoystick;
                @Joystick.performed += instance.OnJoystick;
                @Joystick.canceled += instance.OnJoystick;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Jump1.started += instance.OnJump1;
                @Jump1.performed += instance.OnJump1;
                @Jump1.canceled += instance.OnJump1;
                @FireButton1.started += instance.OnFireButton1;
                @FireButton1.performed += instance.OnFireButton1;
                @FireButton1.canceled += instance.OnFireButton1;
            }
        }
    }
    public GamepadActions @Gamepad => new GamepadActions(this);
    public interface IGamepadActions
    {
        void OnFireButton(InputAction.CallbackContext context);
        void OnJoystick(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnJump1(InputAction.CallbackContext context);
        void OnFireButton1(InputAction.CallbackContext context);
    }
}
