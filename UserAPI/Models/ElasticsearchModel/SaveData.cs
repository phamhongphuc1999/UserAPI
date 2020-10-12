using Nest;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.ElasticsearchModel
{
    public class SaveData<TDocument>
    {
        [Required]
        public TDocument data;

        public Id id;
    }
}
