using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Customer : MonoBehaviour
{
    private  NavMeshAgent agent;
    public NavMeshAgent Agent {  get { return agent; } }
    private CapsuleCollider capsuleCollider;

    public CustomerStates currentState = CustomerStates.WAITING_CUSTOMER;

    public UnityAction<Customer> OnTargetReached;

    public bool checkTarget;

 
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (checkTarget)
        {
         CheckTargetReached();
        }
    }

    private void CheckTargetReached()
    {
        const float DISTANCE_THRESHOLD = 0.21f;
        if (Vector3.Distance(agent.destination, transform.position) < DISTANCE_THRESHOLD)
        {
            OnTargetReached?.Invoke(this);
            checkTarget=false;
        }
    }

    public void MoveTo(Vector3 destination)
    {
        checkTarget = true;
        agent.SetDestination(destination);
    }

   

   /* public void FallAsleep(Transform sleepAnchor)
    {
        customerCurrentState = CustomerStates.SLEEP;
        agent.enabled = false;
        capsuleCollider.enabled = true;
        print("JUMPED");
        transform.DOJump(sleepAnchor.position,2f,1,1f);
        transform.rotation = sleepAnchor.rotation;
    }

    public void WakeUp()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        pos.z -= 1f;
        transform.position = pos;
        transform.eulerAngles = Vector3.up * -180f;
        agent.enabled = true;
        capsuleCollider.enabled = true;
    }
*/
}
