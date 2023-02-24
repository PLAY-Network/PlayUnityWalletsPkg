using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RGN.Impl.Firebase;
using RGN.Modules.Wallets;
using RGN.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Samples
{
    public sealed class WalletsExample : IUIScreen, System.IDisposable
    {
        [Header("Internal references")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _createWalletButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _scrollContentRectTrasform;
        [SerializeField] private CreateWalletDialog _createWalletDialog;
        [SerializeField] private LoadingIndicator _loadingIndicator;

        [Header("Prefabs")]
        [SerializeField] private WalletItem _walletItemPrefab;

        private List<WalletItem> _walletItems;

        public override Task InitAsync(IRGNFrame rgnFrame)
        {
            base.InitAsync(rgnFrame);
            _createWalletDialog.Init(this);
            _backButton.gameObject.SetActive(false);
            _backButton.onClick.AddListener(OnBackButtonClick);
            _createWalletButton.onClick.AddListener(OnCreateButtonClick);
            RGNCore.I.AuthenticationChanged += OnAuthenticationChangedAsync;
            return ReloadWalletItemsAsync();
        }
        protected override void Dispose(bool disposing)
        {
            _createWalletDialog.Dispose();
            _backButton.onClick.RemoveListener(OnBackButtonClick);
            _createWalletButton.onClick.RemoveListener(OnCreateButtonClick);
            RGNCore.I.AuthenticationChanged -= OnAuthenticationChangedAsync;
            DisposeWalletItems();
        }

        public override void SetVisible(bool visible)
        {
            base.SetVisible(visible);
            _backButton.gameObject.SetActive(true);
        }

        internal void SetUIInteractable(bool interactable)
        {
            _canvasGroup.interactable = interactable;
            _loadingIndicator.SetEnabled(!interactable);
        }
        internal async Task ReloadWalletItemsAsync()
        {
            DisposeWalletItems();
            SetUIInteractable(false);
            var walletsData = await WalletsModule.I.GetUserWalletsAsync();
            var wallets = walletsData.wallets;
            _walletItems = new List<WalletItem>(wallets.Length);
            for (int i = 0; i < wallets.Length; ++i)
            {
                WalletItem walletItem = Instantiate(_walletItemPrefab, _scrollContentRectTrasform);
                walletItem.Init(i, wallets[i]);
                _walletItems.Add(walletItem);
            }
            SetUIInteractable(true);
        }
        private void OnCreateButtonClick()
        {
            _createWalletDialog.SetVisible(true);
        }
        private void DisposeWalletItems()
        {
            if (_walletItems == null)
            {
                return;
            }
            for (int i =0; i < _walletItems.Count; ++i)
            {
                _walletItems[i].Dispose();
            }
            _walletItems.Clear();
        }
        private async void OnAuthenticationChangedAsync(EnumLoginState state, EnumLoginError error)
        {
            if (state == EnumLoginState.Success && RGNCore.I.AuthorizedProviders == EnumAuthProvider.Email)
            {
                await ReloadWalletItemsAsync();
            }
        }
    }
}
