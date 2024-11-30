using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class CUserHome : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename= |DataDirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;

    protected void Page_Load(object sender, EventArgs e)
    {
        bind();
    }
    string key, mail;
    string s1, s2, s3, s4, s5;
    protected void lnkView_Click(object sender, EventArgs e)
    {
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string id = grdrow.Cells[0].Text;

        con.Open();
        cmd = new SqlCommand("select * from filetb where id ='" + id + "'", con);
        SqlDataReader dr;
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            s1 = dr["FileName"].ToString();
            s2 = dr["fileInfo"].ToString();
            s3 = dr["Gmid"].ToString();
           


        }
        con.Close();

        cmd = new SqlCommand("insert into ufile values('" + id + "','" + s1 + "','" + s2 + "','" + s3 + "','" + Session["Uname"].ToString() + "','Waiting')", con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        Response.Write("<Script> alert('Request Send') </Script>");

    }

    private void bind()
    {
        cmd = new SqlCommand("select * from filetb where  GMId='" + Session["gmid"].ToString() + "'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();

        cmd = new SqlCommand("select * from ufile where Status='Approved' and Username='" + Session["uname"].ToString() + "' ", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataTable dt1 = new DataTable();
        da1.Fill(dt1);
        GridView2.DataSource = dt1;
        GridView2.DataBind();

    }
    protected void lnkView_Click1(object sender, EventArgs e)
    {
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string id = grdrow.Cells[0].Text;
        string id1 = grdrow.Cells[1].Text;
        con.Open();
        cmd = new SqlCommand("select * from filetb where id ='" + id1 + "' and Keys='" + TextBox1.Text + "'", con);
        SqlDataReader dr;
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            string aaa = dr["FilePath"].ToString();

            if (aaa != string.Empty)
            {
                string filePath = aaa;
                Response.ContentType = "doc/docx";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + aaa + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }


        }
        else
        {
            Response.Write("<Script> alert('Key Incorrect') </Script>");
        }
        con.Close();

      

    }
}