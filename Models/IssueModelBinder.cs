using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelBindingSample.Models
{
    public class IssueModelBinder : IModelBinder
    {
        public IssueModelBinder() { }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            var properties = typeof(Issue).GetProperties();
            var subProps = typeof(User).GetProperties().ToList();

            foreach (var property in properties)
            {
                var type = property.PropertyType;

                var key = property.Name;

                if (type == typeof(User))
                    continue;

                var valueProviderResult = bindingContext.ValueProvider.GetValue(key);

                if (valueProviderResult == ValueProviderResult.None)
                {
                    bindingContext.ModelState.TryAddModelError(
                        modelName, $"Error on GetValue from property {key} in ValueProvider");
                    return Task.CompletedTask;
                }
                var value = valueProviderResult.FirstValue;

                bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

                if (string.IsNullOrEmpty(value))
                {
                    bindingContext.ModelState.TryAddModelError(
                        modelName, $"{key} is null or empty");

                    return Task.CompletedTask;
                }

                var customAttrs = property.GetCustomAttributes();
                var isObjectId = false;

                foreach (var attribute in customAttrs)
                {
                    isObjectId = attribute.GetType() == typeof(BsonIdAttribute);
                    if (isObjectId) break;
                }

                if (isObjectId)
                {
                    if (!ObjectId.TryParse(value, out _))
                    {
                        // Non-objectid arguments result in model state errors
                        bindingContext.ModelState.TryAddModelError(
                            modelName, $"{key} must be an object id.");

                        return Task.CompletedTask;
                    }
                }

                if (type.IsEquivalentTo(typeof(DateTime)))
                {
                    if (!DateTime.TryParse(value, out _))
                    {
                        // Non-datetime arguments result in model state errors
                        bindingContext.ModelState.TryAddModelError(
                            modelName, $"{property.Name} must be an DateTime.");

                        return Task.CompletedTask;
                    }
                }
            }
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model as Issue);
            return Task.CompletedTask;
        }
    }
}
