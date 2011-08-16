using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;    

namespace iActivator
{
    public class SslTcpClient
        {
            private static Hashtable certificateErrors = new Hashtable();
            public static bool ValidateServerCertificate(
                  object sender,
                  X509Certificate certificate,
                  X509Chain chain,
                  SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;
                
                return false;
            }
            public static string RunClient(string serverName,string activation_info,ref string buffer)
            {                                
                TcpClient client = new TcpClient(serverName,443);                
                SslStream sslStream = new SslStream(
                    client.GetStream(),
                    false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate),
                    null
                    );
              
                try
                {
                    sslStream.AuthenticateAsClient(serverName);
                }
                catch (AuthenticationException e)
                {   
                    if (e.InnerException != null)
                    {
                    }
                    client.Close();
                    Environment.Exit(-1);
                }

                byte[] messsage = Encoding.UTF8.GetBytes(activation_info + "\n<EOF>");                
                sslStream.Write(messsage);
                sslStream.Flush();               
                string serverMessage = ReadMessage(sslStream);                
                client.Close();                
                buffer = serverMessage;
                return serverMessage;
            }
            static string ReadMessage(SslStream sslStream)
            {
              
                byte[] buffer = new byte[2048];
                StringBuilder messageData = new StringBuilder();
                int bytes = -1;
                do
                {
                    bytes = sslStream.Read(buffer, 0, buffer.Length);                    
                    Decoder decoder = Encoding.UTF8.GetDecoder();
                    char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                    decoder.GetChars(buffer, 0, bytes, chars, 0);                    
                    messageData.Append(chars);
                    if (messageData.ToString().Contains(@"</Document>") == true)
                    {
                        break;
                    }
                } while (bytes != 0);

                return messageData.ToString();
            }            
        }
    }
    

