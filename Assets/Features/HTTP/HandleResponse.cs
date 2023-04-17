using System.Collections.Generic;
using Http.Models;
using UnityEngine;

namespace Blueprints.Http
{
    interface IHttpResponseHandler
    {
         void OnGetResponse(HttpResponse response);
    }
}
