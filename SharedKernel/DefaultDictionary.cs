using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel
{
    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new()
    {
        public TValue defaultValue {  get; set; }
        public new TValue this[TKey key]
        {
            get
            {
                TValue val;
                if (!TryGetValue(key, out val))
                {
                    val = defaultValue;
                    Add(key, val);
                }
                return val;
            }
            set { base[key] = value; }
        }
    }
}
