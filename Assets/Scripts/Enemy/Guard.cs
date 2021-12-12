using System.Collections;
using System.Collections.Generic;
using Behaviour_tree.Base_Nodes;
using Behaviour_tree.Decorator_Nodes;
using Behaviour_tree.Switch_Nodes;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public AgentUI enemyUI;
    [Header("Vision Cone")]
    public float radius1 = 10;
    public float angle1 = 100;
    
    public float radius2 = 10;
    public float angle2 = 60;
    
    public float radius3 = 10;
    public float angle3 = 30;
    
    public float noticeTargetThreshold = 1500;
    public float noticeBuffer = 500;

    [Header("Patrol")] 
    public Transform patrolPoint;
    public GameObject head;
    public float walkSpeed;
    public float runSpeed;
    

    [Header("Attack")]
    public Transform crowbarInHand;

    private float animIdleFloat = 0;
    private float animWalkFloat = 1;
    private float animRunFloat = 2;
    
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
        LayerMask weaponMask = LayerMask.GetMask("weapon");
        LayerMask obstructionMask = LayerMask.GetMask("obstruction");
        BlackBoard blackBoard = new BlackBoard();
        blackBoard.SetValue("agent", agent);
        blackBoard.SetValue("gameObject", gameObject);
        blackBoard.SetValue("noticeThreshhold", noticeTargetThreshold);
        blackBoard.SetValue("currentNotice", 0f);
        blackBoard.SetValue("hasWeapon", false);

        Transform[] patrolPoints = new Transform[patrolPoint.childCount];
        for (int i = 0; i < patrolPoint.childCount; i++)
        {
            patrolPoints[i] = patrolPoint.GetChild(i);
        }
        
        BTBaseNode enemyLookNode = new BTSelector(
            
            new BTLookNoticeThreshhold(
                new BTLook(head, targetMask, obstructionMask, radius3, angle3, blackBoard, "target"),
                blackBoard, 10, noticeBuffer
            ),
            new BTLookNoticeThreshhold(
                new BTLook(head, targetMask, obstructionMask, radius2, angle2, blackBoard, "target"),
                blackBoard, 10, noticeBuffer
            ),
            new BTLookNoticeThreshhold(
                new BTLook(head, targetMask, obstructionMask, radius1, angle1, blackBoard, "target"),
                blackBoard, 10, noticeBuffer
            )
        );
        
        BTBaseNode[] patrolNodes = new BTBaseNode[patrolPoints.Length];
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolNodes[i] = new BTSequence(
                    new BTMove(agent, patrolPoints[i]),
                    new BTParallelComplete(
                        new BTAnimate(anim, 0.5f, new AnimatePackage(animWalkFloat, "moveY")),
                        new BTChangeSpeed(agent, walkSpeed, 0.5f)
                    ),
                    new BTCheckDistanceAgent(agent, 0.3f),
                    new BTAnimate(anim, 0.5f, new AnimatePackage(animIdleFloat, "moveY")),
                    new BTCheckDistanceAgent(agent, 0.001f),
                    new BTTurn(transform, patrolPoints[i], 0.5f),
                    new BTLookAround(anim)
                );
        }

        tree = new BTSwitchNode(
            new BTSequence(
                new BTUpdateSlider(0, noticeTargetThreshold, blackBoard, "currentNotice", enemyUI),
                new BTSelector(
                    new BTSequence(
                        new BTLook(head, weaponMask, obstructionMask, radius2, angle1, blackBoard, "weapon"),
                        new BTChangeText(enemyUI, TextDisplay.Happy),
                        new BTRaiseEvent(EventType.PLAYER_ESCAPED)),
                    new BTSequence(
                        enemyLookNode,
                        new BTChangeText(enemyUI, TextDisplay.Attack),
                        new BTRaiseEvent(EventType.PLAYER_SPOTTED)),
                    new BTInvert(new BTNoticeText(enemyUI, blackBoard, "currentNotice"))
                    )
            ),
             new BTSequence(
                 new BTDebug("going patrolling"),
                 new BTChangeText(enemyUI, TextDisplay.Patrolling),
                 new BTRaiseEvent(EventType.PLAYER_ESCAPED),
                 new BTSequence(patrolNodes)),
            
             new BTSequence(
                 new BTInvert(new BTSequence(
                     // check if already has a weapon
                     new BTInvert(new BTFailIfBool(blackBoard, "hasWeapon")),
                     // check to see if he sees weapon
                     new BTLook(head, weaponMask, obstructionMask, radius2, angle1, blackBoard, "weapon"),
                     // I see a weapon and start doing the pick weapon up behaviour
                     new BTRaiseEvent(EventType.PLAYER_ESCAPED),
                     new BTChangeText(enemyUI, TextDisplay.Happy),
                     
                     new BTParallelComplete(
                         new BTAnimate(anim, 0.5f, new AnimatePackage(animWalkFloat, "moveY")),
                         new BTChangeSpeed(agent, walkSpeed, 0.5f)
                     ),
                     
                     // walk to weapon
                     new BTMoveTo(agent, blackBoard, "weapon"),
                     new BTCheckDistanceAgent(agent, 0.5f),
                     // standing infront of weapon
                     new BTMove(agent, transform),
                     new BTAnimate(anim, 0.1f, new AnimatePackage(1, "weaponPickUp")),
                     new BTWait(1.2f),
                     new BTPickWeaponUp(blackBoard, crowbarInHand),
                     new BTAnimate(anim, 0.1f, new AnimatePackage(0, "weaponPickUp")),
                     new BTWait(5f),
                     new BTChangeObjectLayer(crowbarInHand.gameObject, 0)
                 )),
                 
                 enemyLookNode,
                 new BTDebug("see player"),
                 new BTChangeText(enemyUI, TextDisplay.Attack),
                 new BTMoveTo(agent, blackBoard, "target"),
                 new BTParallelComplete(
                     new BTAnimate(anim, 0.5f, new AnimatePackage(animRunFloat, "moveY")),
                     new BTChangeSpeed(agent, runSpeed, 0.5f)
                 ),
                 new BTMoveTo(agent, blackBoard, "target"),

                 //if enemy is in range attack
                 new BTSequence(
                     new BTRunningToFailed(new BTCheckDistanceAgent(agent, 3)),
                     new BTDebug("player is in range"),
                     new BTAnimate(anim, 0.1f, new AnimatePackage(1, "isAttacking")),
                     new BTAnimate(anim, 0.1f, new AnimatePackage(0, "isAttacking")),
                     new BTResetAnim(anim, new AnimatePackage(0, "isAttacking"))
                 )
             )
         );
    }

    void FixedUpdate()
    {
        tree?.Run();
    }
}