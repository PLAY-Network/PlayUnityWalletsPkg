using System.Collections.Generic;
using System.Threading.Tasks;
using RGN.Impl.Firebase;
using RGN.Modules.Wallets;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Samples
{
    internal sealed class WalletsExample : IInitializable, System.IDisposable
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _createWalletButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _scrollContentRectTrasform;
        [SerializeField] private CreateWalletDialog _createWalletDialog;

        [SerializeField] private WalletItem _walletItemPrefab;

        private List<WalletItem> _walletItems;

        public override Task InitAsync()
        {
            _createWalletDialog.Init(this);
            _backButton.gameObject.SetActive(false);
            _createWalletButton.onClick.AddListener(OnCreateButtonClick);
            return ReloadWalletItemsAsync();
        }
        protected override void Dispose(bool disposing)
        {
            _createWalletDialog.Dispose();
            _createWalletButton.onClick.RemoveListener(OnCreateButtonClick);
            DisposeWalletItems();
        }

        internal void SetUIInteractable(bool interactable)
        {
            _canvasGroup.interactable = interactable;
        }
        internal async Task ReloadWalletItemsAsync()
        {
            DisposeWalletItems();
            _canvasGroup.interactable = false;
            var walletsData = await WalletsModule.I.GetUserWalletsAsync();
            var wallets = walletsData.wallets;
            _walletItems = new List<WalletItem>(wallets.Length);
            for (int i = 0; i < wallets.Length; ++i)
            {
                WalletItem walletItem = Instantiate(_walletItemPrefab, _scrollContentRectTrasform);
                walletItem.Init(i, wallets[i]);
                _walletItems.Add(walletItem);
            }
            _canvasGroup.interactable = true;
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
    }
}
