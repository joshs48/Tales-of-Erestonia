using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
	public GameObject[] items;

	[SerializeField] Cinemachine.CinemachineVirtualCamera shopCam;
	[SerializeField] GameObject shopkeeper;

	private bool canOpen = false;


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 3)
		{
			UIUpdater.CreateNotificationBar("Press 'shield' to Enter the Shop");
			canOpen = true;
			StartCoroutine(Open(other.gameObject));
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			canOpen = false;
		}
	}

	IEnumerator Open(GameObject player)
	{
		while (canOpen && !player.GetComponent<PlayerControl>().shield)
		{
			yield return new WaitForSeconds(0.01f);
		}
		if (player.GetComponent<PlayerControl>().shield)
		{
			RunShop();

			
		}
	}

	private void RunShop()
	{
		shopCam.Priority = 11;
		
	}

		/*
		 * FOR REFERENCE:
		 * private static IEnumerator OutlineNearestEnemy(int prevQSS)
		{
			GameObject currEnemyTarget = null;
			Shader prevShader = null;
			Material newMaterial;
			Texture prevTexture;
			while (game.CurQSS == prevQSS)
			{
				GameObject enemyTarget = game.player.GetComponent<PlayerControl>().FindNearestEnemy();
				if (enemyTarget != currEnemyTarget)
				{
					if (currEnemyTarget != null)
					{
						currEnemyTarget.GetComponentInChildren<SkinnedMeshRenderer>(false).material.shader = prevShader;
					}
					prevShader = enemyTarget.GetComponentInChildren<SkinnedMeshRenderer>(false).material.shader;
					newMaterial = enemyTarget.GetComponentInChildren<SkinnedMeshRenderer>(false).material;
					prevTexture = newMaterial.mainTexture;
					newMaterial.shader = game.outlineTargetShader;
					newMaterial.SetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b", game.targetColor);
					newMaterial.SetFloat("Vector1_75f17a0aa44e4590b2907dfe0d781665", game.targetPower);
					newMaterial.SetTexture("Texture2D_b4da09b4ebcb49edaa884d93d113e777", prevTexture);

				}
				currEnemyTarget = enemyTarget;
				yield return new WaitForSeconds(0.1f);

			}
			if (currEnemyTarget != null)
			{
				currEnemyTarget.GetComponentInChildren<SkinnedMeshRenderer>(false).material.shader = prevShader;
			}

		}
		*/

		/*MORE REFERENCE:
		 * IEnumerator Open(GameObject player)
		{
			while (canOpen && !player.GetComponent<PlayerControl>().shield)
			{
				yield return new WaitForSeconds(0.01f);
			}
			if (player.GetComponent<PlayerControl>().shield)
			{
				for (int i = 1; i < transform.childCount; i++)
				{
					transform.GetChild(i).gameObject.SetActive(true);
				}

				if (!isOpened)
				{
					GameObject lid = transform.Find("Lid").gameObject;

					for (int i = 0; i < 100; i++)
					{
						lid.transform.Rotate(Vector3.right, -0.5f);
						yield return new WaitForSeconds(0.004f);
					}
				}
				isOpened = true;
				ChestUIRunner.CreateChestUI(this);
			}




		}
			private void OnTriggerEnter(Collider other)
			{
				if (other.gameObject.CompareTag("Player"))
				{
				UIUpdater.CreateNotificationBar("Press 'shield' to open chest!");
				canOpen = true;
				StartCoroutine(Open(other.gameObject));
			}
			}

			private void OnTriggerExit(Collider other)
			{
			if (other.gameObject.CompareTag("Player"))
			{
				canOpen = false;
			}*/

	}
