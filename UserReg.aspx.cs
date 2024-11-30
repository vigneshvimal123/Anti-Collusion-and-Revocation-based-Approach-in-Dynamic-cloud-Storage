using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;


public partial class UserReg : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|Datadirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;
    string id, key;

    protected void Page_Load(object sender, EventArgs e)
    {
        cmd = new SqlCommand("select id,FirstName,LastName,MobileNo,Email from gmreg where Status !='Waiting'", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataTable dt1 = new DataTable();
        da1.Fill(dt1);
        GridView1.DataSource = dt1;
        GridView1.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

      

        con.Open();
        cmd = new SqlCommand("select * from gmreg where id='" + GridView1.SelectedRow.Cells[1].Text + "'", con);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            id = dr["GMID"].ToString();
            key = dr["Key1"].ToString();
        }
        con.Close();


        cmd = new SqlCommand("insert into userregtb values(@FirstName,@LastName,@Mobileno,@Email,@Address,@GMid,@Key1,@Password,@Status)", con);
        cmd.Parameters.AddWithValue("@FirstName", TextBox1.Text);
        cmd.Parameters.AddWithValue("@LastName", TextBox2.Text);
        cmd.Parameters.AddWithValue("@Mobileno", TextBox3.Text);
        cmd.Parameters.AddWithValue("@Email", TextBox4.Text);
        cmd.Parameters.AddWithValue("@Address", TextBox5.Text);
        cmd.Parameters.AddWithValue("@GMid", id.ToString());
        cmd.Parameters.AddWithValue("@Key1", key);
        cmd.Parameters.AddWithValue("@password", TextBox7.Text);
        cmd.Parameters.AddWithValue("@Status", "Waiting");
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Write("<Script>  alert('New Gm Info Saved') </Script>");




        string to = TextBox4.Text;
        string from = "sampletest685@gmail.com";
        // string subject = "Key";
        // string body = TextBox1.Text;
        string password = "hneucvnontsuwgpj";
        using (MailMessage mm = new MailMessage(from, to))
        {
            mm.Subject = "GMID";
            mm.Body = "PUBLIC KEY:" + key;
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