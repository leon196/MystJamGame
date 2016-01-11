using System;
using UnityEngine;

[Serializable]
public class MouseLook : MonoBehaviour
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public float EquirectangleSensitivity = 0.1f;
    public float zoomSensitivity = 10f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;
    public float minFOV = 10f;
    public float maxFOV = 120f;
    public float minZoom = 0.1f;
    public float maxZoom = 1f;

    private Transform character;
    private Transform cameraTransform;
    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;
    private float fieldOfView;

    // equirectangle
    float angleX = 0f;
    float angleY = 3.14159f;
    float zoomEqui = 1f;
    float zoomEquiSmooth = 1f;

    // Transition
    Transition transition;

    void Start ()
    {
        character = transform;
        fieldOfView = Camera.main.fieldOfView;
        cameraTransform = GetComponentInChildren<Camera>().transform;
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = cameraTransform.localRotation;
        transition = GameObject.FindObjectOfType<Transition>();
    }


    void Update ()
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;

        // equirectangle
        angleX += Input.GetAxis("Mouse X") * EquirectangleSensitivity;
        angleY += Input.GetAxis("Mouse Y") * EquirectangleSensitivity;
        zoomEqui -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * Time.deltaTime;
        zoomEqui = Mathf.Clamp(zoomEqui, minZoom, maxZoom);
        Shader.SetGlobalFloat("_InputHorizontal", angleX);
        Shader.SetGlobalFloat("_InputVertical", angleY);
        zoomEquiSmooth = Mathf.Lerp(zoomEquiSmooth, zoomEqui, Time.deltaTime);
        Shader.SetGlobalFloat("_InputDepth", zoomEquiSmooth);

        m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

        if(clampVerticalRotation)
        m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

        if(smooth)
        {
            character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            cameraTransform.localRotation = Quaternion.Slerp (cameraTransform.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = m_CharacterTargetRot;
            cameraTransform.localRotation = m_CameraTargetRot;
        }

        if (transition.isInTransition) {
            fieldOfView = Mathf.Clamp(maxFOV / 2f + transition.transitionTimeRatio * (maxFOV - minFOV), minFOV, maxFOV);
        } else {
            fieldOfView = Mathf.Clamp(fieldOfView - zoom, minFOV, maxFOV);
        }

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fieldOfView, smoothTime * Time.deltaTime);

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if(!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
            InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
