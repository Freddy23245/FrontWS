using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PracticaFrontWS.Core
{
    public class BaseService
    {
  

        public PracticaWS.PracticaWSSoapClient ObtenerPracticaWS()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(ApplicationSettings.URLPracticaWS));
            var WsSoapClient = new PracticaWS.PracticaWSSoapClient(binding, endpoint);
            return WsSoapClient;
        }
    }
}
