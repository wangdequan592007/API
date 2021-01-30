namespace EF_ORACLE
{
    //public class MESContextFactory : IDesignTimeDbContextFactory<MESContext>
    //{
    //    //ApplicationDbContext 代表的是你的创建失败的那个类型
    //    public MESContext CreateDbContext(string[] args)
    //    {
    //        IConfigurationRoot configuration = new ConfigurationBuilder()
    //        .SetBasePath(Directory.GetCurrentDirectory())
    //        .AddJsonFile("appsettings.json")
    //        .Build();
    //        var builder = new DbContextOptionsBuilder();
    //        var UrlOracle = configuration.GetConnectionString("OracleDba");
    //        //var Ora = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=172.17.5.235)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=dmsnew;Password=chpass";
    //        //builder.UseOracle(Ora);
    //        //return new AppContext(builder.Options);
    //        var optionsBuilder = new DbContextOptionsBuilder<MESContext>();
    //        optionsBuilder.UseOracle(Md5Encrypt.Decrypt(UrlOracle));
    //        return new MESContext(optionsBuilder.Options);
    //    }
    //}
    //无效--
    //public class MESContextFactory : IDbContextFactory<MESContext>
    //{
    //    public MESContext Create()
    //    {
    //        var UrlOracle = ("F6D943EFE5877845E6B3B7E3C1EA5893CF7F1EAD6C53DB587D97E7A8DC19C99951F24AC68130377EC5CB34F697A34D42C68F23EFC2F4D525D0D92481A0862549D4C77EC7D86153D68534FE584FAA555FD12CE76DA4699F7649FEDB1D08E33718BCCEA4D02AC45CE92EDF78D0F0DE578674B2FC728D7A2541E3B9FD8FD3470A6EAFDAE149583986AD0CBA82314A96BCE87652906AF4D3F575F459C9D9FA2C9468B6D64012B08E9360");
    //        return new MESContext(Decrypt(UrlOracle));
    //    }
    //    public static string Decrypt(string Text, string sKey = "enok")
    //    {
    //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
    //        int len;
    //        len = Text.Length / 2;
    //        byte[] inputByteArray = new byte[len];
    //        int x, i;
    //        for (x = 0; x < len; x++)
    //        {
    //            i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
    //            inputByteArray[x] = (byte)i;
    //        }
    //        des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
    //        des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
    //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
    //        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
    //        cs.Write(inputByteArray, 0, inputByteArray.Length);
    //        cs.FlushFinalBlock();
    //        return Encoding.Default.GetString(ms.ToArray());
    //    }

    //    private static string Md5Hash(string input)
    //    {
    //        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
    //        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
    //        StringBuilder sBuilder = new StringBuilder();
    //        for (int i = 0; i < data.Length; i++)
    //        {
    //            sBuilder.Append(data[i].ToString("x2"));
    //        }
    //        return sBuilder.ToString();
    //    }
    //}
}
