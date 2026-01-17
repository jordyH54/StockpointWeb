namespace stockpoint.Models
{
    using System;
    using System.IO;
    using System.Net.Mime;
    using System.Net.NetworkInformation;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using static stockpoint.Models.cOcadis;

    public class cOcadisINI
    {
        public class Val
        {
            #region Variables
            #endregion
            public DateTime Hoy { get; set; } = DateTime.Now;
            public int Entero(string val = "")
            {
                if (val.Equals(""))
                    return 0;
                try
                {
                    return int.Parse(val);
                }
                catch (System.Exception)
                {
                    return 0;
                }
            }
            public decimal Decimal(string val = "")
            {
                if (val.Equals(""))
                    return 0;
                try
                {
                    val = val.Replace("$", "");
                    val = val.Replace(",", "");
                    val = val.Replace(" ", "");
                    return decimal.Parse(val);
                }
                catch (System.Exception)
                {
                    return 0;
                }
            }
            public double Doble(string val = "")
            {
                if (val.Equals(""))
                    return 0;
                try
                {
                    val = val.Replace("$", "");
                    val = val.Replace(",", "");
                    val = val.Replace(" ", "");
                    return double.Parse(val);
                }
                catch (System.Exception)
                {
                    return 0;
                }
            }
            public DateTime Fecha(string val = "")
            {
                if (val.Equals(""))
                    return DateTime.MinValue;
                try
                {
                    return DateTime.Parse(val);
                }
                catch (System.Exception)
                {
                    return DateTime.MinValue;
                }
            }
            public void EscribirQR(string FileName, string txt)
            {
                string[] arrCaperta = FileName.Split('/');
                string A = arrCaperta[arrCaperta.Length - 1];
                A = FileName.Replace(A, "");

                A = HttpContext.Current.Request.MapPath(A);
                if (!System.IO.Directory.Exists(A))
                    System.IO.Directory.CreateDirectory(A);
                if (System.IO.File.Exists(FileName))
                {
                    System.IO.File.Delete(FileName);
                }
                FileName = HttpContext.Current.Request.MapPath(FileName);

                ZXing.BarcodeWriter br = new ZXing.BarcodeWriter();
                br.Format = ZXing.BarcodeFormat.QR_CODE;
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(br.Write(txt), 200, 200);
                bm.Save(FileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            public void EscribirDesdeMemoria(MemoryStream stream, string FileName, string Folder)
            {
                stream.Position = 0;
                string path = Path.Combine(HttpContext.Current.Server.MapPath(Folder), FileName);
                File.WriteAllBytes(path, stream.ToArray());

            }
            public string LeerArchivo(String sArchivo)
            {
                string Archivo= System.Web.HttpContext.Current.Server.MapPath(sArchivo);
                String texto = "";
                String strLine = "";
                try
                {
                    System.IO.FileStream aFile = new FileStream(Archivo, FileMode.Open);
                    StreamReader sr = new StreamReader(aFile);
                    while (strLine != null)
                    {
                        if (strLine != "\n")
                        {
                            texto += strLine;
                            strLine = sr.ReadLine();
                            texto += "\n";
                        }
                    }
                    sr.Close();
                    aFile.Close();
                }
                catch (IOException e)
                {
                    Console.WriteLine("An IO exception has been thrown!");
                    Console.WriteLine(e.ToString());
                }
                return texto;
            }
            public string FechaTexto(DateTime fecha)
            {
                if (fecha == DateTime.MinValue)
                    return "";
                string str_date = fecha.Day + " de ";
                switch (fecha.Month)
                {
                    case 1:; str_date += "Enero"; break;
                    case 2:; str_date += "Febrero"; break;
                    case 3:; str_date += "Marzo"; break;
                    case 4:; str_date += "Abril"; break;
                    case 5:; str_date += "Mayo"; break;
                    case 6:; str_date += "Junio"; break;
                    case 7:; str_date += "Julio"; break;
                    case 8:; str_date += "Agosto"; break;
                    case 9:; str_date += "Septiembre"; break;
                    case 10:; str_date += "Octubre"; break;
                    case 11:; str_date += "Noviembre"; break;
                    case 12:; str_date += "Diciembre"; break;
                }

                str_date += " de " + fecha.Year;
                return str_date;
            }
            public string FormatoPesos(decimal d = 0)
            {
                try
                {
                    return d.ToString("c2");
                }
                catch (Exception)
                {

                    return "$0.00";
                }
            }
            public string FormatoPesos(string d = "0")
            {
                try
                {
                    decimal valor = Decimal(d);
                    return valor.ToString("c2");
                }
                catch (Exception)
                {

                    return "$0.00";
                }
            }

        }
        #region oCode


        private class cEncriptar
        {
            public string KEY { get; set; }
            public string Dominio { get; set; }
            public string Usuario { get; set; }
            public string Mac { get; set; }
            public string Server { get; set; }
            public string UserDB { get; set; }
            public string PswDB { get; set; }
            public string DB { get; set; }
            public string CN { get; set; }
            public static byte[] EncriptarR(string strEncriptar, byte[] bytPK)
            {
                Rijndael miRijndael = Rijndael.Create();
                byte[] encrypted = null;
                byte[] returnValue = null;

                try
                {
                    miRijndael.Key = bytPK;
                    miRijndael.GenerateIV();

                    byte[] toEncrypt = System.Text.Encoding.UTF8.GetBytes(strEncriptar);
                    encrypted = (miRijndael.CreateEncryptor()).TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);

                    returnValue = new byte[miRijndael.IV.Length + encrypted.Length];
                    miRijndael.IV.CopyTo(returnValue, 0);
                    encrypted.CopyTo(returnValue, miRijndael.IV.Length);
                }
                catch { }
                finally { miRijndael.Clear(); }

                return returnValue;
            }
            public cEncriptar()
            {
                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                string[] arrUser = GetUser().Split('\\');

                Dominio = computerProperties.DomainName;
                Usuario = arrUser[1];
                Mac = GetMac();
                KEY = "www.ocadis";

            }
            public static string Desencriptar(byte[] bytDesEncriptar, byte[] bytPK)
            {
                Rijndael miRijndael = Rijndael.Create();
                byte[] tempArray = new byte[miRijndael.IV.Length];
                byte[] encrypted = new byte[bytDesEncriptar.Length - miRijndael.IV.Length];
                string returnValue = string.Empty;

                try
                {
                    miRijndael.Key = bytPK;

                    Array.Copy(bytDesEncriptar, tempArray, tempArray.Length);
                    Array.Copy(bytDesEncriptar, tempArray.Length, encrypted, 0, encrypted.Length);
                    miRijndael.IV = tempArray;

                    returnValue = System.Text.Encoding.UTF8.GetString((miRijndael.CreateDecryptor()).TransformFinalBlock(encrypted, 0, encrypted.Length));
                }
                catch { }
                finally { miRijndael.Clear(); }

                return returnValue;
            }
            private string GetMac()
            {

                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                string Micadena = "";
                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties(); //  .GetIPInterfaceProperties();


                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        // Display the physical address in hexadecimal.
                        Micadena = Micadena + bytes[i].ToString("X2");

                        // Insert a hyphen after each byte, unless we are at the end of the  
                        // address. 
                        if (i != bytes.Length - 1)
                        {
                            Micadena = Micadena + "-";
                        }
                    }
                    return Micadena;


                }
                return Micadena;
            }
            private static string GetUser()//Usuario + dominio
            {
                string UsName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;//Dominio + Login
                                                                                            //string UsName = Environment.UserName;//Sólo login
                return UsName;
            }
            public string Encriptar(string strEncriptar, string strPK)
            {
                byte[] bytes = EncriptarR(strEncriptar, (new PasswordDeriveBytes(strPK, null)).GetBytes(32));

                StringBuilder TextoEncriptado = new StringBuilder();
                foreach (byte element in bytes)
                {
                    TextoEncriptado.Append(element.ToString().PadLeft(3, '0'));
                    //Console.WriteLine("{0} = {1}", element, (char)element);
                }

                return TextoEncriptado.ToString().Substring(0, (TextoEncriptado.ToString().Length));

            }
            public string Encriptar(string strEncriptar)
            {
                if (strEncriptar == null || strEncriptar.Equals(""))
                    return "";
                byte[] bytes = EncriptarR(strEncriptar, (new PasswordDeriveBytes(KEY, null)).GetBytes(32));

                StringBuilder TextoEncriptado = new StringBuilder();
                foreach (byte element in bytes)
                {
                    TextoEncriptado.Append(element.ToString().PadLeft(3, '0'));
                    //Console.WriteLine("{0} = {1}", element, (char)element);
                }

                return TextoEncriptado.ToString().Substring(0, (TextoEncriptado.ToString().Length));

            }
            public string Desencriptar(string strDesEncriptar, string strPK)
            {

                byte[] bytDesEncriptar = null;

                //string[] split = strDesEncriptar.Split(new Char[] { '-' });

                byte Valor;
                int I = 0, I3 = 0, IRow = 0;
                //foreach (string s in split)
                //{
                //    I = I + 1;
                //}
                I = strDesEncriptar.Length / 3;
                bytDesEncriptar = new byte[I];


                for (I = 0; I < strDesEncriptar.Length; I++)
                {
                    I3 = I3 + 1;
                    if (I3 == 3)
                    {
                        Valor = Convert.ToByte(strDesEncriptar.Substring(I - 2, 3));
                        bytDesEncriptar[IRow] = Valor;
                        I3 = 0;
                        IRow = IRow + 1;
                    }
                }


                return Desencriptar(bytDesEncriptar, (new PasswordDeriveBytes(strPK, null)).GetBytes(32));

                #region Metodo Split
                //byte[] bytDesEncriptar = null;

                //string[] split = strDesEncriptar.Split(new Char[] { '-' });

                //byte Valor;
                //int I = 0;
                //foreach (string s in split)
                //{
                //    I = I + 1;
                //}
                //bytDesEncriptar = new byte[I];
                //I = 0;

                //foreach (string s in split)
                //{
                //    Valor = Convert.ToByte(s);
                //    bytDesEncriptar[I] = Valor;
                //    I = I + 1;
                //}

                //return Desencriptar(bytDesEncriptar, (new PasswordDeriveBytes(strPK, null)).GetBytes(32));
                #endregion
            }
            public string Desencriptar(string strDesEncriptar)
            {
                try
                {
                    if (strDesEncriptar == null)
                        return "";
                    if (strDesEncriptar == "")
                        return "";
                    byte[] bytDesEncriptar = null;

                    //string[] split = strDesEncriptar.Split(new Char[] { '-' });

                    byte Valor;
                    int I = 0, I3 = 0, IRow = 0;
                    //foreach (string s in split)
                    //{
                    //    I = I + 1;
                    //}
                    I = strDesEncriptar.Length / 3;
                    bytDesEncriptar = new byte[I];


                    for (I = 0; I < strDesEncriptar.Length; I++)
                    {
                        I3 = I3 + 1;
                        if (I3 == 3)
                        {
                            Valor = Convert.ToByte(strDesEncriptar.Substring(I - 2, 3));
                            bytDesEncriptar[IRow] = Valor;
                            I3 = 0;
                            IRow = IRow + 1;
                        }
                    }


                    return Desencriptar(bytDesEncriptar, (new PasswordDeriveBytes(KEY, null)).GetBytes(32));
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            public bool IsNumeric(String sValue)
            {
                Double d;
                try
                {
                    d = Convert.ToDouble(sValue);
                    return (true);
                }
                catch
                {
                    return (false);
                }
            }
            public string GenerarPsd()
            {
                string sClave = "";
                ToExcelIndexCol(DateTime.Now.Second * 896, ref sClave);
                ToExcelIndexCol(DateTime.Now.Millisecond, ref sClave);
                sClave = sClave + DateTime.Now.Millisecond.ToString();
                ToExcelIndexCol(DateTime.Now.Second * 887, ref sClave);
                ToExcelIndexCol(DateTime.Now.Day, ref sClave);
                ToExcelIndexCol(DateTime.Now.Millisecond * 788, ref sClave);
                ToExcelIndexCol(DateTime.Now.Minute, ref sClave);
                return sClave;
            }
            public void ToExcelIndexCol(int n, ref string res)
            {
                if (n == 0)
                {
                    //Console.Write("Z");
                    return;
                }
                else
                {
                    int r = n % 59;
                    n = n / 59;
                    if (r == 0)
                        ToExcelIndexCol(n - 1, ref res);
                    else
                        ToExcelIndexCol(n, ref res);
                    if (r == 0)
                    {
                        res += "Z";
                        if (n == 1)
                            return;
                    }
                    switch (r)
                    {
                        case 1:
                            res += "A";
                            break;
                        case 2:
                            res += "B";
                            break;
                        case 3:
                            res += "C";
                            break;
                        case 4:
                            res += "D";
                            break;
                        case 5:
                            res += "E";
                            break;
                        case 6:
                            res += "F";
                            break;
                        case 7:
                            res += "G";
                            break;
                        case 8:
                            res += "H";
                            break;
                        case 9:
                            res += "I";
                            break;
                        case 10:
                            res += "J";
                            break;
                        case 11:
                            res += "K";
                            break;
                        case 12:
                            res += "L";
                            break;
                        case 13:
                            res += "M";
                            break;
                        case 14:
                            res += "N";
                            break;
                        case 15:
                            res += "O";
                            break;
                        case 16:
                            res += "P";
                            break;
                        case 17:
                            res += "Q";
                            break;
                        case 18:
                            res += "R";
                            break;
                        case 19:
                            res += "S";
                            break;
                        case 20:
                            res += "T";
                            break;
                        case 21:
                            res += "U";
                            break;
                        case 22:
                            res += "V";
                            break;
                        case 23:
                            res += "W";
                            break;
                        case 24:
                            res += "X";
                            break;
                        case 25:
                            res += "Y";
                            break;
                        case 26:
                            res += "Z";
                            break;
                        case 27:
                            res += "a";
                            break;
                        case 28:
                            res += "b";
                            break;
                        case 29:
                            res += "c";
                            break;
                        case 30:
                            res += "d";
                            break;
                        case 31:
                            res += "e";
                            break;
                        case 32:
                            res += "f";
                            break;
                        case 33:
                            res += "g";
                            break;
                        case 34:
                            res += "h";
                            break;
                        case 35:
                            res += "i";
                            break;
                        case 36:
                            res += "j";
                            break;
                        case 37:
                            res += "k";
                            break;
                        case 38:
                            res += "l";
                            break;
                        case 39:
                            res += "m";
                            break;
                        case 40:
                            res += "n";
                            break;
                        case 41:
                            res += "o";
                            break;
                        case 42:
                            res += "p";
                            break;
                        case 43:
                            res += "q";
                            break;
                        case 44:
                            res += "r";
                            break;
                        case 45:
                            res += "s";
                            break;
                        case 46:
                            res += "t";
                            break;
                        case 47:
                            res += "u";
                            break;
                        case 48:
                            res += "v";
                            break;
                        case 49:
                            res += "w";
                            break;
                        case 50:
                            res += "x";
                            break;
                        case 51:
                            res += "y";
                            break;
                        case 52:
                            res += "z";
                            break;
                        case 53:
                            res += "1";
                            break;
                        case 54:
                            res += "2";
                            break;
                        case 55:
                            res += "3";
                            break;
                        case 56:
                            res += "4";
                            break;
                        case 57:
                            res += "5";
                            break;
                        case 58:
                            res += "6";
                            break;
                        case 59:
                            res += "7";
                            break;
                        case 60:
                            res += "8";
                            break;
                    }
                }
            }

        }

        #endregion
        public class cMail
        {
            public string Server { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public int Port { get; set; }
            public string Para { get; set; }
            public string De { get; set; }
            public string Asunto { get; set; }
            public string Copias { get; set; }
            public string Cuerpo { get; set; }
            public string Archivos { get; set; }
            public bool SSL { get; set; } = true;
            public cResul R { get; set; } = new cResul();

            public cMail()
            {
                Val oVal = new Val();
                cCatalogo oCatalogo = new cCatalogo();
                string[] config = oCatalogo.Configuracion("Web MailO").Split(',');
                Server = config[0];
                Port = oVal.Entero(config[1]);
                User = config[2];
                Password = config[3];
                if (config[4].Equals("true"))
                    SSL = true;
                Copias = config[6];
                oCatalogo = null;
            }

            #region Metods
            public cResul EnviarCorreo()
            {

                cResul R = new cResul();
                System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();

                if (Para.IndexOf('@') == -1 || Para.IndexOf('.') == -1)
                {
                    R.mensaje = "Formato de correo no válido";
                    return R;
                }
                try
                {
                    correo.From = new System.Net.Mail.MailAddress(User);
                    correo.To.Clear();
                    correo.Bcc.Clear();
                    correo.To.Add(Para);
                    correo.Bcc.Add(User);
                    if (!string.IsNullOrEmpty(Copias))
                    {
                        String[] arrCC = Copias.Split(';');

                        for (int i = 0; i < arrCC.Length; i++)
                        {
                            if (arrCC[i].ToString().Trim() != "")
                            {
                                correo.CC.Add(arrCC[i].ToString());
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(Archivos))
                    {
                        string[] arr = Archivos.Split(';');
                        foreach (string str in arr)
                        {
                            System.Net.Mail.Attachment oAttch = new System.Net.Mail.Attachment(str, MediaTypeNames.Application.Octet);
                            correo.Attachments.Add(oAttch);

                        }
                    }
                    correo.Subject = Asunto;
                    correo.Body = Cuerpo;
                    correo.IsBodyHtml = true;
                    correo.Priority = System.Net.Mail.MailPriority.High;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = Server;
                    smtp.Port = Port;
                    smtp.EnableSsl = SSL;
                    correo.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnSuccess;
                    smtp.Credentials = new System.Net.NetworkCredential(User, Password);
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Send(correo);

                    R.mensaje = "Mensaje enviado a " + Para;
                }
                catch (Exception ex)
                {
                    R.mensaje = "ERROR: " + ex.Message;

                }
                return R;
            }
            #endregion
            public class Val
            {
                public int Entero(string valor)
                {
                    int resultado;
                    if (int.TryParse(valor, out resultado))
                        return resultado;
                    return 0;
                }
            }

        }

    }
}