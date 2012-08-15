using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BrickPile.UI.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    public class InheritanceAwareModelBinderProvider : Dictionary<Type, IModelBinder>, IModelBinderProvider {
        /// <summary>
        /// Returns the model binder for the specified type.
        /// </summary>
        /// <param name="modelType">The type of the model.</param>
        /// <returns>
        /// The model binder for the specified type.
        /// </returns>
        public IModelBinder GetBinder(Type modelType) {
            var binders = from binder in this
                          where binder.Key.IsAssignableFrom(modelType)
                          select binder.Value;

            return binders.FirstOrDefault();
        }
    }
}