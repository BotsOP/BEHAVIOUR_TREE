using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [Header("Vision Cone")]
    public float radius1 = 10;
    public float angle1 = 100;
    
    public float radius2 = 10;
    public float angle2 = 60;
    
    public float radius3 = 10;
    public float angle3 = 30;
    
    public float noticeTargetThreshold = 250;

    [Header("Patrol")] 
    public Transform[] patrolPoints;
    
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
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

        agent.SetDestination(new Vector3(10, 0, 10));
        //THIS IS THE MAIN TREE!!!!!!!
        tree = new BTParallelSelector(
            new BTSequence(
                enemyLookNode,
                new BTChase(blackBoard, 1, 5)
            ),

            //replace with moveTo -> animate -> checkDis -> animate -> wait
            new BTSequence(
                new BTMove(agent, patrolPoints[0]),
                new BTAnimate(new[] {1f}, new[] {"moveY"}, anim, 0.5f),
                new BTCheckDistanceAgent(agent, 0.3f),
                new BTAnimate(new[] {0f}, new[] {"moveY"}, anim, 0.5f),
                new BTCheckDistanceAgent(agent, 0.001f),
                new BTTurn(transform, patrolPoints[0], 0.5f),
                new BTLookAround(anim),
                
                new BTMove(agent, patrolPoints[1]),
                new BTAnimate(new[] {1f}, new[] {"moveY"}, anim, 0.5f),
                new BTCheckDistanceAgent(agent, 0.3f),
                new BTAnimate(new[] {0f}, new[] {"moveY"}, anim, 0.5f),
                new BTCheckDistanceAgent(agent, 0.01f),
                new BTTurn(transform, patrolPoints[1], 0.5f),
                new BTLookAround(anim)
                
                // new BTMove(agent, patrolPoints[2]),
                // new BTAnimate(new[] {1f}, new[] {"moveY"}, anim, 0.5f),
                // new BTCheckDistanceAgent(agent, 0.3f),
                // new BTAnimate(new[] {0f}, new[] {"moveY"}, anim, 0.5f),
                // new BTCheckDistanceAgent(agent, 0.001f),
                // new BTTurn(transform, patrolPoints[2], 0.5f),
                // new BTLookAround(anim),
                //
                // new BTMove(agent, patrolPoints[3]),
                // new BTAnimate(new[] {1f}, new[] {"moveY"}, anim, 0.5f),
                // new BTCheckDistanceAgent(agent, 0.3f),
                // new BTAnimate(new[] {0f}, new[] {"moveY"}, anim, 0.5f),
                // new BTCheckDistanceAgent(agent, 0.001f),
                // new BTTurn(transform, patrolPoints[3], 0.5f),
                // new BTLookAround(anim)
                
                
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