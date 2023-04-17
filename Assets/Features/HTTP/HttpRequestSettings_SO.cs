using UnityEngine;

namespace Blueprints.Http
{
    [CreateAssetMenu(fileName = "New HTTP Request", menuName = "Http/Http Request")]
    public class HttpRequestSettings_SO : ScriptableObject
    {
        public string baseUrl = "https://api.bloktopia.com";

        public string path;

        public HttpMethod method;
    }
}
