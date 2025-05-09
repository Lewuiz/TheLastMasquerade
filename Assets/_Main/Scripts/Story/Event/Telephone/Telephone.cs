using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class Telephone : MonoBehaviour
    {
        [SerializeField] private List<Button> telephoneButtonList = new List<Button>();
        [SerializeField] private CanvasGroup cg = default;
        [SerializeField] private string phoneNumberCall = default;
        [SerializeField] private TextMeshProUGUI phoneNumberTMP = default;

        public bool HasWin { get; private set; } = false;

        private string dialNumber = "";

        public void Init()
        {
            HasWin = false;
            SetOnButtonClickNumber();
        }

        private void SetOnButtonClickNumber()
        {
            for (int i = 0; i < telephoneButtonList.Count; i++)
            {
                string number = telephoneButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text;
                string capturedNumber = number;
                telephoneButtonList[i].onClick.RemoveAllListeners();
                telephoneButtonList[i].onClick.AddListener(() =>
                {
                    dialNumber += capturedNumber;
                    UpdatePhoneNumber();
                    CheckNumber();
                });
            }
        }

        private void CheckNumber()
        {
            for (int i = 0; i < dialNumber.Length; i++)
            {
                if (dialNumber[i] != phoneNumberCall[i])
                {
                    dialNumber = "";
                    UpdatePhoneNumber();
                    return;

                }
            }

            if(phoneNumberCall.Length == dialNumber.Length)
            {
                HasWin = true;
                Hide();
            }
        }

        public void Hide()
        {
            StartCoroutine(HideCor());
        }

        private IEnumerator HideCor()
        {
            yield return cg.DOFade(0f, .3f).WaitForCompletion();
            gameObject.SetActive(false);
        }

        private void UpdatePhoneNumber()
        {
            phoneNumberTMP.text = dialNumber;
        }

        public void Show()
        {
            StartCoroutine(ShowCor());
        }

        private IEnumerator ShowCor()
        {
            yield return cg.DOFade(1f, .3f).WaitForCompletion();
            gameObject.SetActive(true);
        }
    }
}
