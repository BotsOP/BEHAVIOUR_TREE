using System.Collections;
using System.Collections.Generic;
using Behaviour_tree.Base_Nodes;
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
        
        //THIS IS THE MAIN TREE!!!!!!!
        tree = new BTSequence(
            new BTParallelSelector(
                enemyLookNode,

                //replace with moveTo -> animate -> checkDis -> animate -> wait
                new BTSequence(
                    new BTMove(agent, patrolPoints[0]),
                    new BTParallelComplete(
                        new BTAnimate(new[]
                        {
                            1f
                        }, new[]
                        {
                            "moveY"
                        }, anim, 0.5f),
                        new BTChangeSpeed(agent, 1.5F, 0.5f)
                    ),
                    new BTCheckDistanceAgent(agent, 0.3f),
                    new BTAnimate(new[]
                    {
                        0f
                    }, new[]
                    {
                        "moveY"
                    }, anim, 0.5f),
                    new BTCheckDistanceAgent(agent, 0.001f),
                    new BTTurn(transform, patrolPoints[0], 0.5f),
                    new BTLookAround(anim),

                    new BTMove(agent, patrolPoints[1]),
                    new BTAnimate(new[]
                    {
                        1f
                    }, new[]
                    {
                        "moveY"
                    }, anim, 0.5f),
                    new BTCheckDistanceAgent(agent, 0.3f),
                    new BTAnimate(new[]
                    {
                        0f
                    }, new[]
                    {
                        "moveY"
                    }, anim, 0.5f),
                    new BTCheckDistanceAgent(agent, 0.01f),
                    new BTTurn(transform, patrolPoints[1], 0.5f),
                    new BTLookAround(anim),

                    new BTMove(agent, patrolPoints[2]),
                    new BTAnimate(new[]
                    {
                        1f
                    }, new[]
                    {
                        "moveY"
                    }, anim, 0.5f),
                    new BTCheckDistanceAgent(agent, 0.3f),
                    new BTAnimate(new[]
                    {
                        0f
                    }, new[]
                    {
                        "moveY"
                    }, anim, 0.5f),
                    new BTCheckDistanceAgent(agent, 0.001f),
                    new BTTurn(transform, patrolPoints[2], 0.5f),
                    new BTLookAround(anim),

                    new BTMove(agent, patrolPoints[3]),
                    new BTAnimate(new[]
                    {
                        1f
                    }, new[]
                    {
                        "moveY"
                    }, anim, 0.5f),
                    new BTCheckDistanceAgent(agent, 0.3f),
                    new BTAnimate(new[]
                    {
                        0f
                    }, new[]
                    {
                        "moveY"
                    }, anim, 0.5f),
                    new BTCheckDistanceAgent(agent, 0.001f),
                    new BTTurn(transform, patrolPoints[3], 0.5f),
                    new BTLookAround(anim)
                )
            ),
            
            new BTSequence(
                enemyLookNode,
                new BTParallelComplete(
                    new BTAnimate(new[]
                    {
                        2f
                    }, new[]
                    {
                        "moveY"
                    }, anim, 0.5f),
                    new BTChangeSpeed(agent, 6, 0.5f),
                    new BTChaseSimple(agent, blackBoard)
                ),
                new BTChase(blackBoard, 1, 5)
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