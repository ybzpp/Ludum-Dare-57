using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Base setup")]
    public float StandHeight = 2f;
    public Vector3 StandCenter = Vector3.zero;
    public float CrouchHeight = 1f;
    public Vector3 CrouchCenter = new Vector3(0, .5f, 0f);
    public float StandCameraYOffset = 0.4f;
    public float CrouchCameraYOffset = 0.4f;

    [Header("Base setup")]
    public float WalkingSpeed = 2.7f;
    public float RunningSpeed = 4.5f;
    public float JumpSpeed = 8.0f;
    public float Gravity = 20.0f;
    public float LookSpeed = 2.0f;
    public float LookXLimit = 90.0f;

    [Header("Camera")]
    public float ShakePower = 0.4f;
    public float ShakeDuration = 0.4f;
    public Camera PlayerCamera;
    public Transform CameraParent;

    [Header("Footstep")]
    public FootstepSound FootstepSound;
    public float FootstepWalkDelay = .6f;
    public float FootstepRunDelay = .3f;
    private float _footstepTime = 0;

    private CharacterController _controller;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;
    private bool _canMove = true;
    private bool _canRotate = true;
    private bool _lockCrouhing = false;

    private void Awake()
    {
        if (!_controller)
        {
            _controller = GetComponent<CharacterController>();
        }

        Game.Player = this;
    }

    private float _sensitivityMultiplyer = 1f;
    public void SetSensitivityMultiplyer(float value)
    {
        _sensitivityMultiplyer = value;
    }

    void Start()
    {
        if (!PlayerCamera)
            PlayerCamera = Camera.main;

        PlayerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + StandCameraYOffset, transform.position.z);
        PlayerCamera.transform.SetParent(CameraParent);

        UnlockRotate();
        UnlockMove();
    }

    public void LockMove()
    {
        _canMove = false;
    }

    public void UnlockMove()
    {
        _canMove = true;
    }

    public void LockRotate()
    {
        _canRotate = false;
    }

    public void LockCrouhing()
    {
        _lockCrouhing = true;
    }

    public void UnlockCrouhing()
    {
        _lockCrouhing = false;
    }

    public void UnlockRotate()
    {
        _canRotate = true;
    }

    void Update()
    {
        if (Game.RuntimeData.IsPause || Game.RuntimeData.IsEnd)
            return;

        if (!_controller)
        {
            _controller = GetComponent<CharacterController>();
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        var inputX = Input.GetAxis("Vertical");
        var inputY = Input.GetAxis("Horizontal");

        bool isCrouhing = _lockCrouhing || Input.GetKey(KeyCode.C);
        _controller.height = isCrouhing ? CrouchHeight : StandHeight;
        _controller.center = isCrouhing ? CrouchCenter : StandCenter;

        var cameraPos = PlayerCamera.transform.localPosition;
        cameraPos.y = isCrouhing ? CrouchCameraYOffset : StandCameraYOffset;
        PlayerCamera.transform.localPosition = cameraPos;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = _canMove ? (isRunning ? RunningSpeed : WalkingSpeed) * inputX : 0;
        float curSpeedY = _canMove ? (isRunning ? RunningSpeed : WalkingSpeed) * inputY : 0;
        float movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && _canMove && _controller.isGrounded)
        {
            _moveDirection.y = JumpSpeed;
        }
        else
        {
            _moveDirection.y = movementDirectionY;
        }

        if (!_controller.isGrounded)
        {
            _moveDirection.y -= Gravity * Time.deltaTime;
        }

        _controller.Move(_moveDirection * Time.deltaTime);

        if (_canRotate && PlayerCamera != null)
        {
            _rotationX += -Input.GetAxis("Mouse Y") * (LookSpeed * _sensitivityMultiplyer);
            _rotationX = Mathf.Clamp(_rotationX, -LookXLimit, LookXLimit);
            PlayerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * (LookSpeed * _sensitivityMultiplyer), 0);
        }


        //footstep
        if (_moveDirection.OnlyXZ().magnitude > .2f)
        {
            _footstepTime += Time.deltaTime;

            var delay = isRunning ? FootstepRunDelay : FootstepWalkDelay;
            if (_footstepTime >= delay)
            {
                FootstepSound.PlayFootstepSound();
                _footstepTime = 0;
            }
        }
        else
        {
            _footstepTime = 0;
        }

        //
        CheckRoof();
    }

    public void Teleport(Transform point)
    {
        _controller.enabled = false;
        transform.position = point.position;
        transform.rotation = point.rotation;
        transform.parent = point;
        _controller.enabled = true;
        _rotationX = 0;
    }

    public float CheckRoofDistance = 1f;
    public void CheckRoof()
    {
        var start = transform.position + Vector3.up * _controller.height;
        Ray ray = new Ray(start, Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit hit, CheckRoofDistance))
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * CheckRoofDistance, Color.red, 1f);
            _moveDirection.y = -1;
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * CheckRoofDistance, Color.green, 1f);
        }
    }
}

