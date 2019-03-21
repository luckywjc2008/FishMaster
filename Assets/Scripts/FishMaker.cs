using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaker : MonoBehaviour
{
    [SerializeField]
    private Transform fishHolder;
    [SerializeField]
    private Transform[] genPositions;
    [SerializeField]
    private GameObject[] fishPrefabs;

    public float waveGenWaitTime = 0.3f;
    public float fishGenWaitTime = 0.5f;

	// Use this for initialization
	void Start ()
    {
		InvokeRepeating("MakeFishes",0,waveGenWaitTime);
	}

    void MakeFishes()
    {
        int genPosIndex = Random.Range(0, genPositions.Length);
        int fishPreIndex = Random.Range(0, fishPrefabs.Length);
        int maxNum = fishPrefabs[fishPreIndex].GetComponent<FishAttr>().maxNum;
        int maxSpeed = fishPrefabs[fishPreIndex].GetComponent<FishAttr>().maxSpeed;
        int num = Random.Range((maxNum/2) + 1, maxNum);
        int speed = Random.Range(maxSpeed/2, maxSpeed);
        int moveType = Random.Range(0, 2);//0代表直走，1代表转弯
        int angOffSet; //仅直走生效，直走的倾斜角
        int angSpeed;  //仅转弯生效，转弯的角速度

        if (moveType == 0)
        {
            //TODO 直走鱼群的生成
            angOffSet = Random.Range(-22, 22);
            StartCoroutine(GenStraightFish(genPosIndex,fishPreIndex,num,speed,angOffSet));
        }
        else
        {
            //TODO 转弯鱼群的生成
            if (Random.Range(0, 2) == 0)  //是否取负的角速度
            {
                angSpeed = Random.Range(-15, -9);
            }
            else
            {
                angSpeed = Random.Range(9, 15);
            }
            StartCoroutine(GenTrunFish(genPosIndex, fishPreIndex, num, speed, angSpeed));
        }

    }

    IEnumerator GenStraightFish(int genPosIndex, int FishPreIndex, int num, int speed, int angOffSet)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[FishPreIndex]);
            fish.transform.SetParent(fishHolder,false);
            fish.transform.localPosition = genPositions[genPosIndex].localPosition;
            fish.transform.localRotation = genPositions[genPosIndex].localRotation;
            fish.transform.Rotate(0,0,angOffSet);
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;
            fish.AddComponent<Ef_AutoMove>().speed = speed;
            yield return new WaitForSeconds(fishGenWaitTime);
        }
    }

    IEnumerator GenTrunFish(int genPosIndex, int FishPreIndex, int num, int speed, int angSpeed)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[FishPreIndex]);
            fish.transform.SetParent(fishHolder, false);
            fish.transform.localPosition = genPositions[genPosIndex].localPosition;
            fish.transform.localRotation = genPositions[genPosIndex].localRotation;
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;
            fish.AddComponent<Ef_AutoMove>().speed = speed;
            fish.AddComponent<Ef_AutoRotate>().speed = angSpeed;
            yield return new WaitForSeconds(fishGenWaitTime);
        }
    }
}
