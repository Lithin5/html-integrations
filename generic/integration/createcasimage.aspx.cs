using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace pluginwiris
{
    /// <summary>
    /// Summary description for createcasimage.
    /// </summary>
    public partial class createcasimage : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
			Hashtable config = Libwiris.loadConfig(this.MapPath(Libwiris.configFile));
		
            if (this.Request.Form["image"] != null && this.Request.Form["image"].Length > 0)
            {
                string fileName = Libwiris.md5(this.Request.Form["image"]);
				string formulaPath = (Libwiris.getFormulaDirectory(config) != null) ? Libwiris.getFormulaDirectory(config) : this.MapPath(Libwiris.FormulaDirectory);
				formulaPath += "/" + fileName + ".ini";

                if (this.Request.Form["mml"] != null && this.Request.Form["mml"].Length > 0 && !File.Exists(formulaPath))
                {
                    TextWriter file = new StreamWriter(formulaPath);
                    file.Write(this.Request.Form["mml"]);
                    file.Close();
                }

                string url = this.Page.ResolveUrl("showcasimage.aspx") + "?formula=" + fileName + ".png";
				string imagePath = (Libwiris.getCacheDirectory(config) != null) ? Libwiris.getCacheDirectory(config) : this.MapPath(Libwiris.CacheDirectory);
				imagePath += "/" + fileName + ".png";

                if (!File.Exists(imagePath))
                {
                    byte[] imageBytes = Convert.FromBase64String(this.Request.Form["image"]);
                    FileStream file = new FileStream(imagePath, FileMode.Create, FileAccess.Write);
                    BinaryWriter writer = new BinaryWriter(file);

                    for (int i = 0; i < imageBytes.Length; ++i)
                    {
                        writer.Write(imageBytes[i]);
                    }

                    writer.Close();
                    file.Close();
                    this.Response.Write(url);
                }
                else
                {
                    this.Response.Write(url);
                }
            }
            else
            {
                this.Response.Write("TODO: aqu� deber�a mostrar el cas.gif");
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}
