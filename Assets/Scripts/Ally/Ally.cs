using System.Collections;
using System.Collections.Generic;
using Behaviour_tree.Base_Nodes;
using Behaviour_tree.Decorator_Nodes;
using Behaviour_tree.Switch_Nodes;
using UnityEngine;
using UnityEngine.AI;

public class Ally : MonoBehaviour
{
    public float tooCloseToPlayer;
    public Transform player;
    public float visionRadius;
    public float visionAngle;
    public Transform enemyTransform;

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
                    new BTMove(agent, transform)
                ),

                new BTSequence(
                    new BTMove(agent, player)
                )
            )),
            
            new BTSequence(
                //new BTDebug("running away"),
                new BTLook(gameObject, enemyMask, obstructionMask, visionRadius, visionAngle, blackBoard, "enemy"),
                
                new BTSwitchNode(
                    new BTLook(gameObject, obstructionMask, visionRadius, visionAngle, blackBoard, "cover", true),

                    new BTSequence(
                        new BTMoveAwayFrom(agent, transform, blackBoard, "enemy"),
                        new BTDebug("running away")
                        ),
                    
                    new BTSequence(
                        new BTGoBehindCover(agent, transform, blackBoard, "enemy", "cover"),
                        new BTDebug("hiding behind cover"),
                        new BTWait(1f)
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
