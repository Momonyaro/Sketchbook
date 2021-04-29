using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Movement;

public class HPUI : MonoBehaviour
{
    [Tooltip("The character icon image")]
    public Image playerIcon;
    [Tooltip("The sprites of the different character icon states")]
    public Sprite[] iconSprites;
    [Tooltip("The image objects that will display the hearts")]
    public Image[] hpImages;
    [Tooltip("The sprites of the different heart states")]
    public Sprite[] hpSprites;

    int hpPerHeart = 4;

    public void UpdateUI(float currentHP, float maxHP, bool gotHurt)
    {
        StartCoroutine(HurtIconAnimaiton(currentHP, maxHP, gotHurt));
        ChangeHearts(currentHP);
    }

    void ChangeHearts(float currentHP)
    {
        bool empty = false;
        int i = 0;

        foreach (Image image in hpImages)
        {
            if (empty)
            {
                image.sprite = hpSprites[0];
            }
            else
            {
                i++;
                if (currentHP >= i * hpPerHeart)
                {
                    image.sprite = hpSprites[hpSprites.Length - 1];
                }
                else
                {
                    int currentHeartHP = (int)(hpPerHeart - (hpPerHeart * i - currentHP));
                    int hpPerImage = hpPerHeart / (hpSprites.Length - 1);
                    int imageIndex = currentHeartHP / hpPerImage;
                    image.sprite = hpSprites[imageIndex];
                    empty = true;
                }
            }
        }
    }

    IEnumerator HurtIconAnimaiton(float currentHP, float maxHP, bool gotHurt)
    {
        if (gotHurt)
            playerIcon.color = new Color(1.0f, 0.78f, 0.75f, 1.0f);

        float hpPercentage = currentHP / maxHP;
        if (hpPercentage <= 0.0f)
            playerIcon.sprite = iconSprites[0];
        else if (hpPercentage <= 1.0f / 3.0f)
            playerIcon.sprite = iconSprites[1];
        else if (hpPercentage <= 2.0f / 3.0f)
            playerIcon.sprite = iconSprites[2];
        else
            playerIcon.sprite = iconSprites[3];

        yield return new WaitForSeconds(0.15f);
        playerIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
