using System;
// using System.Net;

namespace dotnet.Libraries.Utilities
{
    public class GeocodeHelper
    {
        public string ReverseGeocode((decimal Latitude, decimal Longitude) coordinates)
        {
            return "Brooklyn New York";
            /*
            var text = "Dundee, Scotland";
            var apiKey = "AIzaSyCOhg9ahueeKSwEzOKM6OZGxD226Ry3D5Y";
            text = WebUtility.UrlEncode(text.Replace(" ", "+"));
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={text}&sensor=false&key={apiKey}";

            var cacert="c:/wwwroot/cacert.pem";
            */

            /*
            $ch = curl_init();
            curl_setopt($ch, CURLOPT_URL, $url );
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1 );
            curl_setopt($ch, CURLOPT_CAINFO, realpath( $cacert ) );
            curl_setopt($ch, CURLOPT_SSL_VERIFYHOST,true );
            curl_setopt($ch, CURLOPT_SSL_VERIFYPEER,2 );

            $response = json_decode( curl_exec( $ch ), true );
            $info = curl_getinfo( $ch );
            curl_close( $ch );


            echo '<pre>', print_r($info,1), PHP_EOL, print_r( $response, 1 ), '</pre>';
            */


        }
/*
        private HttpClientHandler GetHttpHandler()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                                                          SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                                                          ClientCertificateOptions = ClientCertificateOption.Manual
            };
            var cert = GetCertificate();
            if (cert.FriendlyName != "Empty") handler.ClientCertificates.Add(cert);
            return handler;
        }

        private X509Certificate2 GetCertificate()
        {
            using (var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certStore.Open(OpenFlags.ReadOnly);
                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, _siteSettings.UnipagosSettings.CertThumbprint, false);

                if (certCollection.Count > 0)
                {
                    return certCollection[0];
                }
            }

            return new X509Certificate2 { FriendlyName = "Empty", };
        }
*/
                 

    }
}
