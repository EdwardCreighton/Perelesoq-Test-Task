using Project_PlayerInteractions.Items;
using UnityEditor;
using UnityEngine;

namespace Project_PlayerInteractions
{
	#if UNITY_EDITOR
	public static class MenuExtension
	{
		[MenuItem("GameObject/Interactions Project/Item")]
		private static void CreateItem()
		{
			Transform camera = SceneView.lastActiveSceneView.camera.transform;
			
			GameObject item = new GameObject("Item")
			{
				transform =
				{
					parent = null,
					position = camera.position + camera.forward * 5f,
					localRotation = Quaternion.identity
				},
				
				tag = "Item"
			};

			item.AddComponent<Item>();
			
			item.AddComponent<Rigidbody>();
			
			Animator animator = item.AddComponent<Animator>();
			animator.enabled = false;

			GameObject mesh = new GameObject("MESH")
			{
				transform =
				{
					parent = item.transform,
					localPosition = Vector3.zero,
					localRotation = Quaternion.identity
				}
			};

			mesh.AddComponent<MeshFilter>();
			mesh.AddComponent<MeshRenderer>();

			GameObject collider = new GameObject("COLLIDER")
			{
				transform =
				{
					parent = item.transform,
					localPosition = Vector3.zero,
					localRotation = Quaternion.identity
				}
			};

			collider.AddComponent<MeshCollider>().convex = true;

			GameObject interactablesHolder = new GameObject("INTERACTABLES")
			{
				transform =
				{
					parent = item.transform,
					localPosition = Vector3.zero,
					localRotation = Quaternion.identity
				}
			};
		}

		[MenuItem("GameObject/Interactions Project/Interactable")]
		private static void CreateInteractable()
		{
			Transform camera = SceneView.lastActiveSceneView.camera.transform;

			GameObject interactable = new GameObject("Interactable")
			{
				transform =
				{
					parent = Selection.activeTransform == null ? null : Selection.activeTransform,
					localPosition = Selection.activeTransform == null ? camera.position + camera.forward * 5f : Vector3.zero,
					localRotation = Quaternion.identity
				},
				
				tag = "Interactable"
			};

			interactable.AddComponent<Interactable>();
			
			Animator animator = interactable.AddComponent<Animator>();
			animator.applyRootMotion = true;
			
			Rigidbody rigidbody = interactable.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;

			interactable.AddComponent<MeshFilter>();
			interactable.AddComponent<MeshRenderer>();

			interactable.AddComponent<MeshCollider>();
		}
		
		[MenuItem("GameObject/Interactions Project/Applicable")]
		private static void CreateApplicable()
		{
			Transform camera = SceneView.lastActiveSceneView.camera.transform;

			GameObject applicable = new GameObject("Applicable")
			{
				transform =
				{
					parent = Selection.activeTransform == null ? null : Selection.activeTransform,
					localPosition = Selection.activeTransform == null ? camera.position + camera.forward * 5f : Vector3.zero,
					localRotation = Quaternion.identity
				},
				
				tag = "Applicable"
			};

			applicable.AddComponent<Applicable>();

			Rigidbody rigidbody = applicable.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;

			applicable.AddComponent<MeshFilter>();
			applicable.AddComponent<MeshRenderer>();

			applicable.AddComponent<MeshCollider>();
		}
	}
	#endif
}
