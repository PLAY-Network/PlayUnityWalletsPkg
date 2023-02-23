using RGN.Modules.Wallets;
using TMPro;
using UnityEngine;

namespace RGN.Samples
{
    internal sealed class WalletItem : MonoBehaviour, System.IDisposable
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _addressText;

        internal void Init(int index, RGNWallet wallet)
        {
            _rectTransform.localPosition = new Vector3(0, index * _rectTransform.sizeDelta.y, 0);
            _addressText.text = wallet.address;
        }
        public void Dispose()
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
