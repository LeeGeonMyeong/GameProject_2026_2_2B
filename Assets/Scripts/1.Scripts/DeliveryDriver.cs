using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DeliveryDriver : MonoBehaviour
{
    [Header("배달원 설정")]
    public float moveSpeed = 8f;
    public float rotationSpeed = 10.0f;

    [Header("상태")]
    public float currentMoney = 0;

    public float batteryLevel = 100f;

    public int deliveryCount = 0;

    //Event 시스템
    [System.Serializable]
    public class DriverEvents
    {
        [Header("이동 Event")]
        public UnityEvent OnMoveStarted;
        public UnityEvent OnMoveStoped;

        [Header("상태 변화 Event")]
        public UnityEvent<float> OnMoneyChanged;
        public UnityEvent<float> OnBatteryChanged;
        public UnityEvent<int> OnDeliveryCountChanged;

        [Header("경고 Event")]
        public UnityEvent OnLowBattery;

        public UnityEvent OnLowBatterEmpty;

        public UnityEvent OnDeliveryCompleted;
    }

    public DriverEvents driverEvents;

    public bool isMoving = false;

    void Start()
    {
        //초기 상태 EVENT 발생
        driverEvents.OnMoneyChanged?.Invoke(currentMoney);
        driverEvents.OnBatteryChanged?.Invoke(batteryLevel);
        driverEvents.OnDeliveryCountChanged?.Invoke(deliveryCount);
    }

    void Update()
    {
        HandleMovement();
    }

  

    void ChangeBattery(float amount)
    {
        float oldBattery = batteryLevel;
        batteryLevel += amount;
        batteryLevel = Mathf.Clamp(batteryLevel, 0, 100);

        //배터리 변화 Event 발생
        driverEvents.OnBatteryChanged?.Invoke(batteryLevel);

        //배터리 상태에 따른 경고
        if (oldBattery > 20f && batteryLevel <= 20f)
        {
            driverEvents.OnLowBattery?.Invoke();                  //배터리 부족 이벤트 발생
        }
        if (oldBattery > 0 && batteryLevel <= 0)
        {
            driverEvents.OnLowBatterEmpty?.Invoke();              //배터리 방전 이벤트 발생
        }
    }

    void HandleMovement()
    {
        //배터리 체크
        if (batteryLevel <= 0)
        {
            if (isMoving)
            {
                StopMoving();
            }
            return;
        }

        //입력 받기
        Vector2 input = Keyboard.current != null ? new Vector2(
            (Keyboard.current.dKey.isPressed ? 1 : 0) -
            (Keyboard.current.aKey.isPressed ? 1 : 0),
            (Keyboard.current.wKey.isPressed ? 1 : 0) -
            (Keyboard.current.sKey.isPressed ? 1 : 0)
            ) : Vector2.zero;

        Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
    }

    

    void StartMoving()
    {
        isMoving = true;
        driverEvents.OnMoveStarted?.Invoke();
    }

    void StopMoving()
    {
        isMoving = false;
        driverEvents.OnMoveStoped?.Invoke();
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount;
        driverEvents.OnMoneyChanged?.Invoke(currentMoney);              //돈 획득 후 이벤트 처리
    }

    public void CompleteDelivery()
    {
        deliveryCount++;
        float reward = Random.Range(3000, 8000);
        AddMoney(reward);
        driverEvents.OnDeliveryCountChanged?.Invoke(deliveryCount);
        driverEvents.OnDeliveryCompleted?.Invoke();
    }

    public void ChargeBattery()
    {
        ChangeBattery(100f -  batteryLevel);                             //배터리 완충
    }

    public string GetStatusText()
    {
        return $"돈 : {currentMoney:F0} 원 | 배터리 : {batteryLevel:F1}% | 배달 : {deliveryCount} 건";
    }

    public bool CanMove()
    {
        return batteryLevel > 0;
    }
}
