using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FPSController : MonoBehaviour
{

    struct Angle
    {
        public float pitch, yaw;
    }
    [SerializeField]
    private float _mouseSensitivity;

    private CharacterController _controller;

    [SerializeField]
    public GameObject cameraObject;

    [SerializeField]
    private bool _invertMouse;

    [SerializeField]
    private float _fov;

    public float jumpForce;
    public float gravity;


    private float _xmeow, _ymeow;
    private Vector3 _meowment;

    public float meowmentSpeed;


    private bool _isGrounded;

    [SerializeField]
    private Vector3 _pVelocity;

    private float _xrotation;

    [SerializeField]
    private GameObject _groundSensor;

    [SerializeField]
    private float _groundDistance;

    [SerializeField]
    private LayerMask _groundMask;




    [SerializeField]
    private float _bobSpeed, _bobAmplitude;

    private Vector3 _cameraPosition;

    public bool IsMoving;

    private float _timer;

    public float shootDamage;

    private GameObject _aimbotTarget;
    private bool _isAimbotting;
    [SerializeField]
    private float _aimbotAimSpeed;

    private bool _isShootCooldown;

    [SerializeField]
    private float _shootCooldown;

    [SerializeField]
    private ParticleSystem _shootParticle;

    //private int _curveControlPoints;

    [SerializeField]
    private float _dashPower, _dashDuration;
    [SerializeField]
    private float _dashCooldown;
    [SerializeField]
    private bool _isDashOnCooldown, _isDashing;

    private Vector3 _dashPosition;

    [SerializeField]
    private GameObject _viewmodelObject;

    [SerializeField]
    private Animator _animator;

    public float Fov
    {
        get
        {
            return Camera.main.fieldOfView;
        }
        set
        {
            Camera.main.fieldOfView = value;
        }
    }

    void Start()
    {

        Camera.main.fieldOfView = _fov;
        _cameraPosition = cameraObject.transform.localPosition;

        _ = (_controller = GetComponent<CharacterController>()) ?? (_controller = gameObject.AddComponent<CharacterController>()); // XDD

        _controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        _viewmodelObject.transform.parent = cameraObject.transform;
    }


    void Update()
    {
        if (GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.Paused) return;
        GetOtherInputs();
        MoveInputs();

        if(_isDashing)
        {
            transform.position = Vector3.Lerp(transform.position, _dashPosition, _dashDuration);
        }
    }
    void LateUpdate()
    {
        if (GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.Paused) return;
        LookInput();
    }

    private void GetOtherInputs()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
        if(Input.GetKeyUp(KeyCode.F))
        {
            _isAimbotting = !_isAimbotting;
        }
        if(_isAimbotting) 
        { 
            Aimbot(); 
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_isDashing) return;
            Dash(_meowment);
        }
    }


    private void Dash(Vector3 direction)
    {
        if (_isDashOnCooldown) return;

        _isDashing = true;
        _dashPosition = transform.position + (direction * _dashPower);

        StartCoroutine(DashCooldown());
    }

    public void Shoot()
    {
        if (_isShootCooldown) return;
        RaycastHit hit;

        _shootParticle.Play();

        _animator.Play("ShootAnim");

        StartCoroutine(ShootCooldown());
        if(Physics.Raycast(cameraObject.transform.position, cameraObject.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(cameraObject.transform.position, cameraObject.transform.forward * hit.distance, Color.red, 1);
            if(hit.transform.tag == "entity")
            {
                hit.transform.GetComponent<Entity>().CurrentHealth -= shootDamage;
            }
        }
    }

    private void MoveInputs()
    {
        _isGrounded = Physics.CheckSphere(_groundSensor.transform.position, _groundDistance, _groundMask);

        if (_isGrounded && _pVelocity.y < 0)
        {
            _pVelocity.y = 0f;
        }

        //_animator.SetBool("IsMoving", IsMoving);

        //pitäs varmaa limittaa se diagonal movement spiidi,,... jotenmkin,,,,,,,, tiedöäppä tuota sitten,,,,,, 
        if (_isGrounded) 
        {
            _xmeow = Input.GetAxis("Horizontal");
            _ymeow = Input.GetAxis("Vertical");
            _pVelocity.x = _xmeow;
            _pVelocity.z = _ymeow;
            _meowment = transform.forward * _ymeow + transform.right * _xmeow;
        }
        _controller.Move(_meowment * Time.deltaTime * meowmentSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            float v = -3.0f;
            _pVelocity.y += Mathf.Sqrt(jumpForce * v * gravity);
        }
        _pVelocity.y += gravity * Time.deltaTime;
        _controller.Move(_pVelocity * Time.deltaTime);

        if (_xmeow != 0 || _ymeow != 0)
        {
            if (!_isGrounded) return;

            Bob();
            IsMoving = true;
        }
        if (_xmeow == 0 && _ymeow == 0)
        {
            IsMoving = false;
            if (cameraObject.transform.position != _cameraPosition)
            {
                CancelBob();
            }
        }
    }

    private void LookInput()
    {
        if (_isAimbotting) return;
        float x = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.fixedUnscaledDeltaTime;
        float y = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.fixedUnscaledDeltaTime;

        _xrotation -= y;
        _xrotation = Mathf.Clamp(_xrotation, -90f, 90f);
        cameraObject.transform.localRotation = Quaternion.Euler(_xrotation, 0f, 0f);
        transform.Rotate(Vector3.up * x);
    }

    private void Bob()
    {
        _timer += 1 * Time.deltaTime;
        cameraObject.transform.position = new Vector3(
            cameraObject.transform.position.x,
            cameraObject.transform.position.y + Mathf.Sin(_timer / _bobSpeed) * _bobAmplitude, 
            cameraObject.transform.position.z
            );

        _viewmodelObject.transform.position = new Vector3(
            _viewmodelObject.transform.position.x,
            _viewmodelObject.transform.position.y + Mathf.Sin(_timer / _bobSpeed) * _bobAmplitude,
            _viewmodelObject.transform.position.z
            );
    }
    private void CancelBob()
    {
        //_timer = 0;
        cameraObject.transform.localPosition = Vector3.Lerp(cameraObject.transform.localPosition, _cameraPosition, _bobAmplitude * 2);
    }

    private void Aimbot()
    {
        _aimbotTarget = FindClosest();
        if (_aimbotTarget == null) 
        {
            _isAimbotting = false;
            return;
        }
        Quaternion angle = Quaternion.LookRotation(_aimbotTarget.transform.position - cameraObject.transform.position);

        //clamp from doing kuperkeikkas
        angle.x = Mathf.Clamp(angle.x, -90f, 90f);

        //freeze roll, only pitch and yaw
        cameraObject.transform.eulerAngles = new Vector3(cameraObject.transform.eulerAngles.x, cameraObject.transform.eulerAngles.y, 0);
        cameraObject.transform.rotation = Quaternion.Slerp(cameraObject.transform.rotation, angle, Time.deltaTime * _aimbotAimSpeed);
        /*Quaternion[] points = new Quaternion[_curveControlPoints];
        for(int i = 0; i < _curveControlPoints; i++)
        {
            _cameraObject.transform.rotation = Quaternion.Lerp(_cameraObject.transform.rotation, points[i], Time.deltaTime * _aimbotAimSpeed);
        }*/

        float angleDist = Vector3.Angle(_aimbotTarget.transform.position - cameraObject.transform.position, cameraObject.transform.forward);

        if (angleDist < 4f)
        {
            Shoot();
        }
    }
    private GameObject FindClosest()
    {
        if (GameManager.instance.EntitiesInWorld.Count <= 0) return null; 
        float dist = 9999f;
        GameObject target = null;
        foreach(GameObject t in GameManager.instance.EntitiesInWorld)
        {
            float d = Vector3.Distance(t.transform.position, transform.position);
            if(d < dist) 
            {
                dist = d;
                target = t;
            }
        }
        return target;
    }


    IEnumerator DashCooldown()
    {
        _isDashOnCooldown = true;
        yield return new WaitForSeconds(_dashCooldown);
        _isDashOnCooldown = false;
        _isDashing = false;
    }

    IEnumerator ShootCooldown()
    {
        _isShootCooldown = true;
        yield return new WaitForSeconds(_shootCooldown);
        _isShootCooldown = false;
    }



    //
    //
    //
    //
    //
    //     IF ATTE READS THIS VI VON ZULUL
    //
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠟⠋⠉⠉⠄⠐⠠⢀⠄⠄⠄⢀⡘⢦⡀⢀
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⢋⣤⣦⣤⣶⣶⣶⣦⡄⠄⠑⠄⠄⠐⠑⠄⣽⣷
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢱⣿⣿⣿⣿⣿⣿⣿⣿⣿⣆⠄⠄⠄⠄⠄⢸⣿⣿
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠁⢨⣿⣿⣿⣿⣿⣿⣿⡿⠗⠄⠄⠄⠄⠄⠸⣿⣿
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠏⠈⡀⠄⠠⠄⠄⠄⠄⢙⢗⡀⠄⣠⡀⠄⠄⠄⢸⣿
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⡘⠄⠄⠄⣠⡀⡄⠄⠄⢎⣿⡶⢶⠄⡃⠄⠄⠄⣼⣿
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⠁⠁⠈⠉⠉⢚⣍⣩⡀⠄⢀⠄⠺⠄⠄⠄⠄⣿⣿
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡄⠄⢒⡛⡛⠛⠛⢿⡁⠄⠉⠄⠄⠄⣠⡄⠸⣿⣿
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⠄⠉⢉⡉⡉⠁⠄⠁⢠⡎⠄⣀⢰⣿⠁⠄⢿⣿
    //    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⡄⠄⠄⠄⠄⠄⠄⢼⡇⢢⣏⠄⠄⠄⠄⣸⣿
    //    ⣿⣿⡿⠻⣿⣿⣿⣿⣿⣿⣿⡿⠛⠁⠄⠄⠄⠄⠄⠄⠙⠁⢠⣿⣷⣄⣀⠄⣿⣿
    //    ⣏⢀⠄⠄⢈⣯⣿⣿⣿⣿⣿⠇⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣼⣿⣿⣿⣿⣿⣿⣿
    //    ⡟⠛⠛⢛⣟⡛⠛⠛⣿⣿⣿⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⡸⣿⣿⣿⣿⢿⣿⣿⣿
    //    ⠁⠄⠄⢈⠉⠁⠄⠄⣹⣿⣿⠄⠄⠄⠄⠄⠄⠄⠄⢠⣾⣷⣿⣿⣭⣾⣿⣿⣿⣿
    //    ⠄⠄⠄⠄⠁⠄⠄⠄⢿⣿⣿⠄⠄⠄⠄⠄⠄⠄⢠⣿⣿⣿⣿⣿⡿⢻⣿⣿⣟⣿
    //
}
