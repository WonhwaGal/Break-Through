using UnityEngine;


public class CameraControl : MonoBehaviour
{
    [System.Serializable]
    public class CameraSettings
    {
        [Header("Camera Settings")]
        public float zoomSpeed = 5;
        public float moveSpeed = 5;
        public float rotationSpeed = 5;
        public float originalFieldOfView = 70;
        public float zoomFieldOfView = 60;
        public float mouseX_sensibility = 2;
        public float mouseY_sensibility = 2;
        public float MaxClampAngle = 90;
        public float MinClampAngle = -30;

        [Header("Camera Collision")]
        public Transform camPosition;
        public LayerMask camCollisionLayers;
    }
    [SerializeField]
    public CameraSettings cameraSettings;

    [System.Serializable]
    public class CameraInputSettings
    {
        public string MouseXAxis = "Mouse X";
        public string MouseYAxis = "Mouse Y";
        public string AimingInput = "Fire2";
    }
    [SerializeField]
    public CameraInputSettings inputSettings;

    public Transform pointer;
    public LayerMask shootAtMask;
    public LayerMask otherMasks;

    Transform center;
    Transform target;
    Camera mainCam;
    float cameraXRotation = 0;
    float cameraYRotation = 0;

    Vector3 initialCamPos;
    RaycastHit hit;
    float shootDis;
    Vector3 upForCam;  // will lift the start point to her waist
    //int invertY; если вставлять в меню опцию изменения

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        mainCam = Camera.main;
        center = transform.GetChild(0);
        initialCamPos = mainCam.transform.localPosition;
        upForCam = new Vector3(0, 3, 0);
        shootDis = 10.0f;
        //FindPlayer();
        if (PlayerPrefs.HasKey("masterSensitivity"))
        {
            cameraSettings.mouseX_sensibility = PlayerPrefs.GetInt("masterSensitivity");
            cameraSettings.mouseY_sensibility = PlayerPrefs.GetInt("masterSensitivity");
        }
        else
        {
            print("no data on Sensitivity");
        }
    }
    private void Update()
    {
        if (!target)
        {
            return;
        }
        if (!Application.isPlaying)
        {
            return;
        }
        RotateCamera();
        ZoomCamera();
        HandleCamCollision();
        ShowAimPointer();
    }
    void LateUpdate()
    {
        if (target)
        {
            FollowPlayer();
        }
        else
        {
            FindPlayer();
        }
    }
    void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void FollowPlayer()
    {
        Vector3 moveVector = Vector3.Lerp(transform.position, target.transform.position, cameraSettings.moveSpeed * Time.deltaTime);
        transform.position = moveVector;
    }
    void RotateCamera()
    {
        cameraYRotation += Input.GetAxis(inputSettings.MouseXAxis) * cameraSettings.mouseX_sensibility; //right-left
        cameraXRotation += Input.GetAxis(inputSettings.MouseYAxis) * cameraSettings.mouseY_sensibility * (-1); //  * invertY up-down

        cameraXRotation = Mathf.Clamp(cameraXRotation, cameraSettings.MinClampAngle, cameraSettings.MaxClampAngle);
        cameraYRotation = Mathf.Repeat(cameraYRotation, 360);

        Vector3 rotatingAngle = new Vector3(cameraXRotation, cameraYRotation, 0);
        Quaternion rotation = Quaternion.Slerp(center.transform.localRotation, Quaternion.Euler(rotatingAngle), cameraSettings.rotationSpeed * Time.deltaTime);
        center.transform.localRotation = rotation;
    }
    void ZoomCamera()
    {
        if (Input.GetButton(inputSettings.AimingInput))
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, cameraSettings.zoomFieldOfView, cameraSettings.zoomSpeed * Time.deltaTime);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, cameraSettings.originalFieldOfView, cameraSettings.zoomSpeed * Time.deltaTime);
        }
    }
    void HandleCamCollision()
    {
        if (Physics.Linecast(target.transform.position + upForCam, cameraSettings.camPosition.position, out hit, cameraSettings.camCollisionLayers))
        {
            Vector3 newCamPos = new Vector3(hit.point.x + hit.normal.x * 0.5f, hit.point.y + hit.normal.y * 1.0f, hit.point.z + hit.normal.z * 0.5f);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, newCamPos, Time.deltaTime * 2 * cameraSettings.moveSpeed);
        }
        else
        {
            mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, initialCamPos, Time.deltaTime * cameraSettings.moveSpeed);
        }
        Debug.DrawLine(target.transform.position + upForCam, cameraSettings.camPosition.position, Color.blue);
    }
    public Vector3 ShowAimPointer()
    {
        if (Input.GetButton(inputSettings.AimingInput))
        {
            Cursor.lockState = CursorLockMode.None;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, shootDis, shootAtMask))
            {
                pointer.gameObject.SetActive(true);
                pointer.position = hit.point;
                return hit.point;
            }
            else if (Physics.Raycast(ray, out hit, shootDis, otherMasks))
            {
                pointer.gameObject.SetActive(false);
                return hit.point;
            }
            else
            {
                pointer.gameObject.SetActive(false);
            }
        }
        if (Input.GetButtonUp(inputSettings.AimingInput))
        {
            pointer.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        return Vector3.zero;
    }
}
