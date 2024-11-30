﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Security.Cryptography;
public partial class GMfileUpload : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|Datadirectory|\Collusiondb.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd;
    Random r = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label5.Text = Session["gmid"].ToString();

    }

   

    protected void Button1_Click(object sender, EventArgs e)
    {
        int i = r.Next(11111, 99999);


        string filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
        FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Upload/" + filename));
        string filePath = Server.MapPath("~/Upload/" + filename);

        decimal size = Math.Round(((decimal)FileUpload1.PostedFile.ContentLength / (decimal)1024), 2);

        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
        br.Close();
        fs.Close();



        cmd = new SqlCommand("insert into filetb values(@GMid,@FileInfo,@FileName,@FilePath,@FileData,@size,@keys)", con);
        cmd.Parameters.AddWithValue("@GMid", Label5.Text);
        cmd.Parameters.AddWithValue("@FileInfo", TextBox1.Text);

        cmd.Parameters.AddWithValue("@FileName", filename);
        cmd.Parameters.AddWithValue("@FilePath", "~/Upload/" + filename);
        cmd.Parameters.AddWithValue("@FileData", bytes);
        cmd.Parameters.AddWithValue("@size", size.ToString() + "KB");
        cmd.Parameters.AddWithValue("@keys", i.ToString());

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        string filePath1 = Server.MapPath("~/Encrypt/" + filename);
        EncryptFile(filePath, filePath1);
        Response.Write("<Script> alert('File Encrypt and Saved') </Script>");



    }

    private void EncryptFile(string inputFile, string outputFile)
    {


        string password = @"myKey123";
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] key = UE.GetBytes(password);

        string cryptFile = outputFile;
        FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

        RijndaelManaged RMCrypto = new RijndaelManaged();

        CryptoStream cs = new CryptoStream(fsCrypt,
            RMCrypto.CreateEncryptor(key, key),
            CryptoStreamMode.Write);

        FileStream fsIn = new FileStream(inputFile, FileMode.Open);

        int data;
        while ((data = fsIn.ReadByte()) != -1)
            cs.WriteByte((byte)data);

        fsIn.Close();
        cs.Close();
        fsCrypt.Close();
        Response.Write("Encryption Sucessfully Completed");

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        TextBox1.Text = "";

    }
}