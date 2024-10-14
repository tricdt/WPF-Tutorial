﻿using System.ComponentModel;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ChangedFieldInfo
    {
        PropertyDescriptorCollection pdc;
        string name;
        object oldValue;
        object newValue;
        int fieldIndex = -1;
        bool hasValue = false;
        double delta;
        bool hasDelta = false;
        public ChangedFieldInfo(PropertyDescriptorCollection pdc, string name)
        {
            this.name = name;
            this.pdc = pdc;
            this.hasValue = false;
        }

        public ChangedFieldInfo(PropertyDescriptorCollection pdc, string name, object oldValue, object newValue)
        {
            this.name = name;
            this.pdc = pdc;
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.hasValue = true;
        }
        public PropertyDescriptorCollection Properties
        {
            get
            {
                return pdc;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public object OldValue
        {
            get
            {
                return oldValue;
            }
            set
            {
                oldValue = value;
                hasValue = true;
            }
        }

        public object NewValue
        {
            get
            {
                return newValue;
            }
            set
            {
                newValue = value;
                hasValue = true;
            }
        }

        public bool HasValue
        {
            get
            {
                return hasValue;
            }
        }

        public void SetValues(object oldValue, object newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.hasValue = true;
        }

        public double Delta
        {
            get
            {
                if (!hasDelta)
                {
                    object _newValue = newValue;
                    object _oldValue = oldValue;
                    if (newValue == null || newValue is DBNull)
                        _newValue = 0;
                    if (oldValue == null || oldValue is DBNull)
                        _oldValue = 0;
                    delta = Convert.ToDouble(_newValue) - Convert.ToDouble(_oldValue);
                    hasDelta = true;
                }
                return delta;
            }
        }

        public int FieldIndex
        {
            get
            {
                if (fieldIndex == -1)
                    fieldIndex = Properties.IndexOf(Properties[Name]);
                return fieldIndex;
            }
        }
    }
}
