using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task_1
{
    public class Customer
    {
        #region default format provider
        public class DefaultProvider : IFormatProvider, ICustomFormatter
        {
            private IFormatProvider parent;

            public DefaultProvider() : this(CultureInfo.CurrentCulture) { }

            public DefaultProvider(IFormatProvider provider)
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
                if (ReferenceEquals(arg, null) || format != "DF")
                    return string.Format(parent, "{0:" + format + "}", arg);

                if (arg is Customer)
                {
                    Customer customer = (Customer)arg;
                    StringBuilder result = new StringBuilder();
                    result.Append($"Name: {customer.Name}, Contact Phone: {customer.ContactPhone}, Revenue: {customer.Revenue}");
                    return result.ToString();
                }

                return string.Empty;
            }

            public override string ToString() => "DF";
        }
        #endregion

        #region private fields
        private string _name;
        private string _contactPhone;
        private decimal _revenue;
        #endregion

        #region public properties
        public string Name => _name;
        public string ContactPhone => _contactPhone;
        public decimal Revenue => _revenue;
        #endregion

        #region public methods
        public Customer(string name, string contactPhone, decimal revenue)
        {
            _name = name;
            _contactPhone = contactPhone;
            _revenue = revenue;
        }

        public override string ToString() => ToString(new DefaultProvider());

        public string ToString(IFormatProvider formatProvider)
        {
            string result = string.Empty;

            if (formatProvider is ICustomFormatter)
            {
                ICustomFormatter formatter = (ICustomFormatter)formatProvider;
                result = formatter.Format(formatProvider.ToString(), this, formatProvider);
            }

            return result;
        }
        #endregion
    }
}
