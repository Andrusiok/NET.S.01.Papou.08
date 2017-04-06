using System;
using System.Text;
using NUnit.Framework;
using System.Globalization;
using Task_1;

namespace Task_1.Tests
{
    public class Provider : IFormatProvider, ICustomFormatter
    {
        private IFormatProvider parent;

        public Provider() : this(CultureInfo.CurrentCulture) { }

        public Provider(IFormatProvider provider)
        {
            parent = provider;
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return null;
        }

        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (ReferenceEquals(arg, null) || format != "P")
                return string.Format(parent, "{0:" + format + "}", arg);

            if (arg is Customer)
            {
                Customer customer = (Customer)arg;
                StringBuilder result = new StringBuilder();
                result.Append($"Name: {customer.Name}");
                return result.ToString();
            }

            return string.Empty;
        }

        public override string ToString() => "P";
    }

    [TestFixture]
    public class Tests
    {
        [TestCase]
        public void Formatter_Positive()
        {
            Customer customer = new Customer("Andrej", "+375 (29) 181-16-41", 15.005m);

            string result = string.Format(new Customer.DefaultProvider(), "{0:DF}", customer);

            result = customer.ToString();
            result = string.Format(new Provider(), "{0:P}", customer);
            result = customer.ToString(new Provider());
        }
    }
}
