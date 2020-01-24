public abstract class InputHandler : UnityEngine.MonoBehaviour
{
	public System.Action InputHapaned;

	public abstract void OnInputEvent();
}
