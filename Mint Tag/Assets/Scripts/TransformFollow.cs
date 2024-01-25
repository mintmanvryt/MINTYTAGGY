using UnityEngine;

public class TransformFollow : MonoBehaviour
{
	public Transform transformToFollow;

	public Vector3 offset;

	private void LateUpdate()
	{
		transform.rotation = transformToFollow.rotation;
		transform.position = transformToFollow.position + transformToFollow.rotation * offset;
	}
}
