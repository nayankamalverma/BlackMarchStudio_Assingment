using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class ChaseState : IState
    {
        public ChaseState(EnemyController controller) : base(controller){}

        public override void Enter()
        {
            controller.SetIsMoving( true);
            //controller.PlayerController;
            List<TileInfo> tiles = controller.PlayerController.adjacentTile;
            Vector2Int targetPos = new Vector2Int();
            foreach (TileInfo tile in tiles)
            {
                if (tile.IsWalkable)
                {
                    targetPos.x = tile.X;
                    targetPos.y = tile.Y;
                    break;
                }
            }
            List<TileInfo> path = controller.FindTilePath(controller.CurrentPos,targetPos);
            controller.MoveTo(path);
        }

        public override void Update()
        {
            if (!controller.IsMoving)
            {
                controller.ChangeState(EnemyState.Ideal);
            }
        }
    }
}