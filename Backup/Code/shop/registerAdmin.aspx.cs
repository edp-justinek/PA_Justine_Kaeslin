using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class shop_registerAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string error = string.Empty;

        // set first redirect url
        string redirectUrl = "";
        try
        {
            redirectUrl = Request.UrlReferrer.AbsoluteUri.Split('&')[0];
        }
        catch
        {

        }

        if (Session["loggedIn"] != null)
        {
            if ((bool)Session["loggedIn"])
            {
                Response.Redirect(Request["registerOK_hidden"]);
            }
        }

        // prepare message with default fields
        string rqMessage = "fisVLRegistrier" + (char)4 + "Sprache" + (char)5 + App_Lib.GetVenezialangCode() + (char)6 + "regtyp" + (char)5 + "full" + (char)6 + "firma" + (char)5 + ConfigurationManager.AppSettings["FirmaID"].ToString() +(char)6;

        // save form
        App_Lib.SetViewState();

        try
        {
            foreach (string myVeneziaField in Request.Form.Keys)
            {
                // for each field in form (expect hidden fields)
                if (!myVeneziaField.Contains("_hidden") && !myVeneziaField.ToLower().Contains("agb"))
                {
                    rqMessage += myVeneziaField + (char)5 + Request[myVeneziaField] + (char)6;
                    if (myVeneziaField == "EmailAdr")
                    {
                        // double for user register
                        rqMessage += "email" + (char)5 + Request[myVeneziaField] + (char)6;
                    }
                }
            }
           
            if (fehlercode == "00")
            {  
				if (!String.IsNullOrEmpty(refurl))
				{
					Response.Redirect(refurl);
				}
				else
				{
					Response.Redirect(Request["registerOK_hidden"]);
				}

                }
                else
                {
                    error = App_Lib.Translate("Benutzer erfolgreich registiert") + ", " + App_Lib.Translate("aber Login ungültig") + ". " + App_Lib.Translate("Bitte Support kontaktieren") + ".";
                }               
            }
            else
            {
                switch (fehlercode)
                {
                    default:
                        error = App_Lib.Translate("Unbekannter Fehler beim Erfassen der Adresse");

                        break;

                        case "12":
                        
                         error = App_Lib.Translate("PLZ oder Ort ist nicht korrekt") + ". " + App_Lib.Translate("Bitte überprüfen Sie die Eingaben nochmals") + ".";

                        break;

                    case "16":
                        error = App_Lib.Translate("E-Mail Adresse / Benutzer bereits vorhanden") + ". " + App_Lib.Translate("Wählen Sie eine andere E-Mail adresse oder melden Sie sich an") + ".";

                        break;
                }

                Response.Redirect(Request.UrlReferrer.AbsoluteUri + "&error=" + error);
            }
            if (!string.IsNullOrEmpty(error))
            {
                Response.Redirect(Request.UrlReferrer.AbsoluteUri + "&error=" + error);
            }
        }
        catch (Exception ex)
        {
        }

    }
}