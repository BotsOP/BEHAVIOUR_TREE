using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public float radius1 = 10;
    public float angle1 = 100;
    
    public float radius2 = 10;
    public float angle2 = 60;
    
    public float radius3 = 10;
    public float angle3 = 30;
    
    public float noticeTargetThreshold = 250;
    
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
        blackBoard.SetValue("noticeThreshhold", noticeTargetThreshold);
        blackBoard.SetValue("currentNotice", 0f);

        // tree = new BTParallelSelector(
        //     new BTSequence(
        //         new BTLook(gameObject, targetMask, obstructionMask, radius, angle, blackBoard, 60),
        //         new BTChase(blackBoard,1, 5)
        //     ),
        //     
        //     new BTSequence(
        //         new BTMove(agent, RandomPos()),
        //         new BTWait(3f),
        //         new BTMove(agent, RandomPos()),
        //         new BTWait(3f)
        //     )
        //
        // );
        BTBaseNode enemyLookNode = new BTSelector(
            new BTLookNoticeThreshhold(
                new BTLook(gameObject, targetMask, obstructionMask, radius1, angle1, blackBoard),
                blackBoard, 10
            ),
            new BTLookNoticeThreshhold(
                new BTLook(gameObject, targetMask, obstructionMask, radius2, angle2, blackBoard),
                blackBoard, 10
            ),
            new BTLookNoticeThreshhold(
                new BTLook(gameObject, targetMask, obstructionMask, radius3, angle3, blackBoard),
                blackBoard, 10
            )
        );
        
        //THIS IS THE MAIN TREE!!!!!!!
        tree = new BTParallelSelector(
            new BTSequence(
                enemyLookNode,
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