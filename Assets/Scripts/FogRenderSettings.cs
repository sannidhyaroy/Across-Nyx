using UnityEngine;

namespace MainScript
{
    public class FogRenderSettings : MonoBehaviour
    {
        private bool revertFogState = false;

        private void OnPreRender()
        {
            revertFogState = RenderSettings.fog;
            RenderSettings.fog = enabled;
        }

        private void OnPostRender()
        {
            RenderSettings.fog = revertFogState;
        }
    }
}
