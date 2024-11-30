using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO;
using System.Net;

public partial class Cserverfile : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|Datadirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;
    SqlDataReader dr;

    string mail;

    protected void Page_Load(object sender, EventArgs e)
    {
        cmd = new SqlCommand("select id,FirstName,LastName,MobileNo,Email from gmreg where Status !='Waiting'", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataTable dt1 = new DataTable();
        da1.Fill(dt1);
        GridView1.DataSource = dt1;
        GridView1.DataBind();
    }
    string id, key;
    protected void Button1_Click(object sender, EventArgs e)
    {

        Random r = new Random();
        int i = r.Next(1111, 9999);

        con.Open();
        cmd = new SqlCommand("select * from gmreg where id='" + GridView1.SelectedRow.Cells[1].Text + "'", con);
         dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            id = dr["GMID"].ToString();
            key = dr["Key1"].ToString();
        }
        con.Close();

        cmd = new SqlCommand("update  userregtb set GMId='" + id + "' , Key1='" + i.ToString() + "' where FirstName='" + Session["uname"].ToString() + "' ", con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        Label2.Text = "Your Public Key" + i.ToString();

        con.Open();
        cmd = new SqlCommand("select * from userregtb where FirstName='" + Session["uname"].ToString() + "' ", con);
         dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            mail = dr["Email"].ToString();
        }
        con.Close();



        string to = mail;
        string from = "sampletest685@gmail.com";
        // string subject = "Key";
        // string body = TextBox1.Text;
        string password = "hneucvnontsuwgpj";
        using (MailMessage mm = new MailMessage(from, to))
        {
            mm.Subject = "GMID";
            mm.Body = "CHANGE PUBLIC KEY:" + i.ToString();
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

      //  Response.Write("<Script> alert('Mail Send All User') </Script>");





        con.Open();
        cmd = new SqlCommand("select * from userregtb where GmId='" + Session["gmid"].ToString() + "'", con);
        dr = cmd.ExecuteReader();
        while (dr.Read ())
        {

            DropDownList1.Items.Add(dr["Email"].ToString());
        }

        con.Close();

        for (int ii = 0; ii < DropDownList1.Items.Count; ii++)
        {

            string to1 = DropDownList1.Items[ii].Text;
            string from1 = "fantest.mail@gmail.com";
            // string subject = "Key";
            // string body = TextBox1.Text;
            string password1 = "fantasy5535";
            using (MailMessage mm = new MailMessage(from1, to1))
            {
                mm.Subject = "GMID";
                mm.Body = Session["Uname"].ToString() + "CHANGE PUBLIC KEY:";
                //if (fuAttachment.HasFile)
                //{
                //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                //}
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(from1, password1);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);

            }



        }


        

    }
}