using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }
    public Text oneShootCostText;
    public Text goldText;
    public Text lvText;
    public Text lvNameText;
    public Text smallCountDownText;
    public Text bigCountDownText;
    public Button bigCountDownButton;
    public Button backButton;
    public Button settingButton;
    public Slider expSlider;

    public int exp = 0;
    public int gold = 500;
    public const int bigCountDown = 240;
    public const int smallCountDown = 60;

    public float bigTimer = bigCountDown;
    public float smallTimer = smallCountDown;

    public Color goldColor;

    public GameObject[] gunGos;

    public GameObject lvUpTips;
    public GameObject fireEffect;
    public GameObject changeEffect;
    public GameObject lvEffect;
    public GameObject goldEffect;

    public GameObject[] bullet1Gos;
    public GameObject[] bullet2Gos;
    public GameObject[] bullet3Gos;
    public GameObject[] bullet4Gos;
    public GameObject[] bullet5Gos;

    public int lv = 0;
    public Transform bulletHolder;

    public Image bgImage;
    public GameObject seaWaveEffect;
    public Sprite[] bgSprites;
    public int bgIndex = 0;

    //每一炮所需的金币数和造成的伤害值
    private int[] onShootCosts = {5,10,20,30,40,50,60,70,80,90,100,200,300,400,500,600,700,800,900,1000};
    //使用的是第几档的炮弹
    private int costIndex = 0;

    private string[] lvName = {"新手","入门","钢铁","青铜","白银","黄金","白金","钻石","大师","宗师"};

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        gold = PlayerPrefs.GetInt("gold", gold);
        lv = PlayerPrefs.GetInt("lv", lv);
        exp = PlayerPrefs.GetInt("exp", exp);
        smallTimer = PlayerPrefs.GetFloat("scd", smallCountDown);
        bigTimer = PlayerPrefs.GetFloat("bcd", bigCountDown);
        UpdateUI();
    }

    void Update()
    {
        ChangeBulletCost();
        Fire();
        UpdateUI();
        ChangeBg();
    }

    private void ChangeBg()
    {
        if (bgIndex != lv/20)
        {
            bgIndex = lv / 20;
            Instantiate(seaWaveEffect);
            AudioManager.Instance.PlayEffectSound(AudioManager.Instance.seaWaveClip);
            if (bgIndex>=3)
            {
                bgImage.sprite = bgSprites[3];
            }
            else
            {
                bgImage.sprite = bgSprites[bgIndex];
            }
        }
    }

    private void UpdateUI()
    {
        bigTimer -= Time.deltaTime;
        smallTimer -= Time.deltaTime;
        if (smallTimer <= 0)
        {
            smallTimer = smallCountDown;
            gold += 50;
        }

        if (bigTimer <= 0 && bigCountDownButton.gameObject.activeSelf == false)
        {
            bigTimer = bigCountDown;
            bigCountDownText.gameObject.SetActive(false);
            bigCountDownButton.gameObject.SetActive(true);
        }

        //经验计算公式:升级所需经验 = 1000+200*lv
        while (exp >= 1000+200*lv)
        {
            exp = exp - (1000 + 200 * lv);
            lv++;
            lvUpTips.SetActive(true);
            lvUpTips.transform.Find("Text").GetComponent<Text>().text = lv.ToString();
            StartCoroutine(lvUpTips.GetComponent<Ef_HideSelf>().HideSelf(0.6f));
            Instantiate(lvEffect);
            AudioManager.Instance.PlayEffectSound(AudioManager.Instance.lvUpClip);
        }

        goldText.text = "$"+gold.ToString();
        lvText.text = lv.ToString();
        if ((lv/10)<=9)
        {
            lvNameText.text = lvName[lv / 10];
        }
        else
        {
            lvNameText.text = lvName[9];
        }
        smallCountDownText.text = "  " + (int) (smallTimer / 10) + "  " + (int) (smallTimer % 10);
        bigCountDownText.text = (int)(bigTimer) + "s";
        expSlider.value = (float) exp / (1000 + 200 * lv);
    }

    private void Fire()
    {
        GameObject[] useBullets = bullet5Gos;
        int buttleIndex;
        if (Input.GetMouseButtonDown(0)&&EventSystem.current.IsPointerOverGameObject()==false)
        {
            if ((gold - onShootCosts[costIndex]) >= 0)
            {
                switch (costIndex / 4)
                {
                    case 0:
                        useBullets = bullet1Gos;
                        break;
                    case 1:
                        useBullets = bullet2Gos;
                        break;
                    case 2:
                        useBullets = bullet3Gos;
                        break;
                    case 3:
                        useBullets = bullet4Gos;
                        break;
                    case 4:
                        useBullets = bullet5Gos;
                        break;
                }
                buttleIndex = (lv % 10 >= 9 ? 9 : lv % 10);
                gold -= onShootCosts[costIndex];
                Instantiate(fireEffect);
                AudioManager.Instance.PlayEffectSound(AudioManager.Instance.fireClip);
                GameObject bullet = Instantiate(useBullets[buttleIndex]);
                bullet.transform.SetParent(bulletHolder, false);
                bullet.transform.position = gunGos[costIndex / 4].transform.Find("FirePos").transform.position;
                bullet.transform.rotation = gunGos[costIndex / 4].transform.Find("FirePos").transform.rotation;
                bullet.GetComponent<BulletAttr>().damage = onShootCosts[costIndex];
                bullet.AddComponent<Ef_AutoMove>().Dir = Vector3.up;
                bullet.GetComponent<Ef_AutoMove>().speed = bullet.GetComponent<BulletAttr>().speed;
            }
            else
            {
                //TODO Flash The Text
                StartCoroutine(GoldNotEnough());
            }


        }
    }

    private void ChangeBulletCost()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            OnButtonMDown();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            OnButtonPDown();
        }
    }

    public void OnButtonPDown()
    {
        gunGos[costIndex/4].SetActive(false);
        costIndex++;
        Instantiate(changeEffect);
        AudioManager.Instance.PlayEffectSound(AudioManager.Instance.changeClip);
        costIndex = (costIndex > onShootCosts.Length - 1) ? 0 : costIndex;
        gunGos[costIndex/4].SetActive(true);

        oneShootCostText.text = ("$" + onShootCosts[costIndex].ToString());
    }
    public void OnButtonMDown()
    {
        gunGos[costIndex / 4].SetActive(false);
        costIndex--;
        Instantiate(changeEffect);
        AudioManager.Instance.PlayEffectSound(AudioManager.Instance.changeClip);
        costIndex = (costIndex < 0)? onShootCosts.Length - 1 : costIndex;
        gunGos[costIndex / 4].SetActive(true);

        oneShootCostText.text = ("$" + onShootCosts[costIndex].ToString());
    }

    public void OnBigCountDownButton()
    {
        gold += 500;
        Instantiate(goldEffect);
        AudioManager.Instance.PlayEffectSound(AudioManager.Instance.rewardClip);
        bigCountDownButton.gameObject.SetActive(false);
        bigCountDownText.gameObject.SetActive(true);
        bigTimer = bigCountDown;
    }

    IEnumerator GoldNotEnough()
    {
        goldText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        goldText.color = goldColor;
    }

}
