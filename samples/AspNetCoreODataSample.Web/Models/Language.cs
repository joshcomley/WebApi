using System.Collections.Generic;

namespace AspNetCoreODataSample.Web.Models
{
    public class Language
    {
        public List<MovieLanguage> Movies { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}