using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace POS.View
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            //this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelVersion.Text = "v" + Configuration.VERSION;
            this.labelCopyright.Text = AssemblyCopyright;
            //this.labelCompanyName.Text = AssemblyCompany;
            this.labelCompanyName.Text = Configuration.storeName;
            //this.textBoxDescription.Text = AssemblyDescription;
            StringBuilder descriptionTextBuilder = new StringBuilder();
            descriptionTextBuilder.Append(AssemblyDescription);
            descriptionTextBuilder.AppendLine("\r\n");
            descriptionTextBuilder.AppendLine("Icons by Icons8:");
            descriptionTextBuilder.AppendLine("https://icons8.com");
            descriptionTextBuilder.AppendLine("\n");
            descriptionTextBuilder.AppendLine("CircularProgressBar by Soroush Falahati:");
            descriptionTextBuilder.AppendLine("https://github.com/falahati/CircularProgress");
            descriptionTextBuilder.AppendLine("\n");
            descriptionTextBuilder.AppendLine("EPPlus by Jan Källman: ");
            descriptionTextBuilder.AppendLine("https://epplussoftware.com/");
            descriptionTextBuilder.AppendLine("\n");
            descriptionTextBuilder.AppendLine("FakeItEasy by Patrik Hägne, FakeItEasy contributors");
            descriptionTextBuilder.AppendLine("https://fakeiteasy.github.io/");
            descriptionTextBuilder.AppendLine("\n");
            descriptionTextBuilder.AppendLine("NLog by Jarek Kowalski,Kim Christensen,Julian Verdurmen:");
            descriptionTextBuilder.AppendLine("https://nlog-project.org/");
            descriptionTextBuilder.AppendLine("\n");
            descriptionTextBuilder.AppendLine("NUnit by Charlie Poole, Rob Prouse");
            descriptionTextBuilder.AppendLine("https://nunit.org/");
            this.textBoxDescription.Text = descriptionTextBuilder.ToString();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
