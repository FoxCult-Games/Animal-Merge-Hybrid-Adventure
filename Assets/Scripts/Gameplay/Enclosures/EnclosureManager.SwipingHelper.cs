namespace FoxCultGames.Gameplay.Enclosures
{
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public partial class EnclosureManager
    {
        [Serializable]
        private class SwipingHelper
        {
            [SerializeField] private float swipeThreshold = 100f;
            
            private IGameContext gameContext;
            private IEnclosureManager enclosureManager;
            
            private Vector2 swipeStartPosition;
            private bool isSwiping;

            public void Initialize(IGameContext gameContext, IEnclosureManager enclosureManager)
            {
                this.gameContext = gameContext;
                this.enclosureManager = enclosureManager;
                
                gameContext.InputActions.Player.HoldStart.performed += Player_HoldStartPerformed;
                gameContext.InputActions.Player.HoldStart.canceled += Player_HoldStartCanceled;
            }

            public void Dispose()
            {
                gameContext.InputActions.Player.HoldStart.performed -= Player_HoldStartPerformed;
                gameContext.InputActions.Player.HoldStart.canceled -= Player_HoldStartCanceled;
            }

            public void Update()
            {
                if (!isSwiping)
                    return;
                
                var swipeEndPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                enclosureManager.CurrentEnclosure.Transform.anchoredPosition = GetSwipeDirection();
                
                Vector2 GetSwipeDirection()
                {
                    var swipeDirection = swipeEndPosition - swipeStartPosition;
                    swipeDirection.y = 0f;
                    return swipeDirection;
                }
            }

            private void Player_HoldStartPerformed(InputAction.CallbackContext ctx)
            {
                swipeStartPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                isSwiping = true;
            }

            private void Player_HoldStartCanceled(InputAction.CallbackContext ctx)
            {
                if (!isSwiping)
                    return;
                
                enclosureManager.CurrentEnclosure.Transform.anchoredPosition = Vector2.zero;
                
                var swipeEndPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                if (swipeEndPosition.x - swipeStartPosition.x > swipeThreshold)
                    enclosureManager.SwipeEnclosure(1);
                else if (swipeStartPosition.x - swipeEndPosition.x > swipeThreshold)
                    enclosureManager.SwipeEnclosure(-1);
                
                isSwiping = false;
            }
        }
        
        [SerializeField] private SwipingHelper swipingHelper;
    }
}