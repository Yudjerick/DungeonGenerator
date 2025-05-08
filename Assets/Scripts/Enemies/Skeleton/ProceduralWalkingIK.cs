using UnityEngine;

public class ProceduralWalkingIK : MonoBehaviour
{
    [Header("IK Targets")]
    public Transform leftFootIK;
    public Transform rightFootIK;
    public Transform body; // Добавляем ссылку на тело персонажа

    [Header("Parameters")]
    public float stepDistance = 0.5f;
    public float stepHeight = 0.2f;
    public float stepDuration = 0.5f;
    public float raycastDistance = 1f;
    public LayerMask groundLayer;

    private Vector3 leftFootTargetPosition;
    private Vector3 rightFootTargetPosition;
    private Vector3 leftFootBodyOffset;
    private Vector3 rightFootBodyOffset;

    private bool isLeftFootMoving;
    private bool isRightFootMoving;
    private float stepProgress;

    void Start()
    {
        // Сохраняем смещение ног относительно тела в локальных координатах
        leftFootBodyOffset = body.InverseTransformPoint(leftFootIK.position);
        rightFootBodyOffset = body.InverseTransformPoint(rightFootIK.position);

        // Инициализируем целевые позиции
        UpdateFootTargetPositions();
    }

    void Update()
    {
        // Обновляем позиции IK целей каждый кадр
        UpdateFootPositions();

        // Проверяем необходимость сделать шаг
        CheckForStep();

        // Обрабатываем движение ног, если они в процессе шага
        HandleFootMovement();
    }

    void UpdateFootPositions()
    {
        // Обновляем позиции IK целей, сохраняя их глобальные координаты
        if (!isLeftFootMoving)
        {
            leftFootIK.position = leftFootTargetPosition;
        }

        if (!isRightFootMoving)
        {
            rightFootIK.position = rightFootTargetPosition;
        }
    }

    void UpdateFootTargetPositions()
    {
        // Получаем текущие позиции ног относительно тела
        Vector3 leftDesiredPosition = body.TransformPoint(leftFootBodyOffset);
        Vector3 rightDesiredPosition = body.TransformPoint(rightFootBodyOffset);

        // Проекция на землю через raycast
        RaycastHit hit;
        if (Physics.Raycast(leftDesiredPosition + Vector3.up * raycastDistance / 2, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            leftFootTargetPosition = hit.point;
        }

        if (Physics.Raycast(rightDesiredPosition + Vector3.up * raycastDistance / 2, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            rightFootTargetPosition = hit.point;
        }
    }

    void CheckForStep()
    {
        // Получаем желаемые позиции ног относительно тела
        Vector3 leftDesiredPosition = body.TransformPoint(leftFootBodyOffset);
        Vector3 rightDesiredPosition = body.TransformPoint(rightFootBodyOffset);

        // Проекция на землю
        RaycastHit hit;
        Vector3 leftDesiredGroundPos = leftDesiredPosition;
        Vector3 rightDesiredGroundPos = rightDesiredPosition;

        if (Physics.Raycast(leftDesiredPosition + Vector3.up * raycastDistance / 2, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            leftDesiredGroundPos = hit.point;
        }

        if (Physics.Raycast(rightDesiredPosition + Vector3.up * raycastDistance / 2, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            rightDesiredGroundPos = hit.point;
        }

        // Проверяем расстояние от текущей позиции ноги до желаемой позиции на земле
        float leftDistance = Vector3.Distance(leftFootIK.position, leftDesiredGroundPos);
        float rightDistance = Vector3.Distance(rightFootIK.position, rightDesiredGroundPos);

        // Если нога слишком далеко и другая нога не движется, начинаем шаг
        if (leftDistance > stepDistance && !isRightFootMoving && !isLeftFootMoving)
        {
            StartLeftFootStep(leftDesiredGroundPos);
        }
        else if (rightDistance > stepDistance && !isLeftFootMoving && !isRightFootMoving)
        {
            StartRightFootStep(rightDesiredGroundPos);
        }
    }

    void StartLeftFootStep(Vector3 targetPosition)
    {
        isLeftFootMoving = true;
        stepProgress = 0f;
        leftFootTargetPosition = targetPosition;
    }

    void StartRightFootStep(Vector3 targetPosition)
    {
        isRightFootMoving = true;
        stepProgress = 0f;
        rightFootTargetPosition = targetPosition;
    }

    void HandleFootMovement()
    {
        if (isLeftFootMoving)
        {
            stepProgress += Time.deltaTime / stepDuration;

            if (stepProgress >= 1f)
            {
                stepProgress = 1f;
                isLeftFootMoving = false;
                leftFootBodyOffset = body.InverseTransformPoint(leftFootTargetPosition);
            }

            // Параболическая траектория для шага
            float height = Mathf.Sin(stepProgress * Mathf.PI) * stepHeight;
            leftFootIK.position = Vector3.Lerp(leftFootIK.position, leftFootTargetPosition, stepProgress) + Vector3.up * height;
        }

        if (isRightFootMoving)
        {
            stepProgress += Time.deltaTime / stepDuration;

            if (stepProgress >= 1f)
            {
                stepProgress = 1f;
                isRightFootMoving = false;
                rightFootBodyOffset = body.InverseTransformPoint(rightFootTargetPosition);
            }

            // Параболическая траектория для шага
            float height = Mathf.Sin(stepProgress * Mathf.PI) * stepHeight;
            rightFootIK.position = Vector3.Lerp(rightFootIK.position, rightFootTargetPosition, stepProgress) + Vector3.up * height;
        }
    }
}