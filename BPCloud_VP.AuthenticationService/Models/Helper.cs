using System.Security.Cryptography;
using System.Xml.Serialization;

namespace BPCloud_VP.AuthenticationService.Models
{
    public class Helper
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
    public class ITEM
    {
        public string METERIAL { get; set; }
        public string METERIALDESC { get; set; }
        public string DELDATE { get; set; }
        public string ORDERQTY { get; set; }
        public string GRQTY { get; set; }
        public string PIPELINEQTY { get; set; }
        public string OPENQTY { get; set; }
        public string ASNQTY { get; set; }
        public string BATCH { get; set; }
        public string MFDATE { get; set; }
        public string EXPDATE { get; set; }
    }
    [XmlRoot(ElementName = "SHIPITEMS")]
    public class SHIPITEMS
    {
        public List<ITEM> ITEM { get; set; }
    }

    [XmlRoot(ElementName = "LIST")]
    public class LIST
    {
        public string INVNO { get; set; }
        public string DATE { get; set; }
        public string VALUE { get; set; }
        public string CUR { get; set; }
    }

    [XmlRoot(ElementName = "INVLIST")]
    public class INVLIST
    {
        [XmlElement(ElementName = "LIST")]
        public List<LIST> LIST { get; set; }
    }

    [XmlRoot(ElementName = "ASNDATA")]
    public class ASNDATA
    {
        public string VID { get; set; }
        public string LEGALENTITY { get; set; }
        public string ADD1 { get; set; }
        public string ADD2 { get; set; }
        public string PO { get; set; }
        public string PODATE { get; set; }
        public string ASN { get; set; }
        public string MOT { get; set; }
        public string DEPDATE { get; set; }
        public string ARRDATE { get; set; }
        public string SHIPPINGAGENCY { get; set; }
        public string TRUCKNNO { get; set; }
        public string NETWEIGHT { get; set; }
        public string GROSSWEIGHT { get; set; }
        public string COUNTRYOFORGIN { get; set; }
        public string AWBNO { get; set; }
        public string AWBDATE { get; set; }
        public string BOL { get; set; }
        public string TRANPORTER { get; set; }
        public string CONPERSON { get; set; }
        public string CONNO { get; set; }
        public SHIPITEMS SHIPITEMS { get; set; }
        public INVLIST INVLIST { get; set; }
    }
}
