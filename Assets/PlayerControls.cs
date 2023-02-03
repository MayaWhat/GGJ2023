//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/PlayerControls.inputactions
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

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Above"",
            ""id"": ""901b5d20-3482-4003-b38b-c864fa1140ea"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""afadbfa8-1e7b-4b52-89cd-0e4e4f6b20b5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f73e877d-d32a-4d23-a284-8560312204c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Root"",
                    ""type"": ""Button"",
                    ""id"": ""5efb00f1-1ec9-477c-a22a-9599926072e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""0fbed4e1-ee45-4689-ae3b-567cb52d0ff5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a2d2b83f-57de-4a54-bbb4-ef8d4310d04c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d77dce0a-96af-4626-ae86-144560a22944"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""464f97eb-18e7-4cc7-8b34-6c15f8b8dc78"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""df217964-945b-4e1a-8631-c10e11a9857f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a415871c-4dfc-4938-862a-d8c34fe0e1ee"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31968afe-7a09-4020-a3d7-c2c8ef536f37"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Root"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PlayerControls"",
            ""bindingGroup"": ""PlayerControls"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Above
        m_Above = asset.FindActionMap("Above", throwIfNotFound: true);
        m_Above_Move = m_Above.FindAction("Move", throwIfNotFound: true);
        m_Above_Jump = m_Above.FindAction("Jump", throwIfNotFound: true);
        m_Above_Root = m_Above.FindAction("Root", throwIfNotFound: true);
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

    // Above
    private readonly InputActionMap m_Above;
    private IAboveActions m_AboveActionsCallbackInterface;
    private readonly InputAction m_Above_Move;
    private readonly InputAction m_Above_Jump;
    private readonly InputAction m_Above_Root;
    public struct AboveActions
    {
        private @PlayerControls m_Wrapper;
        public AboveActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Above_Move;
        public InputAction @Jump => m_Wrapper.m_Above_Jump;
        public InputAction @Root => m_Wrapper.m_Above_Root;
        public InputActionMap Get() { return m_Wrapper.m_Above; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AboveActions set) { return set.Get(); }
        public void SetCallbacks(IAboveActions instance)
        {
            if (m_Wrapper.m_AboveActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_AboveActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_AboveActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_AboveActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_AboveActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_AboveActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_AboveActionsCallbackInterface.OnJump;
                @Root.started -= m_Wrapper.m_AboveActionsCallbackInterface.OnRoot;
                @Root.performed -= m_Wrapper.m_AboveActionsCallbackInterface.OnRoot;
                @Root.canceled -= m_Wrapper.m_AboveActionsCallbackInterface.OnRoot;
            }
            m_Wrapper.m_AboveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Root.started += instance.OnRoot;
                @Root.performed += instance.OnRoot;
                @Root.canceled += instance.OnRoot;
            }
        }
    }
    public AboveActions @Above => new AboveActions(this);
    private int m_PlayerControlsSchemeIndex = -1;
    public InputControlScheme PlayerControlsScheme
    {
        get
        {
            if (m_PlayerControlsSchemeIndex == -1) m_PlayerControlsSchemeIndex = asset.FindControlSchemeIndex("PlayerControls");
            return asset.controlSchemes[m_PlayerControlsSchemeIndex];
        }
    }
    public interface IAboveActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRoot(InputAction.CallbackContext context);
    }
}
