using System.ComponentModel.DataAnnotations;
namespace qualityservice.Model
{
    public class MessageCalculates
    {
        [Key]
        public int messageId{get;set;}
        public string key{get;set;}
        public string value{get;set;}
    }
}