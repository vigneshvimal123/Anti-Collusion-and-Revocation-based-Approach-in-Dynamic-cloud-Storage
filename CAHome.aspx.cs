using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;

public partial class CAHome : System.Web.UI.Page
{


    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|Datadirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;
    Random r = new Random();
    int i, j, k;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        bind();
    }

    public static string pass(int length)
    {
        const string chars = "abcdefghijklmnopqstuuvwxyz";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    string pss;
    protected void Button1_Click(object sender, EventArgs e)
    {
        i = r.Next(1111, 6666);
        j = r.Next(1111, 8888);
        k = r.Next(1111, 9999);
        pss=pass (6).ToString ();
        cmd = new SqlCommand("update gmreg set GMID='" + i.ToString() + "',Password='" + pss + "', status='Approved',key1='" + j.ToString() + "',key2='" + k.ToString() + "' where id='" + GridView1.SelectedRow.Cells[1].Text + "'", con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Write("<Script> alert('GM Approved Successfully!') </Script>");


        string to = GridView1.SelectedRow.Cells[5].Text;
        string from = "sampletest685@gmail.com";
        // string subject = "Key";
        // string body = TextBox1.Text;
        string password = "hneucvnontsuwgpj";
        using (MailMessage mm = new MailMessage(from, to))
        {
            mm.Subject = "GMID";
            mm.Body = "GMID:" + i.ToString() + " & " + "Password:  " + pss.ToString();
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
        bind();
    }
    

    private void bind()
    {
        cmd = new SqlCommand("select id,FirstName,LastName,MobileNo,Email from gmreg where Status='Waiting'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt=new  DataTable ();
        da.Fill (dt);
        GridView1 .DataSource =dt;
        GridView1 .DataBind ();

         cmd = new SqlCommand("select id,FirstName,LastName,MobileNo,Email from gmreg where Status !='Waiting'", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataTable dt1=new  DataTable ();
        da1.Fill (dt1);
        GridView2 .DataSource =dt1;
        GridView2 .DataBind ();

    }
}