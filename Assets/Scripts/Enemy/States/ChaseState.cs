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
            List<TileInfo> path = new List<TileInfo>();
            for (int i = 0; i < tiles.Count ; i++)
            {
                if (tiles[i].IsWalkable)
                {
                    targetPos.x = tiles[i].X;
                    targetPos.y = tiles[i].Y;
                    path = controller.FindTilePath(controller.CurrentPos, targetPos);
                    if(path!=null)break;
                }
            }
            controller.MoveTo(path);
        }

        public override void Update()
        {
            if (!controller.IsMoving)
            {
                controller.ChangeState(EnemyState.Ideal);
                controller.PlayerController.SetIsPlayersTurn(true);
            }
        }
    }
}