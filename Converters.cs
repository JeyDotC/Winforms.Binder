using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms.Binder
{
    public static class Converters
    {
        public static Converter<string, T> FromStringConverter<T>() =>
            input => (T)Convert.ChangeType(input ?? string.Empty, typeof(T));

        public static Converter<T, string> ToStringConverter<T>() =>
            input => input?.ToString();

        public static Converter<T, T> BypassConverter<T>() =>
            input => input;
    }
}
