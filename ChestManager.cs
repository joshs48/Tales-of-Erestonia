using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public string chestName;
    public List<ItemManager> chestItems = new List<ItemManager>();
    public List<AmmoManager> chestAmmo = new List<AmmoManager>();

    private bool canOpen = false;
    private bool isOpened = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Open(GameObject player)
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
        }
    }

    public void RemoveItem(ItemManager item, AmmoManager ammo)
    {
        if (item != null)
        {
            chestItems.Remove(item);
            Destroy(transform.Find(item.gameObject.name).gameObject);
        }
        if (ammo != null)
        {
            chestAmmo.Remove(ammo);
            Destroy(transform.Find(ammo.gameObject.name).gameObject);

        }
    }


}
