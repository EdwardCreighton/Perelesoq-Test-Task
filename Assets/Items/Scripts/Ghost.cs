using UnityEngine;
using UnityEngine.Rendering;

namespace Project_PlayerInteractions.Items
{
	public class Ghost
	{
		#region Fields

		private GameObject ghostObject;

		#endregion

		#region Getters

		public Vector3 Position => ghostObject.transform.position;
		public Quaternion Rotation => ghostObject.transform.rotation;

		#endregion

		public Ghost(Item originalItem)
		{
			ghostObject = new GameObject(originalItem.name + " GHOST");

			GameObject meshObject = new GameObject("MESH")
			{
				transform =
				{
					parent = ghostObject.transform,
					localPosition = originalItem.PhysicCollider.transform.localPosition,
					localRotation = originalItem.PhysicCollider.transform.localRotation
				}
			};

			MeshFilter filter = meshObject.AddComponent<MeshFilter>();
			MeshRenderer renderer = meshObject.AddComponent<MeshRenderer>();

			filter.mesh = originalItem.Mesh;
			renderer.material = originalItem.GhostMaterial;
			renderer.shadowCastingMode = ShadowCastingMode.Off;

			ghostObject.transform.localScale = originalItem.transform.localScale;
		}

		public void SetPosition(Vector3 newPosition)
		{
			ghostObject.transform.position = newPosition;
		}

		public void SetRotation(Quaternion rotation)
		{
			ghostObject.transform.rotation = rotation;
		}

		public void SetTransformUp(Vector3 upDirection)
		{
			ghostObject.transform.up = upDirection;
		}

		public void Destroy()
		{
			Object.Destroy(ghostObject);
		}
	}
}
