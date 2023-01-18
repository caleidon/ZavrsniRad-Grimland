// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""CameraControls"",
            ""id"": ""ea6200c7-c5ba-4642-b256-8decb65a8bf4"",
            ""actions"": [
                {
                    ""name"": ""Camera_Move"",
                    ""type"": ""Value"",
                    ""id"": ""e0c1389e-44cb-49f5-9021-755aa542e888"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera_Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""5848b10f-9176-4e33-bd66-6f0652708829"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePoint"",
                    ""type"": ""Value"",
                    ""id"": ""24b2db4d-e0dc-41ef-b58f-9f20ee5a6d8e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""178c03ab-6e84-4fb6-842c-da740836d9c3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f3d69886-8e6e-4f15-a976-08908f3e208b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""20f17aa7-d164-41cd-9487-66be31808ff1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7f440c8d-cbed-428e-b70f-bc6dac38f053"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""367cebe8-6951-48a4-8997-11ad63e2532c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""d60debcc-8222-4f82-8932-147e2a3606f4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4fd485ff-9dc7-4ba2-9936-c0b8b0c55e38"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""08fc786f-2006-4325-81cd-33f57d5ed5ee"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e1012b78-5430-4d2b-b715-a0356352dc4d"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2d7cc2dd-e0a9-4e78-8c4f-7285712c55b2"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""13403e55-d2e2-4cc8-bcc7-b6cdd0c48e06"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Camera_Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""791f23ff-912b-41de-994a-0e9f6f1685d6"",
                    ""path"": ""*/{Point}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""TimeControls"",
            ""id"": ""106d6613-4dd5-41ab-b75c-629e9fa6f8c8"",
            ""actions"": [
                {
                    ""name"": ""PauseOrResume"",
                    ""type"": ""Button"",
                    ""id"": ""1c017450-8f5a-44be-9e8f-c63d9d6670aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Set Speed - Normal"",
                    ""type"": ""Button"",
                    ""id"": ""e5aa65c1-8d39-4191-acba-6e092f0f5ae8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Set Speed - Fast"",
                    ""type"": ""Button"",
                    ""id"": ""f31a3136-cca4-4678-8ced-94c0bd95e04c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Set Speed - Fastest"",
                    ""type"": ""Button"",
                    ""id"": ""fdcca70c-e26d-4410-a75e-72a0db4a1118"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d0a2c2ce-4ade-4621-ac48-a1e564819a96"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseOrResume"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7087d68-1a68-4cce-9bdf-4221c4f76ac9"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Set Speed - Normal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""699fcae6-aa2d-42c2-939a-3a109aed8deb"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Set Speed - Fast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ffdb4924-2a70-49fd-9970-fd6a011c89a8"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Set Speed - Fastest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GameControls"",
            ""id"": ""138d3eee-e935-4fb9-8285-26ff58fdb005"",
            ""actions"": [
                {
                    ""name"": ""TogglePauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""dd7fa4e8-d607-4a08-9ff6-7e4eb54db2d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Click"",
                    ""type"": ""Button"",
                    ""id"": ""dbbe71b4-7e41-42f4-a54e-0634711edfb0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Click"",
                    ""type"": ""Button"",
                    ""id"": ""95f7768b-553a-4027-8cdc-efd2182c145f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Wood"",
                    ""type"": ""Button"",
                    ""id"": ""f05dd119-99bf-4081-a48e-ca9b87e42d20"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Blueprint"",
                    ""type"": ""Button"",
                    ""id"": ""c78e006f-c747-4803-8aa9-a5aa033a5a24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Human"",
                    ""type"": ""Button"",
                    ""id"": ""b1476363-5c1e-4c69-8ceb-8a94ac8f571c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""249e4637-2868-4757-9fc9-7b5f30ab1c92"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TogglePauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2cd26d9-ae53-463d-baf7-41fac0b3065a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e12cff7e-bbbb-4e52-a4f3-bca7889c2d44"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8472887d-c5fa-4cfd-bac1-1eac72c54e08"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Wood"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3bb40f59-a27f-43bb-ad71-72e60eed6c19"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Blueprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""46485cf8-110c-489a-bfcb-1aa0090e818a"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Human"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraControls
        m_CameraControls = asset.FindActionMap("CameraControls", throwIfNotFound: true);
        m_CameraControls_Camera_Move = m_CameraControls.FindAction("Camera_Move", throwIfNotFound: true);
        m_CameraControls_Camera_Zoom = m_CameraControls.FindAction("Camera_Zoom", throwIfNotFound: true);
        m_CameraControls_MousePoint = m_CameraControls.FindAction("MousePoint", throwIfNotFound: true);
        // TimeControls
        m_TimeControls = asset.FindActionMap("TimeControls", throwIfNotFound: true);
        m_TimeControls_PauseOrResume = m_TimeControls.FindAction("PauseOrResume", throwIfNotFound: true);
        m_TimeControls_SetSpeedNormal = m_TimeControls.FindAction("Set Speed - Normal", throwIfNotFound: true);
        m_TimeControls_SetSpeedFast = m_TimeControls.FindAction("Set Speed - Fast", throwIfNotFound: true);
        m_TimeControls_SetSpeedFastest = m_TimeControls.FindAction("Set Speed - Fastest", throwIfNotFound: true);
        // GameControls
        m_GameControls = asset.FindActionMap("GameControls", throwIfNotFound: true);
        m_GameControls_TogglePauseMenu = m_GameControls.FindAction("TogglePauseMenu", throwIfNotFound: true);
        m_GameControls_LeftClick = m_GameControls.FindAction("Left Click", throwIfNotFound: true);
        m_GameControls_RightClick = m_GameControls.FindAction("Right Click", throwIfNotFound: true);
        m_GameControls_Wood = m_GameControls.FindAction("Wood", throwIfNotFound: true);
        m_GameControls_Blueprint = m_GameControls.FindAction("Blueprint", throwIfNotFound: true);
        m_GameControls_Human = m_GameControls.FindAction("Human", throwIfNotFound: true);
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

    // CameraControls
    private readonly InputActionMap m_CameraControls;
    private ICameraControlsActions m_CameraControlsActionsCallbackInterface;
    private readonly InputAction m_CameraControls_Camera_Move;
    private readonly InputAction m_CameraControls_Camera_Zoom;
    private readonly InputAction m_CameraControls_MousePoint;
    public struct CameraControlsActions
    {
        private @Controls m_Wrapper;
        public CameraControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Camera_Move => m_Wrapper.m_CameraControls_Camera_Move;
        public InputAction @Camera_Zoom => m_Wrapper.m_CameraControls_Camera_Zoom;
        public InputAction @MousePoint => m_Wrapper.m_CameraControls_MousePoint;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterface != null)
            {
                @Camera_Move.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCamera_Move;
                @Camera_Move.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCamera_Move;
                @Camera_Move.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCamera_Move;
                @Camera_Zoom.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCamera_Zoom;
                @Camera_Zoom.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCamera_Zoom;
                @Camera_Zoom.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCamera_Zoom;
                @MousePoint.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMousePoint;
                @MousePoint.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMousePoint;
                @MousePoint.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMousePoint;
            }
            m_Wrapper.m_CameraControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Camera_Move.started += instance.OnCamera_Move;
                @Camera_Move.performed += instance.OnCamera_Move;
                @Camera_Move.canceled += instance.OnCamera_Move;
                @Camera_Zoom.started += instance.OnCamera_Zoom;
                @Camera_Zoom.performed += instance.OnCamera_Zoom;
                @Camera_Zoom.canceled += instance.OnCamera_Zoom;
                @MousePoint.started += instance.OnMousePoint;
                @MousePoint.performed += instance.OnMousePoint;
                @MousePoint.canceled += instance.OnMousePoint;
            }
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);

    // TimeControls
    private readonly InputActionMap m_TimeControls;
    private ITimeControlsActions m_TimeControlsActionsCallbackInterface;
    private readonly InputAction m_TimeControls_PauseOrResume;
    private readonly InputAction m_TimeControls_SetSpeedNormal;
    private readonly InputAction m_TimeControls_SetSpeedFast;
    private readonly InputAction m_TimeControls_SetSpeedFastest;
    public struct TimeControlsActions
    {
        private @Controls m_Wrapper;
        public TimeControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @PauseOrResume => m_Wrapper.m_TimeControls_PauseOrResume;
        public InputAction @SetSpeedNormal => m_Wrapper.m_TimeControls_SetSpeedNormal;
        public InputAction @SetSpeedFast => m_Wrapper.m_TimeControls_SetSpeedFast;
        public InputAction @SetSpeedFastest => m_Wrapper.m_TimeControls_SetSpeedFastest;
        public InputActionMap Get() { return m_Wrapper.m_TimeControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TimeControlsActions set) { return set.Get(); }
        public void SetCallbacks(ITimeControlsActions instance)
        {
            if (m_Wrapper.m_TimeControlsActionsCallbackInterface != null)
            {
                @PauseOrResume.started -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnPauseOrResume;
                @PauseOrResume.performed -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnPauseOrResume;
                @PauseOrResume.canceled -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnPauseOrResume;
                @SetSpeedNormal.started -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedNormal;
                @SetSpeedNormal.performed -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedNormal;
                @SetSpeedNormal.canceled -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedNormal;
                @SetSpeedFast.started -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedFast;
                @SetSpeedFast.performed -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedFast;
                @SetSpeedFast.canceled -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedFast;
                @SetSpeedFastest.started -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedFastest;
                @SetSpeedFastest.performed -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedFastest;
                @SetSpeedFastest.canceled -= m_Wrapper.m_TimeControlsActionsCallbackInterface.OnSetSpeedFastest;
            }
            m_Wrapper.m_TimeControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PauseOrResume.started += instance.OnPauseOrResume;
                @PauseOrResume.performed += instance.OnPauseOrResume;
                @PauseOrResume.canceled += instance.OnPauseOrResume;
                @SetSpeedNormal.started += instance.OnSetSpeedNormal;
                @SetSpeedNormal.performed += instance.OnSetSpeedNormal;
                @SetSpeedNormal.canceled += instance.OnSetSpeedNormal;
                @SetSpeedFast.started += instance.OnSetSpeedFast;
                @SetSpeedFast.performed += instance.OnSetSpeedFast;
                @SetSpeedFast.canceled += instance.OnSetSpeedFast;
                @SetSpeedFastest.started += instance.OnSetSpeedFastest;
                @SetSpeedFastest.performed += instance.OnSetSpeedFastest;
                @SetSpeedFastest.canceled += instance.OnSetSpeedFastest;
            }
        }
    }
    public TimeControlsActions @TimeControls => new TimeControlsActions(this);

    // GameControls
    private readonly InputActionMap m_GameControls;
    private IGameControlsActions m_GameControlsActionsCallbackInterface;
    private readonly InputAction m_GameControls_TogglePauseMenu;
    private readonly InputAction m_GameControls_LeftClick;
    private readonly InputAction m_GameControls_RightClick;
    private readonly InputAction m_GameControls_Wood;
    private readonly InputAction m_GameControls_Blueprint;
    private readonly InputAction m_GameControls_Human;
    public struct GameControlsActions
    {
        private @Controls m_Wrapper;
        public GameControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TogglePauseMenu => m_Wrapper.m_GameControls_TogglePauseMenu;
        public InputAction @LeftClick => m_Wrapper.m_GameControls_LeftClick;
        public InputAction @RightClick => m_Wrapper.m_GameControls_RightClick;
        public InputAction @Wood => m_Wrapper.m_GameControls_Wood;
        public InputAction @Blueprint => m_Wrapper.m_GameControls_Blueprint;
        public InputAction @Human => m_Wrapper.m_GameControls_Human;
        public InputActionMap Get() { return m_Wrapper.m_GameControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameControlsActions set) { return set.Get(); }
        public void SetCallbacks(IGameControlsActions instance)
        {
            if (m_Wrapper.m_GameControlsActionsCallbackInterface != null)
            {
                @TogglePauseMenu.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnTogglePauseMenu;
                @TogglePauseMenu.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnTogglePauseMenu;
                @TogglePauseMenu.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnTogglePauseMenu;
                @LeftClick.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnLeftClick;
                @RightClick.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnRightClick;
                @Wood.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnWood;
                @Wood.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnWood;
                @Wood.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnWood;
                @Blueprint.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnBlueprint;
                @Blueprint.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnBlueprint;
                @Blueprint.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnBlueprint;
                @Human.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnHuman;
                @Human.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnHuman;
                @Human.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnHuman;
            }
            m_Wrapper.m_GameControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TogglePauseMenu.started += instance.OnTogglePauseMenu;
                @TogglePauseMenu.performed += instance.OnTogglePauseMenu;
                @TogglePauseMenu.canceled += instance.OnTogglePauseMenu;
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @Wood.started += instance.OnWood;
                @Wood.performed += instance.OnWood;
                @Wood.canceled += instance.OnWood;
                @Blueprint.started += instance.OnBlueprint;
                @Blueprint.performed += instance.OnBlueprint;
                @Blueprint.canceled += instance.OnBlueprint;
                @Human.started += instance.OnHuman;
                @Human.performed += instance.OnHuman;
                @Human.canceled += instance.OnHuman;
            }
        }
    }
    public GameControlsActions @GameControls => new GameControlsActions(this);
    public interface ICameraControlsActions
    {
        void OnCamera_Move(InputAction.CallbackContext context);
        void OnCamera_Zoom(InputAction.CallbackContext context);
        void OnMousePoint(InputAction.CallbackContext context);
    }
    public interface ITimeControlsActions
    {
        void OnPauseOrResume(InputAction.CallbackContext context);
        void OnSetSpeedNormal(InputAction.CallbackContext context);
        void OnSetSpeedFast(InputAction.CallbackContext context);
        void OnSetSpeedFastest(InputAction.CallbackContext context);
    }
    public interface IGameControlsActions
    {
        void OnTogglePauseMenu(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnWood(InputAction.CallbackContext context);
        void OnBlueprint(InputAction.CallbackContext context);
        void OnHuman(InputAction.CallbackContext context);
    }
}
