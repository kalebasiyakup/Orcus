﻿namespace Orcus.Core.Attribute
{
    public class DetailAttribute : System.Attribute
    {
        private readonly string value;
        public string Value
        {
            get
            {
                return this.value;
            }
        }

        public DetailAttribute(string value)
        {
            this.value = value;
        }
    }
}