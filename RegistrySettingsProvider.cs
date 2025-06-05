using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchMKV
{
    class RegistrySettingsProvider : SettingsProvider
    {
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(ApplicationName, config);
        }
        public override string ApplicationName
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name;
            }

            set
            {
                // Do nothing
            }
        }

        private string UserConfigPath
        {
            get
            {
                return "HKEY_CURRENT_USER\\Software\\Adeptweb\\BatchMKV";
            }

        }
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            //collection that will be returned.
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            //iterate through the properties we get from the designer, checking to see if the setting is in the dictionary
            object val;
            foreach (SettingsProperty setting in collection)
            {
                SettingsPropertyValue value = new SettingsPropertyValue(setting);
                value.IsDirty = false;
                var t = Type.GetType(setting.PropertyType.FullName);

                val = Registry.GetValue(UserConfigPath, setting.Name, setting.DefaultValue);
                if (val == null) { val = setting.DefaultValue; }
                value.SerializedValue = val;

                try
                {
                    if (t.ToString() == "System.DateTime")
                    {
                        switch (val.GetType().ToString())
                        {
                            case "System.DateTime":
                                value.PropertyValue = val;
                                break;
                            case "System.String":
                                value.PropertyValue = DateTime.Parse(val.ToString());
                                break;
                            default:
                                value.PropertyValue = new DateTime((long)val);
                                break;
                        }
                    }
                    else
                        value.PropertyValue = Convert.ChangeType(value.SerializedValue, t);
                }
                catch
                {
                    try
                    { value.PropertyValue = Convert.ChangeType(setting.DefaultValue, t); }
                    catch
                    { value.PropertyValue = null; }
                }

                values.Add(value);
            }
            return values;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            //grab the values from the collection parameter and update the values in our dictionary.
            RegistryValueKind valType;
            object val;
            object serializeAs;

            foreach (SettingsPropertyValue value in collection)
            {
                val = value.PropertyValue;
                serializeAs = val;

                var t = val.GetType();
                switch(t.ToString())
                {
                    case "System.String":
                        valType = RegistryValueKind.String;
                        break;
                    case "System.DateTime":
                        valType = RegistryValueKind.QWord;
                        serializeAs = ((DateTime)val).Ticks;
                        break;
                    default:
                        valType = RegistryValueKind.DWord;
                        break;
                }

                Registry.SetValue(UserConfigPath, value.Name, serializeAs, valType);
            }
        }
    }
}
