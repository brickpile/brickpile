using System;

namespace Stormbreaker {
    public class ControlsAttribute : Attribute {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        public Type DocumentType { get; set; }
        public string ControllerName
        {
            get
            {
                var name = DocumentType.Name;
                var i = name.IndexOf("Controller");
                return i > 0 ? name.Substring(0, i) : name;
            }
        }
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        public ControlsAttribute(Type documentType)
        {
            DocumentType = documentType;
        }
    }
}
