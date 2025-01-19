namespace FoxCultGames.Gameplay.Entities.SubControllers
{
    using System;
    using GameData;
    using Settings;
    using PrimeTween;
    using UnityEngine;

    public class EntityMovementController : EntitySubController
    {
        private enum EntityState
        {
            Idle,
            Moving
        }

        private IGameContext gameContext;
        private EntityMovementSettings entityMovementSettings;
        private RectTransform entityTransform;
        
        private Vector2 targetPosition;
        private EntityState currentState;
        private Sequence movementSequence;
        private Sequence idleSequence;
        
        private float idleTimer;
        
        private bool canMove = true;

        protected override void Initialize(IGameContext gameContext, EntityController entityController)
        {
            this.gameContext = gameContext;
            entityTransform = (RectTransform)entityController.transform;
            entityMovementSettings = GameData.EntitiesSettings[entityController.EntityId].EntityMovementSettings;
            
            entityController.OnStartedDragging += () =>
            {
                canMove = false;
                ResetAll();
            };
            entityController.OnStoppedDragging += () =>
            {
                canMove = true;
                EnterIdleState();
            };
            
            EnterIdleState();
        }

        private void OnDestroy()
        {
            movementSequence.Stop();
            idleSequence.Stop();
        }

        protected override void UpdateController()
        {
            if (!canMove)
                return;
            
            switch (currentState)
            {
                case EntityState.Idle:
                    idleTimer -= Time.deltaTime;
                    if (idleTimer <= 0)
                    {
                        EnterMovingState();
                    }
                    break;
                case EntityState.Moving:
                    if (HasArrivedAtDestination())
                    {
                        EnterIdleState();
                        break;
                    }
                    
                    MoveEntity();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EnterIdleState()
        {
            currentState = EntityState.Idle;
            idleTimer = entityMovementSettings.IdleTime;
            
            movementSequence.Stop();
            ResetRotation();
            CreateIdleSequence();
        }

        private void EnterMovingState()
        {
            targetPosition = ClampBetweenBounds(GetRandomDestination());
            currentState = EntityState.Moving;
            
            idleSequence.Stop();
            ResetScale();
            CreateMovementSequence();
        }

        private void CreateMovementSequence()
        {
            movementSequence = Sequence.Create(cycles: -1, CycleMode.Yoyo)
                .Chain(Tween.Rotation(entityTransform, Quaternion.Euler(0f, 0f, entityMovementSettings.RotationChange), entityMovementSettings.RotationPerSideDuration))
                .Chain(Tween.Rotation(entityTransform, Quaternion.Euler(0f, 0f, -entityMovementSettings.RotationChange), entityMovementSettings.RotationPerSideDuration));
        }

        private void CreateIdleSequence()
        {
            idleSequence = Sequence.Create(cycles: -1, CycleMode.Yoyo)
                .Chain(Tween.Scale(entityTransform, Vector3.one * entityMovementSettings.ScaleModifier, entityMovementSettings.ScaleDuration))
                .Chain(Tween.Scale(entityTransform, Vector3.one, entityMovementSettings.ScaleDuration));
        }

        private void ResetAll()
        {
            movementSequence.Stop();
            idleSequence.Stop();
            ResetRotation();
            ResetScale();
        }

        private void ResetRotation()
        {
            entityTransform.rotation = Quaternion.identity;
        }
        
        private void ResetScale()
        {
            entityTransform.localScale = Vector3.one;
        }

        private Vector2 ClampBetweenBounds(Vector2 destination)
        {
            var rect = gameContext.EnclosureManager.CurrentEnclosure.Transform.rect;
            return new Vector2
            {
                x = Mathf.Clamp(destination.x, rect.min.x, rect.max.x),
                y = Mathf.Clamp(destination.y, rect.min.y, rect.max.y)
            };
        }

        private Vector2 GetRandomDestination()
        {
            var randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            return entityTransform.anchoredPosition + randomDirection * entityMovementSettings.MovementRange;
        }

        private bool HasArrivedAtDestination()
        {
            return Vector2.Distance(entityTransform.anchoredPosition, targetPosition) < entityMovementSettings.ArrivalDistance;
        }

        private void MoveEntity()
        {
            var direction = (targetPosition - entityTransform.anchoredPosition).normalized;
            entityTransform.anchoredPosition += direction * entityMovementSettings.MovementSpeed * Time.deltaTime;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPosition, 0.5f);
        }
    }
}