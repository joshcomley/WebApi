using System.Collections.Generic;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData.Query
{
    internal class ExpressionHookRegistrar
    {
        public static ExpressionHookRegistrar Instance { get; } = new ExpressionHookRegistrar();

        internal Dictionary<IEdmModel, List<ExpressionHook>> Hooks { get; set; }

        public List<ExpressionHook> For(IEdmModel model)
        {
            if (Hooks != null && Hooks.ContainsKey(model))
            {
                return Hooks[model];
            }

            return null;
        }

        public void Register(IEdmModel model, ExpressionHook hook)
        {
            Init(model);
            Hooks[model].Add(hook);
        }

        public void Unregister(IEdmModel model, ExpressionHook hook)
        {
            Init(model);
            Hooks[model].Remove(hook);
        }

        private void Init(IEdmModel model)
        {
            Hooks = Hooks ?? new Dictionary<IEdmModel, List<ExpressionHook>>();
            if (!Hooks.ContainsKey(model))
            {
                Hooks.Add(model, new List<ExpressionHook>());
            }
        }
    }
}