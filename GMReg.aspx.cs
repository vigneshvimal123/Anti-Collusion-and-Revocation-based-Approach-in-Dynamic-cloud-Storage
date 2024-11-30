using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class GMReg : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|Datadirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        cmd = new SqlCommand("insert into gmreg values(@FirstName,@LastName,@Mobileno,@Email,@Address,@Company,@Password,@GMid,@Key1,@Key2,@Status)", con);
        cmd.Parameters.AddWithValue("@FirstName", TextBox1.Text);
        cmd.Parameters.AddWithValue("@LastName", TextBox2.Text);
        cmd.Parameters.AddWithValue("@Mobileno", TextBox3.Text);
        cmd.Parameters.AddWithValue("@Email", TextBox4.Text);
        cmd.Parameters.AddWithValue("@Address", TextBox5.Text);
        cmd.Parameters.AddWithValue("@Company", TextBox6.Text);
        cmd.Parameters.AddWithValue("@Password", "");
        cmd.Parameters.AddWithValue("@GMid", "");
        cmd.Parameters.AddWithValue("@Key1", "");
        cmd.Parameters.AddWithValue("@Key2", "");
        cmd.Parameters.AddWithValue("@Status", "Waiting");
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Write("<Script>  alert('New Gm Info Saved') </Script>");
    }
}