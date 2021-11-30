using System.Collections;
using System.Collections.Generic;
using Behaviour_tree.Base_Nodes;
using Behaviour_tree.Decorator_Nodes;
using Behaviour_tree.Switch_Nodes;
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
    public GameObject head;
    
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Attack")]
    public Transform crowbarInHand;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        LayerMask targetMask = LayerMask.GetMask("player");
        LayerMask weaponMask = LayerMask.GetMask("weapon");
        LayerMask obstructionMask = LayerMask.GetMask("obstruction");
        BlackBoard blackBoard = new BlackBoard();
        blackBoard.SetValue("agent", agent);
        blackBoard.SetValue("gameObject", gameObject);
        blackBoard.SetValue("noticeThreshhold", noticeTargetThreshold);
        blackBoard.SetValue("currentNotice", 0f);
        blackBoard.SetValue("hasWeapon", false);

        BTBaseNode enemyLookNode = new BTSelector(
            new BTLookNoticeThreshhold(
                new BTLook(head, targetMask, obstructionMask, radius3, angle3, blackBoard, "target"),
                blackBoard, 10
            ),
            new BTLookNoticeThreshhold(
                new BTLook(head, targetMask, obstructionMask, radius2, angle2, blackBoard, "target"),
                blackBoard, 10
            ),
            new BTLookNoticeThreshhold(
                new BTLook(head, targetMask, obstructionMask, radius1, angle1, blackBoard, "target"),
                blackBoard, 10
            )
        );

        BTBaseNode[] patrolNodes = new BTBaseNode[patrolPoints.Length];
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolNodes[i] = new BTSequence(
                    new BTMove(agent, patrolPoints[i]),
                    new BTParallelComplete(
                        new BTAnimate(anim, 0.5f, new AnimatePackage(1f, "moveY")),
                        new BTChangeSpeed(agent, 1.5F, 0.5f)
                    ),
                    new BTCheckDistanceAgent(agent, 0.3f),
                    new BTAnimate(anim, 0.5f, new AnimatePackage(0f, "moveY")),
                    new BTCheckDistanceAgent(agent, 0.001f),
                    new BTTurn(transform, patrolPoints[i], 0.5f),
                    new BTLookAround(anim)
                );
        }
        
        //THIS IS THE MAIN TREE!!!!!!!
        tree = new BTSequence(
            new BTParallelSelector(
                enemyLookNode,
                new BTSequence(
                    patrolNodes
                )),

            new BTSequence(
                //if see weapon pick it up
                new BTInvert(new BTSequence(
                    new BTInvert(new BTFailIfBool(blackBoard, "hasWeapon")),
                    new BTLook(head, weaponMask, obstructionMask, radius2, angle1, blackBoard, "weapon"),
                    new BTMoveTo(agent, blackBoard, "weapon"),
                    new BTCheckDistanceAgent(agent, 1f),
                    new BTAnimate(anim, 0.1f, new AnimatePackage(1, "weaponPickUp")),
                    new BTWait(1.2f),
                    new BTPickWeaponUp(blackBoard, crowbarInHand),
                    new BTAnimate(anim, 0.1f, new AnimatePackage(0, "weaponPickUp")),
                    new BTWait(5.5f)
                    )),
                
                //new BTDebug("going to target"),
                new BTMoveTo(agent, blackBoard, "target"),
                new BTParallelComplete(
                    new BTAnimate(anim, 0.5f, new AnimatePackage(2f, "moveY")),
                    new BTChangeSpeed(agent, 6, 0.5f)
                ),
                new BTMoveTo(agent, blackBoard, "target"),
                
                //if enemy is in range attack
                new BTSequence(
                    new BTRunningToFailed(new BTCheckDistanceAgent(agent, 3)),
                    new BTAnimate(anim, 0.1f, new AnimatePackage(1, "isAttacking")),
                    new BTAnimate(anim, 0.1f, new AnimatePackage(0, "isAttacking"))
                    ),

                new BTInvert(new BTRunningToFailed(new BTCheckDistanceAgent(agent, 30)))
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