using System.Collections;
using System.Collections.Generic;
using Behaviour_tree.Base_Nodes;
using Behaviour_tree.Decorator_Nodes;
using Behaviour_tree.Switch_Nodes;
using DefaultNamespace.Behaviour_tree;
using DefaultNamespace.Behaviour_tree.Composite_Nodes;
using UnityEngine;
using UnityEngine.AI;

public class Ally : MonoBehaviour
{
    public float tooCloseToPlayer;
    public Transform player;
    public float visionRadius;
    public float visionAngle;
    public float walkSpeed;
    public float runSpeed;
    public float maxGrenades;
    public Transform enemyTransform;
    public GameObject grenade;
    public Transform handTransform;
    public AgentUI allyUI;

    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator anim;
    private BlackBoard blackBoard;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        EventSystem.Subscribe(EventType.PLAYER_SPOTTED, Spotted);
        EventSystem.Subscribe(EventType.PLAYER_ESCAPED, Escaped);
        
        LayerMask enemyMask = LayerMask.GetMask("enemy");
        LayerMask obstructionMask = LayerMask.GetMask("obstruction");

        blackBoard = new BlackBoard();
        blackBoard.SetValue("enemySeesUs", false);
        blackBoard.SetValue("enemy", enemyTransform);
        blackBoard.SetValue("isThrowing", false);

        Vector3 grenadePos = new Vector3(12.6f, 4.4f, -0.2f);
        //Vector3 grenadePos = Vector3.zero;
        Quaternion grenadeRotation = Quaternion.Euler(new Vector3(-92.47f, 0, 0));
        //Quaternion grenadeRotation = Quaternion.identity;
        //float grenadeScale = 76.41807f;
        float grenadeScale = 1;

        tree = new BTSwitchNode(
            new BTFailIfBool(blackBoard, "enemySeesUs"),
            new BTSequence(
                new BTSwitchNode(
                    new BTInvert(new BTCheckDistance(tooCloseToPlayer, player, transform)),

                    new BTSequence(
                        new BTDebug("idle"),
                        new BTChangeText(allyUI, TextDisplay.Idle),
                        new BTMove(agent, transform),
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(0, "moveY")),
                            new BTAnimate(anim, 0.5f, new AnimatePackage(0, "throw")),
                            new BTAnimate(anim, 0.5f, new AnimatePackage(0, "cover")),
                            new BTChangeSpeed(agent, walkSpeed, 0.5f)
                        ),
                        new BTLookAt(transform, player, 10f, -50)
                    ),

                    new BTSequence(
                        new BTDebug("walking to player"),
                        new BTMove(agent, player),
                        new BTChangeText(allyUI, TextDisplay.Following),
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(1, "moveY")),
                            new BTAnimate(anim, 0.5f, new AnimatePackage(0, "throw")),
                            new BTAnimate(anim, 0.5f, new AnimatePackage(0, "cover")),
                            new BTChangeSpeed(agent, walkSpeed, 0.5f)
                        )
                    )
                )),

            new BTSequence(
                new BTDebug("enemy sees us"),
                new BTParallelComplete(
                    new BTAnimate(anim, 0.5f, new AnimatePackage(2, "moveY")),
                    new BTChangeSpeed(agent, runSpeed, 0.5f)
                ),
                new BTSwitchNode(
                    new BTMimicIfSucceeds(
                        new BTInvert(new BTCheckBool(blackBoard, "isThrowing")), 
                        new BTLook(gameObject, enemyMask, obstructionMask, visionRadius, visionAngle, blackBoard, "enemy")
                        ),

        new BTSequence(
            new BTDebug("dont see enemy"),
                        new BTCheckDistanceAgent(agent, 0.1f),
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(2, "moveY")),
                            new BTChangeSpeed(agent, runSpeed, 0.5f)
                        ),

                        new BTSwitchNode(new BTIsAgainstWall(blackBoard, 1f, transform),
                            new BTSetBool(blackBoard, "isThrowing", false),
                            new BTDebug("against wall")
                            ),
                        
                        new BTSwitchNode(
                            new BTSequence(
                                new BTMoreThen(blackBoard, "amountGrenades", maxGrenades)
                            ),

                            new BTSequence(
                                new BTDebug("behind cover dont see enemy and no grenades"),
                                new BTChangeText(allyUI, TextDisplay.RunningAway),
                                new BTAnimate(anim, 0.1f, new AnimatePackage(1, "cover"))
                                ),

                            new BTParallelComplete(
                                new BTSequence(
                                    new BTDebug("behind cover dont see enemy and grenade"),
                                new BTSetBool(blackBoard, "isThrowing", true),
                                new BTChangeText(allyUI, TextDisplay.ThrowingGrenade),
                                new BTLookAt(transform, player, 0.5f),
                                new BTAnimate(anim, 0.5f, new AnimatePackage(1, "throw")),
                                new BTWait(3f),
                                new BTInstantiate(grenade, grenadePos, grenadeRotation, grenadeScale, handTransform, blackBoard, "grenade"),
                                new BTParentObject(blackBoard, "grenade", null),
                                new BTThrow(blackBoard, "grenade", player),
                                new BTWait(3f),
                                new BTHasGrenade(blackBoard, "amountGrenades"),
                                
                                new BTSwitchNode(new BTMoreThen(blackBoard, "amountGrenades", maxGrenades),
                                    new BTSequence(
                                        new BTSetBool(blackBoard, "isThrowing", false)
                                        ),
                                    new BTSequence(
                                        new BTWait(2f),
                                        new BTSetBool(blackBoard, "isThrowing", false)
                                        )
                                    )
                                ),
                                new BTLookAt(transform, player, 0.5f)
                                ))
                    ),

                    new BTSequence(
                        new BTDebug("see enemy and looking for cover"),

                        new BTSwitchNode(
                    new BTLook(gameObject, obstructionMask, visionRadius, visionAngle, blackBoard, "cover", true),

                    new BTSequence(
                        new BTMoveAwayFrom(agent, transform, blackBoard, "enemy"),
                        
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(2, "moveY")),
                            new BTChangeSpeed(agent, runSpeed, 0.5f)
                        )
                    ),
                    
                    new BTSequence(
                        new BTDebug("see enemy and walking toward cover"),
                        new BTGoBehindCover(agent, transform, blackBoard, "enemy", "cover"),
                        //new BTIsAgainstWall(blackBoard, 0.5f, transform),
                        
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(2, "moveY")),
                            new BTAnimate(anim, 0.1f, new AnimatePackage(0, "throw")),
                            new BTChangeSpeed(agent, runSpeed, 0.5f)
                        )
                        
                    )),
                        new BTChangeText(allyUI, TextDisplay.RunningAway)
                        )
                )
            ));
    }

    //how the fuck am I going to do this with nodes
    //its gonna be so jank and shit lmao
    private void Spotted()
    {
        blackBoard.SetValue("enemySeesUs", true);
    }

    private void Escaped()
    {
        blackBoard.SetValue("enemySeesUs", false);
    }

    void FixedUpdate()
    {
        tree?.Run();
    }
}
