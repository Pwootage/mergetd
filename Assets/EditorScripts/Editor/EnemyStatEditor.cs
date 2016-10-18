using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(EnemyStats))]
public class IngredientDrawer : PropertyDrawer {

	// Draw the property inside the given rect
	override public void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		float xpos = position.x;
		float ypos = position.y;
		float height = position.height;

		Rect hpLabel = new Rect(xpos, ypos, 20, height);
		xpos += 20;
		Rect hpRect = new Rect(xpos, ypos, 45, height);
		xpos += 50;
		Rect speedLabel = new Rect(xpos, ypos, 30, height);
		xpos += 30;
		Rect speedRect = new Rect(xpos, ypos, 25, height);
		xpos += 30;
		Rect valueLabel = new Rect(xpos, ypos, 15, height);
		xpos += 15;
		Rect valueRect = new Rect(xpos, ypos, position.width-90, height);

		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.LabelField(hpLabel, "HP");
		EditorGUI.PropertyField(hpRect, property.FindPropertyRelative("health"), GUIContent.none);
		EditorGUI.LabelField(speedLabel, "SPD");
		EditorGUI.PropertyField(speedRect, property.FindPropertyRelative("speed"), GUIContent.none);
		EditorGUI.LabelField(valueLabel, "$");
		EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}

#endif