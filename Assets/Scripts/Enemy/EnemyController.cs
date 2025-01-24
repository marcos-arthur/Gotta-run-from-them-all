using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private int currentPathIndex;
    
    private List<Vector3> pathVectorList;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement() {
        if (pathVectorList != null) {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f) {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;
            } else {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving(){
        pathVectorList = null;

        SceneManager.LoadScene("TicTacToe");
    }

    public Vector3 GetPosition(){
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition){
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        if(pathVectorList != null && pathVectorList.Count > 1){
            pathVectorList.RemoveAt(0);
        }
    }
}
