using Core.Services.Page;
using UnityEngine;

namespace Core.Services.UI.Loading
{
    public sealed class LoadingPageView : PageView
    {
        [SerializeField] private SliderView _progress;
        [SerializeField] private TextView _status;

        public void SetProgress(float value) =>
            _progress.SetCurrentValue(value);

        public void SetStatus(string status) =>
            _status.SetText(status);
    }
}
