using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.IO;


public partial class GMuser : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|Datadirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;

    protected void Page_Load(object sender, EventArgs e)
    {
        bind();
    }
    private void bind()
    {
        cmd = new SqlCommand("select * from ufile where  GMId='" + Session["gmid"].ToString() + "' and Status='Waiting' ", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        GridView2.DataSource = dt;
        GridView2.DataBind();

        //cmd = new SqlCommand("select * from ufile where Status='Approved' and Username='" + Session["uname"].ToString() + "' ", con);
        //SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        //DataTable dt1 = new DataTable();
        //da1.Fill(dt1);
        //GridView2.DataSource = dt1;
        //GridView2.DataBind();

    }
    string key, mail,uname;
    protected void lnkView_Click1(object sender, EventArgs e)
    {
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string id = grdrow.Cells[0].Text;
        string id1 = grdrow.Cells[1].Text;


        con.Open();
        cmd = new SqlCommand("select * from filetb where id='"+id1+"'", con);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            key = dr["Keys"].ToString();
            Label2.Text = "your key" + key;

        }
        con.Close();


        con.Open();
        cmd = new SqlCommand("Update   ufile set   Status='Approved'where id ='" + id + "'", con);
        cmd.ExecuteNonQuery();
        Response.Write("<Script> alert('Approved') </Script>");
        con.Close();
        con.Open();
        cmd = new SqlCommand("select * from ufile where id='" + id + "' ", con);
        SqlDataReader dr1 = cmd.ExecuteReader();
        if (dr1.Read())
        {
            uname = dr1["userName"].ToString();
        }
        con.Close();


        con.Open();
        cmd = new SqlCommand("select * from userregtb where FirstName='" + uname + "' ", con);
        SqlDataReader dr11 = cmd.ExecuteReader();
        if (dr11.Read())
        {

            mail = dr11["Email"].ToString();
        }
        con.Close();



        string to = mail;
        string from = "sampletest685@gmail.com";
        // string subject = "Key";
        // string body = TextBox1.Text;
        string password = "hneucvnontsuwgpj";
        using (MailMessage mm = new MailMessage(from, to))
        {
            mm.Subject = "File KEY";
            mm.Body = "File Id" + id1 + " KEY:" + key;
            //if (fuAttachment.HasFile)
            //{
            //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
            //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
            //}
            mm.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential(from, password);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
            ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);

        }



    }
}