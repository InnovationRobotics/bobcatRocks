// GENERATED AUTOMATICALLY FROM 'Assets/BOBCATMODEL/_bobcatScripts/misc/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controllers : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controllers()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""28bd76e0-ab63-4040-a115-488fee4ecbdc"",
            ""actions"": [
                {
                    ""name"": ""Arm"",
                    ""type"": ""Button"",
                    ""id"": ""f50e4650-f736-4b84-ba50-f899c35d72cd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Loader"",
                    ""type"": ""Button"",
                    ""id"": ""9abb9814-70ba-402a-8858-d60c9fd8e0a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""62537f4e-b998-428b-8f5c-748148c024bd"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Arm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58f9c64c-79f5-4b85-9c16-856e5d8d66f6"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Loader"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_Arm = m_GamePlay.FindAction("Arm", throwIfNotFound: true);
        m_GamePlay_Loader = m_GamePlay.FindAction("Loader", throwIfNotFound: true);
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

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private IGamePlayActions m_GamePlayActionsCallbackInterface;
    private readonly InputAction m_GamePlay_Arm;
    private readonly InputAction m_GamePlay_Loader;
    public struct GamePlayActions
    {
        private @Controllers m_Wrapper;
        public GamePlayActions(@Controllers wrapper) { m_Wrapper = wrapper; }
        public InputAction @Arm => m_Wrapper.m_GamePlay_Arm;
        public InputAction @Loader => m_Wrapper.m_GamePlay_Loader;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void SetCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
            {
                @Arm.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnArm;
                @Arm.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnArm;
                @Arm.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnArm;
                @Loader.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLoader;
                @Loader.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLoader;
                @Loader.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLoader;
            }
            m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Arm.started += instance.OnArm;
                @Arm.performed += instance.OnArm;
                @Arm.canceled += instance.OnArm;
                @Loader.started += instance.OnLoader;
                @Loader.performed += instance.OnLoader;
                @Loader.canceled += instance.OnLoader;
            }
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);
    public interface IGamePlayActions
    {
        void OnArm(InputAction.CallbackContext context);
        void OnLoader(InputAction.CallbackContext context);
    }
}
