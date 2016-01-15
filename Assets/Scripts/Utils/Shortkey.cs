using UnityEditor;
using UnityEngine;

public class Shortkey : MonoBehaviour {

	[MenuItem ("MyMenu/ViewToSelected %q")]
	static void ViewToSelected() {
		EditorApplication.ExecuteMenuItem("GameObject/Align View to Selected");
	}
	[MenuItem ("MyMenu/ResetPosition %g")]
	static void ResetPosition() {
		Selection.activeTransform.localPosition = Vector3.zero;
	}
}