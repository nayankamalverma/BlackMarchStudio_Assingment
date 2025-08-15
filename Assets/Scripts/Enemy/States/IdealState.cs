using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class IdealState : IState
    {
        public IdealState(EnemyController controller) : base(controller){}

        public override void Enter()
        {
            controller.SetIsMoving(false);
        }

        public override void Update()
        {
            if (!controller.PlayerController.isPlayersTurn && !controller.PlayerController.IsMoving())
            {
                controller.ChangeState(EnemyState.Chase);
            }
        }

    }
}