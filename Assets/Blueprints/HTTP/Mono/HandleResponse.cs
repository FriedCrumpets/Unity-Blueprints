using System.Collections.Generic;
using UnityEngine;

namespace Blueprints.Http
{
    interface IHttpResponseHandler
    {
         void OnGetResponse(HttpResponse response);
    }
}


