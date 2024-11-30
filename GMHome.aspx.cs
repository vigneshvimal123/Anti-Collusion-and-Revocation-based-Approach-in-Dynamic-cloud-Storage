using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class GMHome : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|Datadirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;

    protected void Page_Load(object sender, EventArgs e)
    {
        bind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        if (GridView1.SelectedRow.Cells[1].Text == "")
        {
        }
        else
        {
            cmd = new SqlCommand("update userregtb set Status='Approved' where id='" + GridView1.SelectedRow.Cells[1].Text + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Write("<Script> alert('User Approved Successfully!') </Script>");
            bind();
        }
      

    }


    private void bind()
    {
        cmd = new SqlCommand("select id,FirstName,LastName,MobileNo,Email from userregtb where Status='Waiting' and GMID='"+Session ["gmid"]+"'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();

        cmd = new SqlCommand("select id,FirstName,LastName,MobileNo,Email from userregtb where Status !='Waiting' and GMID='" + Session["gmid"] + "' ", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataTable dt1 = new DataTable();
        da1.Fill(dt1);
        GridView2.DataSource = dt1;
        GridView2.DataBind();

    }
}