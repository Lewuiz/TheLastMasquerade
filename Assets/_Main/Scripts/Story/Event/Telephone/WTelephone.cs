using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class TelephoneWindowData
    {
        public Action onGameCompleted;
    }

    public class WTelephone : WindowBase
    {
        [SerializeField] private List<Button> telephoneButtonList = new List<Button>();
        [SerializeField] private CanvasGroup cg = default;
        [SerializeField] private string phoneNumberCall = default;
        [SerializeField] private TextMeshProUGUI phoneNumberTMP = default;
        [SerializeField] private Transform rollNumberTr = default;
        private bool isPlayingAnimation = false;

        private TelephoneWindowData telephoneWindowData = default;

        private Dictionary<string, float> rotateNumberDict = new Dictionary<string, float>()
        {
            {"0", -327f},
            {"1", -59f},
            {"2", -87f},
            {"3", -115f},
            {"4", -139f},
            {"5", -172f},
            {"6", -203f},
            {"7", -233f},
            {"8", -261f},
            {"9", -295f},
        };

        public bool HasWin { get; private set; } = false;

        private string dialNumber = "";


        private void SetOnButtonClickNumber()
        {
            for (int i = 0; i < telephoneButtonList.Count; i++)
            {
                string number = telephoneButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text;
                string capturedNumber = number;
                telephoneButtonList[i].onClick.RemoveAllListeners();
                telephoneButtonList[i].onClick.AddListener(() =>
                {
                    if (isPlayingAnimation)
                        return;

                    dialNumber += capturedNumber;
                    DialNumberAnimation(capturedNumber);
                    UpdatePhoneNumber();
                    CheckNumber();
                });
            }
        }

        private void DialNumberAnimation(string capturedNumber)
        {
            StartCoroutine(DialNumberAnimationCor(capturedNumber));
        }

        private IEnumerator DialNumberAnimationCor(string capturedNumber)
        {
            isPlayingAnimation = true;
            yield return rollNumberTr.DORotate(new Vector3(0f, 0f, rotateNumberDict[capturedNumber]), 200f, RotateMode.FastBeyond360).SetSpeedBased(true).WaitForCompletion();
            yield return rollNumberTr.DORotate(new Vector3(0f, 0f, 360f), 200f, RotateMode.FastBeyond360).SetSpeedBased(true).WaitForCompletion();
            isPlayingAnimation = false;
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

            if (phoneNumberCall.Length == dialNumber.Length)
            {
                HasWin = true;
                CloseWindow();
                telephoneWindowData.onGameCompleted?.Invoke();
            }
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

        protected override void SetDefaultUI()
        {
            HasWin = false;
            SetOnButtonClickNumber();
            telephoneWindowData = data as TelephoneWindowData;
        }
    }
}
