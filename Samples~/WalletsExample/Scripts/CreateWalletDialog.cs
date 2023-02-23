using RGN.Modules.Wallets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Samples
{
    internal sealed class CreateWalletDialog : MonoBehaviour, System.IDisposable
    {
        [SerializeField] private Button _createWalletButton;
        [SerializeField] private Button _closeDialogAreaButton;
        [SerializeField] private TMP_InputField _passwordInputField;

        private WalletsExample _walletsExample;

        internal void Init(WalletsExample walletsExample)
        {
            _walletsExample = walletsExample;
            _createWalletButton.onClick.AddListener(OnCreateWalletButtonClickAsync);
            _closeDialogAreaButton.onClick.AddListener(OnCloseButtonClick);
            SetVisible(false);
        }
        public void Dispose()
        {
            _createWalletButton.onClick.RemoveListener(OnCreateWalletButtonClickAsync);
            _closeDialogAreaButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        internal void SetVisible(bool visible)
        {
            _passwordInputField.text = string.Empty;
            gameObject.SetActive(visible);
        }

        private async void OnCreateWalletButtonClickAsync()
        {
            _walletsExample.SetUIInteractable(false);
            try
            {
                var result = await WalletsModule.I.CreateWalletAsync(_passwordInputField.text);
                if (!result.wallet_created || !result.success)
                {
                    Debug.LogError(result.error);
                    return;
                }
                SetVisible(false);
                await _walletsExample.ReloadWalletItemsAsync();
            }
            finally
            {
                _walletsExample.SetUIInteractable(true);
            }
        }
        private void OnCloseButtonClick()
        {
            SetVisible(false);
        }
    }
}
