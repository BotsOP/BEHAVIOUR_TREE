using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    private BTBaseNode tree;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        LayerMask targetMask = LayerMask.GetMask("player");
        LayerMask obstructionMask = LayerMask.GetMask("obstruction");
        BlackBoard blackBoard = new BlackBoard();
        blackBoard.SetValue("agent", agent);
        blackBoard.SetValue("gameObject", gameObject);

        tree = new BTParallelSelector(
            new BTSequence(
                new BTLook(gameObject, targetMask, obstructionMask, 10, 130, blackBoard),
                new BTChase(blackBoard,1, 5)
            ),
            
            new BTSequence(
                new BTMove(agent, RandomPos()),
                new BTWait(3f),
                new BTMove(agent, RandomPos()),
                new BTWait(3f)
            )

        );
    }
    
    private Vector3 RandomPos()
    {
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    void FixedUpdate()
    {
        tree?.Run();
    }
}



