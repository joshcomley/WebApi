using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreODataSample.Web.Controllers
{
    public class KeyValueBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var dic = new List<KeyValuePair<string, object>>();
            var key = "key";//  bindingContext.ModelName;
            foreach (var pair in bindingContext.ActionContext.RouteData.Values)
            {
                if (pair.Key == key)
                {
                    dic.Add(new KeyValuePair<string, object>(string.Empty, pair.Value));
                }
                else if (pair.Key.StartsWith($"{key}:"))
                {
                    dic.Add(new KeyValuePair<string, object>(pair.Key.Substring(key.Length + 1), pair.Value));
                }
            }
            bindingContext.Result = ModelBindingResult.Success(dic.ToArray());
            return Task.FromResult<object>(null);
        }
    }
}