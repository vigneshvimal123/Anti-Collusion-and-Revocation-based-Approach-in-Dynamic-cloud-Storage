using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserLogin : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|Datadirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        con.Open();
        cmd = new SqlCommand("select * from userregtb where FirstName='" + TextBox1.Text + "' and Password='" + TextBox2.Text + "' and   key1='" + TextBox3.Text + "'  ", con);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            Session["uname"] = TextBox1.Text;
            Session["gmid"] = dr["GMID"].ToString();
            Response.Redirect("CUserHome.aspx");
        }
        else
        {
            Response.Write("<Script> alert('Password Mismatch') </Script>");
        }
        con.Close();

    }
}