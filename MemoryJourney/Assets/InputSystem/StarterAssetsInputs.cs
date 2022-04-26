using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public Vector2 lean;
		public bool jump;
		public bool sprint;
		public bool pick;
		
		

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputAction.CallbackContext value)
		{
			Vector2 vec = value.ReadValue<Vector2>();
			MoveInput(vec);
		}

		public void OnLook(InputAction.CallbackContext value)
		{
			if(cursorInputForLook)
			{
				Vector2 vec = value.ReadValue<Vector2>();
				LookInput(vec);
			}
		}

		public void OnJump(InputAction.CallbackContext value)
		{
			if(value.started)
            {
				JumpInput(true);
			}
			else if(value.canceled)
            {
				JumpInput(false);


			}
			
		}

		public void OnSprint(InputAction.CallbackContext value)
		{
			if(value.started)
            {
				SprintInput(true);
			}
			else if(value.canceled)
            {
				SprintInput(false);
			}
		}

		public void OnLean(InputAction.CallbackContext value)
        {
			Vector2 vec = value.ReadValue<Vector2>();
			LeanInput(vec);
        }

		public void OnPick(InputAction.CallbackContext value)
        {
			if(value.started)
            {
				PickInput(true);

			}
			else if(value.canceled)
            {
				PickInput(false);
			}
			
        }

#else
	// old input sys if we do decide to have it (most likely wont)...
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void LeanInput(Vector2 newLeanDirection)
        {
			lean = newLeanDirection;
		}
		public void PickInput(bool newPickState)
        {
			pick = newPickState;

        }

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}