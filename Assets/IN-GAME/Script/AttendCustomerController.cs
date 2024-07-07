using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AttendCustomerController : MonoBehaviour
{
    private bool isPlayerAttending = false;
    [SerializeField] Hotel hotel;
    [SerializeField] WaitingQueueController waitingQueueController;
    [SerializeField] private CanvasGroup attendUI;
    [SerializeField] Image progressFill;
    [SerializeField] MoneyStack MoneyStack;

    private const float ATTEND_TIME = 3f;

    private Coroutine attendingCoroutine;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && hotel.CanAttendCustomer() && waitingQueueController.GetFirstCustomer() != null)
        {
            StartAttendingCoroutine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CancelAttendingCoroutine();
        }
    }

    private void StartAttendingCoroutine()
    {
        if (isPlayerAttending)
            return;

        isPlayerAttending = true;
        attendingCoroutine = StartCoroutine(AttendCustomerCoroutine());
    }

    private IEnumerator AttendCustomerCoroutine()
    {
        float timer = 0f;
        while (timer < ATTEND_TIME)
        {
            timer += Time.deltaTime;
            progressFill.fillAmount = timer / ATTEND_TIME;
            yield return null;
        }

        progressFill.fillAmount = 0;
        Customer customer = waitingQueueController.GetFirstCustomer();
        if (customer != null)
        {
            AssignRoom(customer);
        }
      //  CheckReceptionUI();
        isPlayerAttending = false;
    }

    private void AssignRoom(Customer customer)
    {
        RoomView availableRoom = hotel.GetAvailableRoom();
        if (availableRoom != null)
        {
            availableRoom.roomState.isOccupied = true;
            availableRoom.AssignedCustomer(customer);
            waitingQueueController.RemoveCustomerFromQueue(customer);
            MoneyStack.SpawnMoney();
        }
    }

    private void CancelAttendingCoroutine()
    {
        if (attendingCoroutine != null)
            StopCoroutine(attendingCoroutine);

        progressFill.fillAmount = 0;
        isPlayerAttending = false;
    }

}
