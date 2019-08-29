using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BrowserTrayApp
{
    public partial class MainForm : Form
    {
        private WebSocketServer server;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new WebSocketServer("wss://localhost:1982/")
            {
                SslConfiguration ={
                    ServerCertificate = CreateSelfSignedCertificate(),
                    CheckCertificateRevocation=false,
                    ClientCertificateRequired=false
                }
            };
            server.AddWebSocketService<EchoService>("/echo");
            server.Start();

            notifyIcon1.Icon = Resource1.Icon1;
            notifyIcon1.ShowBalloonTip(3000, "Browser Tray Application", "The server is running.", ToolTipIcon.Info);
        }

        private X509Certificate2 CreateSelfSignedCertificate()
        {
            var cert = GenCert.CertificateGenerator.Create("foo", DateTime.Today, DateTime.Today.AddYears(10), "password", true, ".");
            return cert;
        }
    }

    class EchoService : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.WriteLine("Client sent: " + e.Data);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }
    }
}
