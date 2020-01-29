using System;
using UnityEditor;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Editors
{
	// TODO CHANGED: Not needed anymore in favor of 'MazeObjectEditor'
	//[CustomEditor(typeof(IBaseScriptableObject), true), CanEditMultipleObjects]
	//public class BaseScriptableObjectEditor : BaseEditor
	//{
	//	protected override void OnBeforeInitialized()
	//	{
	//		if (target is ScriptableObject && !(target is IBaseScriptableObject))
	//		{
	//			throw new InvalidOperationException("target is a ScriptableObject but not a IBaseScriptableObject. Please inherit IBaseScriptableObject");
	//		}

	//		if ((target as IBaseScriptableObject) == null)
	//		{
	//			Debug.LogWarning(string.Concat(new[] {
	//							"Casting target object to IBaseScriptableObject failed! Something's wrong. ",
	//							"Maybe you switched back and inherited ScriptableObject instead of IBaseScriptableObject ",
	//							"and you still had your gameObject selected? ",
	//							"If that's the case then the BaseScriptableObjectEditor is still there in memory ",
	//							"and so this could be resolved by reselcting your gameObject. ",
	//							"Destroying this BaseScriptableObjectEditor instance anyway..."
	//						}));

	//			DestroyImmediate(this);
	//		}
	//	}
	//}
}