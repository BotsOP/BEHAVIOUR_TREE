using System.Collections;
using System.Collections.Generic;
using Behaviour_tree.Base_Nodes;
using Behaviour_tree.Decorator_Nodes;
using Behaviour_tree.Switch_Nodes;
using DefaultNamespace.Behaviour_tree;
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
    public Rigidbody grenadeRB;

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

        tree = new BTSwitchNode(
            new BTFailIfBool(blackBoard, "enemySeesUs"),
            new BTSequence(
                new BTSwitchNode(
                    new BTInvert(new BTCheckDistance(tooCloseToPlayer, player, transform)),

                    new BTSequence(
                        new BTMove(agent, transform),
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(1, "moveY")),
                            new BTChangeSpeed(agent, walkSpeed, 0.5f)
                        ),
                        new BTLookAt(transform, player, 10f, -50)
                    ),

                    new BTSequence(
                        new BTMove(agent, player),
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(1, "moveY")),
                            new BTChangeSpeed(agent, walkSpeed, 0.5f)
                        )
                    )
                )),

            new BTSequence(
                new BTParallelComplete(
                    new BTAnimate(anim, 0.5f, new AnimatePackage(2, "moveY")),
                    new BTChangeSpeed(agent, runSpeed, 0.5f)
                ),
                new BTSwitchNode(
                    new BTLook(gameObject, enemyMask, obstructionMask, visionRadius, visionAngle, blackBoard, "enemy"),

                    new BTSequence(
                        new BTDebug("not there cover"),
                        new BTCheckDistanceAgent(agent, 0.1f),
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(2, "moveY")),
                            new BTChangeSpeed(agent, runSpeed, 0.5f)
                        ),
                        
                        new BTIsAgainstWall(blackBoard, 1f, transform),
                        new BTLookAt(transform, player, 0.5f),
                        new BTDebug("has enough grenades"),
                        new BTAnimate(anim, 0.5f, new AnimatePackage(1, "throw")),
                        new BTWait(3f),
                        new BTActivateObject(grenade, true),
                        new BTParentObject(grenade, null),
                        new BTThrow(grenadeRB, player),
                        new BTWait(0.1f),
                        new BTActivateComponent(grenade, null, true),
                        new BTAnimate(anim, 0.5f, new AnimatePackage(0, "throw")),
                        new BTHasGrenade(maxGrenades)
                    ),
                
                new BTSwitchNode(
                    new BTLook(gameObject, obstructionMask, visionRadius, visionAngle, blackBoard, "cover", true),

                    new BTSequence(
                        new BTDebug("cant see obstruction"),
                        new BTMoveAwayFrom(agent, transform, blackBoard, "enemy"),
                        
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(2, "moveY")),
                            new BTChangeSpeed(agent, runSpeed, 0.5f)
                        )
                    ),
                    
                    new BTSequence(
                        new BTDebug("going behind cover"),
                        new BTGoBehindCover(agent, transform, blackBoard, "enemy", "cover"),
                        new BTIsAgainstWall(blackBoard, 0.5f, transform),
                        
                        new BTParallelComplete(
                            new BTAnimate(anim, 0.5f, new AnimatePackage(2, "moveY")),
                            new BTChangeSpeed(agent, runSpeed, 0.5f)
                        )
                        
                    ))
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
