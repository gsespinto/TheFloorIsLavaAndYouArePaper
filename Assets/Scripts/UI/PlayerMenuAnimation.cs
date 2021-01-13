using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuAnimation : MonoBehaviour
{
    [SerializeField] private GameObject playerAvatar;
    [SerializeField] private Transform initialPos;
    [SerializeField] private Transform shopPos;    
    [SerializeField] private Transform levelsPos;
    [SerializeField] private float stopThreshold = 0.01f;
    [SerializeField] private float speed = 0.1f;

    private Animator avatarAnimator;
    [SerializeField] Button pauseAvatarBT;
    [SerializeField] Sprite pauseIcon;
    [SerializeField] Sprite playIcon;

    public bool GoToShop = false;
    public bool GoToLevels = false;

    // Start is called before the first frame update
    void Start()
    {
        playerAvatar.transform.position = initialPos.position;
        avatarAnimator = playerAvatar.GetComponent<Animator>();
        pauseAvatarBT.onClick.AddListener(PauseUpauseAvatar);
    }

    // Update is called once per frame
    void Update()
    {
        ShopAnimation();
        LevelAnimation();
        ReturnToInitial();
    }

    void ShopAnimation()
    {
        if (!GoToLevels)
        {
            if (GoToShop)
            {
                if (Vector3.Distance(playerAvatar.transform.position, shopPos.position) > stopThreshold)
                    playerAvatar.transform.position = Vector3.Lerp(playerAvatar.transform.position, shopPos.position, speed);

                if (Vector3.Distance(playerAvatar.transform.localScale, shopPos.localScale) > stopThreshold / 5)
                    playerAvatar.transform.localScale = Vector3.Lerp(playerAvatar.transform.localScale, shopPos.localScale, speed);
            }
        }
    }

    void LevelAnimation()
    {
        if (!GoToShop)
        {
            if (GoToLevels)
            {
                if (Vector3.Distance(playerAvatar.transform.position, levelsPos.position) > stopThreshold)
                    playerAvatar.transform.position = Vector3.Lerp(playerAvatar.transform.position, levelsPos.position, speed);

                if (Vector3.Distance(playerAvatar.transform.localScale, levelsPos.localScale) > stopThreshold / 5)
                    playerAvatar.transform.localScale = Vector3.Lerp(playerAvatar.transform.localScale, levelsPos.localScale, speed);
            }
        }
    }

    void ReturnToInitial()
    {
        if (!GoToLevels && !GoToShop)
        {
            if (Vector3.Distance(playerAvatar.transform.position, initialPos.position) > stopThreshold)
                playerAvatar.transform.position = Vector3.Lerp(playerAvatar.transform.position, initialPos.position, speed);

            if (Vector3.Distance(playerAvatar.transform.localScale, initialPos.localScale) > stopThreshold / 5)
                playerAvatar.transform.localScale = Vector3.Lerp(playerAvatar.transform.localScale, initialPos.localScale, speed);
        }
    }

    public void PauseUpauseAvatar()
    {
        if(avatarAnimator.speed != 0)
        {
            avatarAnimator.speed = 0;
            pauseAvatarBT.image.sprite = playIcon;
        }
        else
        {
            avatarAnimator.speed = 1;
            pauseAvatarBT.image.sprite = pauseIcon;
        }
    }

    public void UnpauseAvatar()
    {
        avatarAnimator.speed = 1;
        pauseAvatarBT.image.sprite = pauseIcon;
    }
}
