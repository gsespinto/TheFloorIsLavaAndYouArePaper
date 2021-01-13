using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagPointerScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Transform flagTrans;
    [SerializeField]
    private Transform playerTrans;
    [SerializeField]
    private Image pointerImage;
    private RectTransform rectTrans;
    #endregion

    private void Start()
    {
        rectTrans = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        PointToFlag();
    }

    void PointToFlag()
    {
        Vector3 flagViewportPos = Camera.main.WorldToViewportPoint(flagTrans.position);


        if(flagViewportPos.x < 1 && flagViewportPos.x > 0 && flagViewportPos.y < 1 && flagViewportPos.y > 0)
        {
            pointerImage.enabled = false;
            return;
        }

        pointerImage.enabled = true;

        Vector3 screenPoint = Camera.main.ViewportToScreenPoint((flagViewportPos - Camera.main.WorldToViewportPoint(playerTrans.position)).normalized);

        rectTrans.anchoredPosition = screenPoint * 0.45f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, Vector3.SignedAngle(playerTrans.right, flagTrans.position - playerTrans.position, Vector3.forward));

        this.transform.rotation = rotation;
    }
}
