using System;
using System.IO;

namespace UX_Developer
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
	public partial class application
	{
		private string guidField;
		private string nameField;
		private string versionField;
		private string descriptionField;
		private string publisherField;
		private string copyrightField;
		private string requiredmfilesversionField;
		private applicationModules modulesField;
		private applicationDashboard[] dashboardsField;

		/// 
		public string guid
		{
			get
			{
				return this.guidField;
			}
			set
			{
				this.guidField = value;
			}
		}
		/// 
		public string name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}
		/// 
		public string version
		{
			get
			{
				return this.versionField;
			}
			set
			{
				this.versionField = value;
			}
		}
		/// 
		public string description
		{
			get
			{
				return this.descriptionField;
			}
			set
			{
				this.descriptionField = value;
			}
		}
		/// 
		public string publisher
		{
			get
			{
				return this.publisherField;
			}
			set
			{
				this.publisherField = value;
			}
		}
		/// 
		public string copyright
		{
			get
			{
				return this.copyrightField;
			}
			set
			{
				this.copyrightField = value;
			}
		}
		/// 
		[System.Xml.Serialization.XmlElementAttribute("required-mfiles-version")]
		public string requiredmfilesversion
		{
			get
			{
				return this.requiredmfilesversionField;
			}
			set
			{
				this.requiredmfilesversionField = value;
			}
		}
		/// 
		public applicationModules modules
		{
			get
			{
				return this.modulesField;
			}
			set
			{
				this.modulesField = value;
			}
		}
		/// 
		[System.Xml.Serialization.XmlArrayItemAttribute("dashboard", IsNullable = false)]
		public applicationDashboard[] dashboards
		{
			get
			{
				return this.dashboardsField;
			}
			set
			{
				this.dashboardsField = value;
			}
		}
	}
	/// 
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class applicationModules
	{
		private applicationModulesModule moduleField;
		/// 
		public applicationModulesModule module
		{
			get
			{
				return this.moduleField;
			}
			set
			{
				this.moduleField = value;
			}
		}
	}
	/// 
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class applicationModulesModule
	{
		private string[] fileField;
		private string environmentField;
		/// 
		[System.Xml.Serialization.XmlElementAttribute("file")]
		public string[] file
		{
			get
			{
				return this.fileField;
			}
			set
			{
				this.fileField = value;
			}
		}
		/// 
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string environment
		{
			get
			{
				return this.environmentField;
			}
			set
			{
				this.environmentField = value;
			}
		}
	}
	/// 
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class applicationDashboard
	{
		private string contentField;
		private string idField;
		/// 
		public string content
		{
			get
			{
				return this.contentField;
			}
			set
			{
				this.contentField = value;
			}
		}
		/// 
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
	}
}