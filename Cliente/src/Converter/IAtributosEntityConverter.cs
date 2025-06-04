using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Model;

namespace Cliente.src.Converter
{
    public class IAtributosEntityConverter : JsonConverter<IEnumerable<IAtributosEntity>>
    {
        public override void WriteJson(JsonWriter writer, IEnumerable<IAtributosEntity>? value, JsonSerializer serializer)
        {
            var lista = value as IEnumerable<AtributosEntity>;
            serializer.Serialize(writer, lista);
        }

        public override IEnumerable<IAtributosEntity>? ReadJson(JsonReader reader, Type objectType, IEnumerable<IAtributosEntity>? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var lista = array.ToObject<ObservableCollection<AtributosEntity>>(serializer);
            return lista!;
        }
       
    }
}
