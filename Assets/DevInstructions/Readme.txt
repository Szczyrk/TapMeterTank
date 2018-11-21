1. Animations

WON and LOST EVENTS

1) If you want to play an animation after game's win/lose and you want game over panel to be shown after your animation ends use TapPuzzleController.RegisterAnimation methods. In that case remember to run CallAnimationIsDone method (inherited from AnimationController class) after animation is done to inform controller about this.

2) If you want to play an animation after game's win/lose but you don't want to "block" game over panel until your animation's playing subscribe to Won/Lost events.


ANIMATION CONTROLLER

Animations that are registered 1) need to call inherited method CallAnimationIsDone from AnimationController classa after they are done. In other case controller will wait forever and it will never show game over panel.


END MATCH

Tap objects movement is smooth so after game win/lose event is called there is a possibility that objects are still moving a little. If your end game logic needs a tap object to stop, just wright a code like this to move it to the end position/rotation/scale immediately:

  [SerializeField]
  private TapPuzzleObject tapObject;
  if (tapObject != null)
        {
            tapObject.SetActive(false);		// Disable tap object's movement
            tapObject.MoveRotateAndScale(true); // Moves immediately
        }