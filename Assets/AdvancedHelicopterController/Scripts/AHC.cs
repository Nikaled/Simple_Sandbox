﻿using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEditor;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

namespace alelavoie
{
    public class AHC : MonoBehaviour
    {
        [SerializeField] private Transform _rotorMainTransform;
        [SerializeField] private Transform _rotorTailTransform;
        [SerializeField] private bool ExplosionOn;
        [HideInInspector]
        public Rigidbody HeliRigidbody;
        [HideInInspector]
        public ConstantForce HeliConstantForce;
        [HideInInspector]
        public PlayerInput PlayerInput;
        [HideInInspector]
        public AudioSource HeliAudioSource;

        [Tooltip("Transform representing the desired center of mass. Will use the Rigidbody's center of mass if left blank")]
        public Transform CenterOfMass;
        [Tooltip("Reference an eplosion prefab to be triggered upon crash")]
        public GameObject Explosion;
        [Tooltip("Reference the Rotor's animation. The speed of the animation will be proportional the the engine's readiness")]
        public Animation RotorAnim;

        [Tooltip("Minimum collision speed (in m/s) at which the helicopter explodes")]
        public float minVelocityExplosion = 6f;
        
        [Range(0f, 1)]
        [Tooltip("Motor strength: how easily the helicopter will gain (or lose) altitude")]        
        public float LiftStrength = 0.5f;

        [Range(0f, 1f)]
        [Tooltip("Helicopter's drag coeficient (wind resistance). Will affect top speed")]
        public float HelicopterDragCoefficient = 0.4f;

        [Range(0f, 5f)]
        [Tooltip("Drag coefficient applied under the rotor. Will push the heli up if the wind hits under the rotor")]
        public float RotorDragCoefficientUnder = 0.4f;

        [Range(0f, 1f)]
        [Tooltip("Drag coefficient applied above the rotor. Will push the heli down if the wind hits on top of the rotor")]
        public float RotorDragCoefficientAbove = 0.4f;

        [Range(0f, 60f)]
        [Tooltip("Angle at which the maximum lift strength starts decreasing. When idling, the helicopter won't lose altitude if the tilt " +
            "angle is smaller than the specified value.")]
        public float MaxLiftConservationAngle = 20f;

        [Range(0f, 20f)]
        [Tooltip("Torque strength applied when tilting forward or backward.")]
        public float PitchSensitivity = 0.4f;

        [Range(0f, 20f)]
        [Tooltip("Torque strength applied when tilting sideways.")]
        public float RollSensitivity = 0.4f;
        [Range(0f, 20f)]
        [Tooltip("Torque strength applied when rotating.")]
        public float YawSensitivity = 0.4f;

        [Tooltip("Check if you want the engine to be running on instantiation.")]
        public bool EngineRunning = true;

        [Tooltip("If throttling is enabled, it can be activated by pressing C to slowly reduce the rotor's lift and disable the controls. Will only work when helicopter is not moving. This is useful if you want the helicopter to stay stable on the ground.")]
        public bool EnableThrottling = false;

        [Tooltip("Apply a small resistance against any movement on the Y axis to help stabilize the helicopter when idling.")]
        public bool AltitudeStabilizer = true;

        [Tooltip(@"""Modulate the lift's orientation for a better experience. If the tilt angle is small (under 5 degrees), the lift's resulting angle will be dampened to add stability when trying to hover. In revenge, if the tilt angle is higher then 5 degrees, the resulting angle will be exagerated to increase horizontal acceleration""")]
        public bool ModulateLiftDirection;

        [Tooltip(@"""Here you can add an audio clip for the helicopter rotor sound. You can use the one provided in this asset as an example (search for helicopter_loop). The script will dynamically adjust the pitch of the audio clip based on the engine speed, providing a realistic sound experience as the helicopter starts or stops.""")]
        public AudioClip RotorSound;


        private AHC_Settings _settings;
        public AHC_Settings Settings
        {
            get { return _settings; }
        }

        private AHC_Controls _controls;
        public AHC_Controls Controls
        {
            get { return _controls; }
        }

        private AHC_Physics _physics;

        private AHC_Engine _engine;
        public AHC_Engine Engine
        {
            get { return _engine; }
        }
        
        private bool _useAnim = false;
        private bool _useExplosion = false;
        private bool _useSound = false;

        private bool _crashed = false;
        public bool Crashed
        {
            get { return _crashed; }
        }

        private bool _throttling = false;
        public bool Throttling
        {
            get { return _throttling; }
        }

        // Inertia Tensor varies with scale. And since I tested torque ranges with the following value, I'm using it
        // to adjust torque for different inertia tensor values.  
        // [HideInInspector]
        // public Vector3 benchmarkInertiaTensor = (3950.0f, 3621.8f, 3241.0f);

        void Awake()
        {            
            if (RotorAnim) _useAnim = true;
            if (Explosion) _useExplosion = true;
            
            //The order in which the following are called is important.
            InitRigidbody();
            InitConstantForce();
            _controls = new AHC_Controls(this);
            _settings = new AHC_Settings(this);
            _engine = new AHC_Engine(this);
            _physics = new AHC_Physics(this);
            InitAudioSource();  
            
        }
        public void MyInitializeButtons()
        {
            HelicopterButtons.instance.GoForward.GetComponent<HelicopterButton>().TranslatingFloat = 30f;
            HelicopterButtons.instance.GoForward.GetComponent<HelicopterButton>().ActionOnHold += OnPitchMobile;

            HelicopterButtons.instance.GoBack.GetComponent<HelicopterButton>().TranslatingFloat = -30f;
            HelicopterButtons.instance.GoBack.GetComponent<HelicopterButton>().ActionOnHold += OnPitchMobile;

            HelicopterButtons.instance.UpEngine.GetComponent<HelicopterButton>().TranslatingFloat = 1f;
            HelicopterButtons.instance.UpEngine.GetComponent<HelicopterButton>().ActionOnHold += OnCollectiveMobile;


            HelicopterButtons.instance.DownEngine.GetComponent<HelicopterButton>().TranslatingFloat = -1f;
            HelicopterButtons.instance.DownEngine.GetComponent<HelicopterButton>().ActionOnHold += OnCollectiveMobile;

            HelicopterButtons.instance.GoLeft.GetComponent<HelicopterButton>().TranslatingFloat = -4f;
            HelicopterButtons.instance.GoLeft.GetComponent<HelicopterButton>().ActionOnHold += OnYawMobile;

            HelicopterButtons.instance.GoRight.GetComponent<HelicopterButton>().TranslatingFloat = 4f;
            HelicopterButtons.instance.GoRight.GetComponent<HelicopterButton>().ActionOnHold += OnYawMobile;

            HelicopterButtons.instance.RollLeft.GetComponent<HelicopterButton>().TranslatingFloat = -20f;
            HelicopterButtons.instance.RollLeft.GetComponent<HelicopterButton>().ActionOnHold += OnRollMobile;

            HelicopterButtons.instance.RollRight.GetComponent<HelicopterButton>().TranslatingFloat = 20f;
            HelicopterButtons.instance.RollRight.GetComponent<HelicopterButton>().ActionOnHold += OnRollMobile;

        }
        public void MyClearButtons()
        {
            HelicopterButtons.instance.GoForward.GetComponent<HelicopterButton>().ActionOnHold -= OnPitchMobile;
            HelicopterButtons.instance.GoBack.GetComponent<HelicopterButton>().ActionOnHold -= OnPitchMobile;
            HelicopterButtons.instance.UpEngine.GetComponent<HelicopterButton>().ActionOnHold -= OnCollectiveMobile;
            HelicopterButtons.instance.DownEngine.GetComponent<HelicopterButton>().ActionOnHold -= OnCollectiveMobile;
            HelicopterButtons.instance.GoLeft.GetComponent<HelicopterButton>().ActionOnHold -= OnYawMobile;
            HelicopterButtons.instance.GoRight.GetComponent<HelicopterButton>().ActionOnHold -= OnYawMobile;
            HelicopterButtons.instance.RollLeft.GetComponent<HelicopterButton>().ActionOnHold -= OnRollMobile;
            HelicopterButtons.instance.RollRight.GetComponent<HelicopterButton>().ActionOnHold -= OnRollMobile;
            OnCollectiveMobile(-1);

        }
        void Start()
        {           
            
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        
        void Update()
        {            
            _engine.ProcessState();
            // Debug.Log(HeliRigidbody.inertiaTensor.magnitude);
            Vector3 hVelocity = new Vector3(HeliRigidbody.velocity.x, 0, HeliRigidbody.velocity.z);
            //Debug.Log(Mathf.Round(hVelocity.magnitude * 3.6f).ToString()); 
        }

        void LateUpdate()
        {
            _physics.ApplyForces();
            SyncRotorAnimSpeed();
            SyncAudioSourcePitch();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (ExplosionOn == false) return;
            if (collision.relativeVelocity.magnitude > minVelocityExplosion)
            {
                if (_useExplosion)
                {
                    Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
                }
                _crashed = true;
                gameObject.SetActive(false);
            }
        }

        public bool IsUpsideDown()
        {
            float angleHeliUpZenith = Vector3.Angle(Vector3.up, gameObject.transform.up);

            if (angleHeliUpZenith > 90)
            {
                return true;
            }
            return false;
        }
        private void InitRigidbody() {
            HeliRigidbody = gameObject.GetComponent<Rigidbody>();
            if (HeliRigidbody == null)
            {
                HeliRigidbody = gameObject.AddComponent<Rigidbody>();  
            }

            //Change center of mass if custom center of mass was specified. 
            if (CenterOfMass != null)
            {
                HeliRigidbody.centerOfMass = CenterOfMass.localPosition;
            }

            HeliRigidbody.drag = Mathf.Lerp(0.001f, 0.1f, HelicopterDragCoefficient);
            HeliRigidbody.mass = 500;
            HeliRigidbody.angularDrag = 10f;
            //Inertia Tensor varies with scale. The following value I what I used to adjust the different torque ranges 
            // HeliRigidbody.inertiaTensor = new Vector3(7492.6f, 7298.9f, 804.0f);            
            HeliRigidbody.useGravity = true;
        }
        private void InitConstantForce() {
            HeliConstantForce = gameObject.GetComponent<ConstantForce>();
            if (HeliConstantForce == null)
            {
                HeliConstantForce = gameObject.AddComponent<ConstantForce>();
            }
        }
        private void InitAudioSource() {
            if (!RotorSound) return;

            HeliAudioSource = gameObject.AddComponent<AudioSource>();
            HeliAudioSource.clip = RotorSound;
            HeliAudioSource.loop = true;
            _useSound = true;
            SyncAudioSourcePitch();
            HeliAudioSource.Play();
            
        }

        private void SyncRotorAnimSpeed()
        {
            MyRotorAnimation();
            if (_useAnim)
            {
                RotorAnim["Fly"].speed = _engine.EngineSpeed;
            }
        }
        public void MyRotorAnimation()
        {
                _rotorMainTransform.Rotate(Vector3.up * (_engine.EngineSpeed*15));
            _rotorTailTransform.Rotate(Vector3.left * (_engine.EngineSpeed * 15));
        }

        private void SyncAudioSourcePitch() {
            if (!_useSound) return;
            HeliAudioSource.pitch = _engine.EngineSpeed;
            HeliAudioSource.volume = _engine.EngineSpeed * 0.8f;
        }

        public void OnPitch(InputValue value)
        {
            //if (Geekplay.Instance.mobile)
            //{
            //    return;
            //}
            _controls.Pitch = value.Get<float>();
            _controls.PitchLastChanged = 0f;

        }
        public void OnPitchMobile(float value)
        {
            //if(value  == -1 && _controls.Pitch == 1)
            //{
            //    value = 0;
            //}
            //if (value == 1 && _controls.Pitch == -1)
            //{
            //    value = 0;
            //}
            _controls.Pitch = value;
            _controls.PitchLastChanged = 0f;
            Debug.Log("Pitching Value" + value);
        }

        public void OnYaw(InputValue value)
        {
            _controls.Yaw = value.Get<float>() *10;
            _controls.YawLastChanged = 0f;

        }
        public void OnYawMobile(float value)
        {
            //if (value == -1 && _controls.Yaw == 1)
            //{
            //    value = 0;
            //}
            //if (value == 1 && _controls.Yaw == -1)
            //{
            //    value = 0;
            //}
            _controls.Yaw = value;
            _controls.YawLastChanged = 0f;

        }
        public void OnRoll(InputValue value)
        {
            _controls.Roll = value.Get<float>();
            _controls.RollLastChanged = 0f;
        }
        public void OnRollMobile(float value)
        {
            //if (value == -1 && _controls.Roll == 1)
            //{
            //    value = 0;
            //}
            //if (value == 1 && _controls.Roll == -1)
            //{
            //    value = 0;
            //}
            _controls.Roll = value;
            _controls.RollLastChanged = 0f;
        }
        public void OnCollective(InputValue value)
        {
            float collectiveValue = value.Get<float>();

            if (collectiveValue > 0f) {
                Engine.Throttling = false;
            }
            if (!Engine.EngineOn)
                return;
            _controls.Collective = value.Get<float>();
        }
        public void OnCollectiveMobile(float value)
        {
            //if (value == -1 && _controls.Collective == 1)
            //{
            //    value = 0;
            //}
            //if (value == 1 && _controls.Collective == -1)
            //{
            //    value = 0;
            //}
            float collectiveValue = value;

            if (collectiveValue > 0f)
            {
                Engine.Throttling = false;
            }
            if (!Engine.EngineOn)
                return;
            _controls.Collective = value;
        }


        public void OnToggleEngine()
        {
            Engine.EngineOn = !Engine.EngineOn;
        }
        public void OnActivateThrottling()
        {
            if (HeliRigidbody.velocity.magnitude <= 0.1f) {
                Engine.Throttling = true;
            }
            
        }

    }
}
