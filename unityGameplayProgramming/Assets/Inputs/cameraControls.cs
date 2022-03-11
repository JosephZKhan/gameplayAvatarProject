// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/cameraControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""cameraControls"",
    ""maps"": [
        {
            ""name"": ""Camera"",
            ""id"": ""8e3b88cc-71e1-43b4-b9ee-cf5d19f6251e"",
            ""actions"": [
                {
                    ""name"": ""Keys"",
                    ""type"": ""Button"",
                    ""id"": ""55bf4528-80e9-48d2-9b24-56eb8f55d623"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""7d080c0c-ca08-4311-9e4c-57fab76eb2bb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Centre"",
                    ""type"": ""Button"",
                    ""id"": ""04180dcb-62ef-49ce-8d5d-b0ffd488e3c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SnapLeft"",
                    ""type"": ""Button"",
                    ""id"": ""5309cf78-bb08-4957-bb7e-1ab9c7a3cde2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SnapRight"",
                    ""type"": ""Button"",
                    ""id"": ""7941a534-b12a-4dc7-bd9b-58dee9aed8f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""POV"",
                    ""type"": ""Button"",
                    ""id"": ""9aca58f7-40a4-4488-a5bf-702a15b3d6e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LockOn"",
                    ""type"": ""Button"",
                    ""id"": ""ebe0300c-ee69-4b3d-a354-5248ced8b46e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9d3856e4-ba7b-4c25-a06b-d415378db575"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Keys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d88169f-f955-4597-9686-02d89bb796d0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Keys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2314c8c9-e617-4d41-9989-15d7eeb669bf"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Keys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce780885-ce13-408a-bece-0b1c207a3051"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Keys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d77b7e2b-d91e-4891-8595-fe6b4c06b44d"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""56e773c3-6425-4edd-a538-d42275f4cc2c"",
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
                    ""id"": ""dacba640-4cc4-4e1b-8e34-a8b6c31a7dfd"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9c15ac79-b961-4241-8389-01b042801150"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""76dfc89f-93cd-47a0-b6b0-d658f8d2b284"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8f983374-9a43-4ba9-96f7-accb29672ade"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2d2bcce7-a483-4302-b774-679ed3427dc1"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Centre"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d91f0925-0eba-48e7-9482-91e9a048f290"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Centre"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27bfabbb-f0d9-4446-b370-af93b8380613"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SnapLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63fdde11-e0fa-4702-a58a-53aec5178fc1"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SnapLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27dac757-49b0-401c-8af7-03b046357f4e"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SnapRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c264eece-c37d-4d7f-b195-b50959953650"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SnapRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b30af21f-2259-4497-9333-45fd1bc57fb1"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""POV"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d29273d8-479d-45fd-acbf-bc2b65e5d7b6"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": []
        }
    ]
}");
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_Keys = m_Camera.FindAction("Keys", throwIfNotFound: true);
        m_Camera_Move = m_Camera.FindAction("Move", throwIfNotFound: true);
        m_Camera_Centre = m_Camera.FindAction("Centre", throwIfNotFound: true);
        m_Camera_SnapLeft = m_Camera.FindAction("SnapLeft", throwIfNotFound: true);
        m_Camera_SnapRight = m_Camera.FindAction("SnapRight", throwIfNotFound: true);
        m_Camera_POV = m_Camera.FindAction("POV", throwIfNotFound: true);
        m_Camera_LockOn = m_Camera.FindAction("LockOn", throwIfNotFound: true);
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

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_Keys;
    private readonly InputAction m_Camera_Move;
    private readonly InputAction m_Camera_Centre;
    private readonly InputAction m_Camera_SnapLeft;
    private readonly InputAction m_Camera_SnapRight;
    private readonly InputAction m_Camera_POV;
    private readonly InputAction m_Camera_LockOn;
    public struct CameraActions
    {
        private @CameraControls m_Wrapper;
        public CameraActions(@CameraControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Keys => m_Wrapper.m_Camera_Keys;
        public InputAction @Move => m_Wrapper.m_Camera_Move;
        public InputAction @Centre => m_Wrapper.m_Camera_Centre;
        public InputAction @SnapLeft => m_Wrapper.m_Camera_SnapLeft;
        public InputAction @SnapRight => m_Wrapper.m_Camera_SnapRight;
        public InputAction @POV => m_Wrapper.m_Camera_POV;
        public InputAction @LockOn => m_Wrapper.m_Camera_LockOn;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @Keys.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnKeys;
                @Keys.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnKeys;
                @Keys.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnKeys;
                @Move.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Centre.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnCentre;
                @Centre.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnCentre;
                @Centre.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnCentre;
                @SnapLeft.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnSnapLeft;
                @SnapLeft.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnSnapLeft;
                @SnapLeft.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnSnapLeft;
                @SnapRight.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnSnapRight;
                @SnapRight.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnSnapRight;
                @SnapRight.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnSnapRight;
                @POV.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnPOV;
                @POV.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnPOV;
                @POV.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnPOV;
                @LockOn.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnLockOn;
                @LockOn.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnLockOn;
                @LockOn.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnLockOn;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Keys.started += instance.OnKeys;
                @Keys.performed += instance.OnKeys;
                @Keys.canceled += instance.OnKeys;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Centre.started += instance.OnCentre;
                @Centre.performed += instance.OnCentre;
                @Centre.canceled += instance.OnCentre;
                @SnapLeft.started += instance.OnSnapLeft;
                @SnapLeft.performed += instance.OnSnapLeft;
                @SnapLeft.canceled += instance.OnSnapLeft;
                @SnapRight.started += instance.OnSnapRight;
                @SnapRight.performed += instance.OnSnapRight;
                @SnapRight.canceled += instance.OnSnapRight;
                @POV.started += instance.OnPOV;
                @POV.performed += instance.OnPOV;
                @POV.canceled += instance.OnPOV;
                @LockOn.started += instance.OnLockOn;
                @LockOn.performed += instance.OnLockOn;
                @LockOn.canceled += instance.OnLockOn;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface ICameraActions
    {
        void OnKeys(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnCentre(InputAction.CallbackContext context);
        void OnSnapLeft(InputAction.CallbackContext context);
        void OnSnapRight(InputAction.CallbackContext context);
        void OnPOV(InputAction.CallbackContext context);
        void OnLockOn(InputAction.CallbackContext context);
    }
}
