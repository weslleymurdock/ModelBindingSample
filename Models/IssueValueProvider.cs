using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace ModelBindingSample.Models
{
    public class IssueValueProvider : CompositeValueProvider
    {
        public CultureInfo Culture { get; }
        private readonly Dictionary<string,string> _values;
        private PrefixContainer? _prefixContainer;

        public IssueValueProvider(BindingSource bindingSource, CultureInfo culture) : base()
        {
            _ = bindingSource ?? throw new ArgumentNullException(nameof(bindingSource));

             Culture =  culture;

        }

        protected PrefixContainer PrefixContainer =>
       _prefixContainer ??= new PrefixContainer(_values.Keys);

        public override bool ContainsPrefix(string prefix) =>
            PrefixContainer.ContainsPrefix(prefix);

        public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix)
        {
            _ = prefix ?? throw new ArgumentNullException(nameof(prefix));

            return PrefixContainer.GetKeysFromPrefix(prefix);
        }

        public override ValueProviderResult GetValue(string key)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));

            if (key.Length == 0)
            {
                return ValueProviderResult.None;
            }

            var value = _values[key];
            if (string.IsNullOrEmpty(value))
            {
                return ValueProviderResult.None;
            }

            return new ValueProviderResult(value, Culture);
        }
    }
}
