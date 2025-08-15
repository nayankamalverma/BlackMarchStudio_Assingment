using Assets.Scripts.Enemy.States;
using Assets.Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyController : IAI
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Vector2Int spawnPos;
        private EnemyStateMachine stateMachine;
        private void Start()
        {
            Spawn();
            CreateStateMachine();
            stateMachine.ChangeState(EnemyState.Ideal);
        }

        public void Spawn()
        {
            TileInfo tile = gridController.GeTileInfo(spawnPos.x, spawnPos.y);
            transform.position = tile.transform.position;
            tile.SetIsOccupied(true);
            currentPos.x = tile.X;
            currentPos.y = tile.Y;
        }

        private void CreateStateMachine()
        {
            stateMachine = new EnemyStateMachine(this);
        }

        private void Update()
        {
            stateMachine.Update();
        }
        public void ChangeState(EnemyState newState)
        {
            stateMachine.ChangeState(newState);
        }

        public PlayerController PlayerController => playerController;
        public Vector2Int CurrentPos => currentPos;
        public bool IsMoving => isMoving;
        public bool SetIsMoving(bool move) => isMoving=move;

        public float MoveSpeed => moveSpeed;
        public GridController GridController => gridController;

        public List<TileInfo> FindTilePath(Vector2Int A, Vector2Int B) => FindMovePath( A, B);

        public void MoveTo(List<TileInfo> path)
        {

            StartCoroutine(MoveAlongPath(path));
        }
    }
}